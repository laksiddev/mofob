using System;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.IO;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;
using Open.MOF.Messaging.Callback;

namespace Open.MOF.Messaging.Adapters
{
    public abstract class MessagingAdapter : IMessagingAdapter //, IDisposable
    {
        protected string _channelEndpointName;
        protected EventHandler<MessageReceivedEventArgs> _messageResponseCallback;
        
        protected abstract bool NativeAsyncSupported { get; }

        public abstract string AdapterContext { get; }

        private MessageHandlingSummary _messageHandlingSummary;
        public MessageHandlingSummary MessageHandlingSummary
        {
            get { return _messageHandlingSummary; }
        }

        protected MessagingAdapter(string channelEndpointName) 
        {
            _channelEndpointName = channelEndpointName;
        }

        public string ChannelEndpointName
        {
            get { return _channelEndpointName; }
        }

        public SimpleMessage SubmitMessage(SimpleMessage message)
        {
            return SubmitMessage(message, null);
        }

        public SimpleMessage SubmitMessage(SimpleMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            IAsyncResult ar = BeginSubmitMessage(message, messageResponseCallback, null);
            return EndSubmitMessage(ar); 
        }

        public IAsyncResult BeginSubmitMessage(SimpleMessage requestMessage, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            _messageHandlingSummary = null;
            MessagingState messagingState = new MessagingState(requestMessage, messageResponseCallback);
            AsyncResult<MessagingState> asyncResult = new AsyncResult<MessagingState>(messageDeliveredCallback, messagingState);

            if (requestMessage is FrameworkMessage)
            {
                FrameworkMessage framewordRequestMessage = (FrameworkMessage)requestMessage;
                // For a Pub Sub type message expecting a non-TwoWay response, use a PubSub callback adapter
                if ((framewordRequestMessage.ResponseTypes != null) &&
                    (framewordRequestMessage.ResponseTypes.Count > 0) &&
                    (!framewordRequestMessage.RequiresTwoWay) &&
                    (framewordRequestMessage.MessageId.HasValue))
                {
                    if (messageResponseCallback != null)
                    {
                        IPubSubCallbackAdapter callbackAdapter = ServiceLocator.Current.GetInstance<IPubSubCallbackAdapter>();
                        callbackAdapter.RegisterCallbackHandler(framewordRequestMessage, messageResponseCallback);
                    }
                }
            }

            if (messageDeliveredCallback != null)
            {
                if (NativeAsyncSupported)
                {
                    // It is assumed that if NativeAsyncSupported = true, the adapter will override this method to perform Async
                    PerformSubmitMessageAsync(asyncResult);
                }
                else
                {
                    System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(PerformSubmitMessageAsync), asyncResult);
                }
            }
            else
            {
                messagingState = PerformSubmitMessage(requestMessage);
                asyncResult.SetAsCompleted(messagingState, true);

                // For a TwoWay message, signal that the response was received
                if ((requestMessage.RequiresTwoWay) && (messagingState.ResponseMessage != null) && (!(messagingState.ResponseMessage is MessageSubmittedResponse)))
                {
                    if (messageResponseCallback != null)
                        messageResponseCallback(this, new MessageReceivedEventArgs(messagingState.ResponseMessage));
                }
            }

            return asyncResult;
        }

        protected virtual void PerformSubmitMessageAsync(object state)
        {
            AsyncResult<MessagingState> asyncResult = (AsyncResult<MessagingState>)state;
            MessagingState messagingState = (MessagingState)asyncResult.AsyncState;
            SimpleMessage requestMessage = messagingState.RequestMessage;

            try
            {
                messagingState = PerformSubmitMessage(requestMessage);
            }
            catch (Exception ex)
            {
                EventLogUtility.LogException(ex);
                asyncResult.SetAsCompleted(ex, false);
            }

            if (!asyncResult.IsCompleted)    // this would only show completed after an exception
            {
                messagingState.HandlingSummary.ProcessedAsync = true;
                asyncResult.SetAsCompleted(messagingState, false);

                // For a TwoWay message, signal that the response was received
                if ((requestMessage.RequiresTwoWay) && (messagingState.ResponseMessage != null) && (!(messagingState.ResponseMessage is MessageSubmittedResponse)))
                {
                    if (messagingState.MessageResponseCallback != null)
                        messagingState.MessageResponseCallback(this, new MessageReceivedEventArgs(messagingState.ResponseMessage));
                }
            }
        }

        protected abstract MessagingState PerformSubmitMessage(SimpleMessage message);

        public SimpleMessage EndSubmitMessage(IAsyncResult ar)
        {
            MessagingState messagingState = ((AsyncResult<MessagingState>)ar).EndInvoke();
            _messageHandlingSummary = messagingState.HandlingSummary;

            return messagingState.ResponseMessage;
        }

        protected abstract AdapterInterfaceType SuportedAdapterInterfaces { get; }

        protected internal abstract bool CanSupportMessage(SimpleMessage message);

        public bool CanSupportInterface(AdapterInterfaceType interfaceType)
        {
            return ((SuportedAdapterInterfaces & interfaceType) != 0);
        }

        public abstract void Dispose();

        public static IMessagingAdapter CreateInstance(SimpleMessage message)
        {
            int preferenceOffset = 0;
            bool areServiceInstancesAvailable = true;
            while (areServiceInstancesAvailable)
            {
                MessagingAdapter adapter = (MessagingAdapter)CreateInstance(message.GetType(), preferenceOffset);
                if (adapter == null)
                {
                    areServiceInstancesAvailable = false;
                }
                else if (adapter.CanSupportMessage(message))
                {
                    return adapter;
                }
                else
                {
                    preferenceOffset++;
                }
            }

            EventLogUtility.LogWarningMessage(String.Format("WARNING:  No Messaging Service was located for the message type: {0}", message.GetType().FullName));
            return null;
        }

        protected internal static IMessagingAdapter CreateInstance<TMessage>()
        {
            return CreateInstance(typeof(TMessage));
        }

        protected internal static IMessagingAdapter CreateInstance(System.Type messageType)
        {
            return CreateInstance(messageType, 0);
        }

        protected internal static IMessagingAdapter CreateInstance(System.Type messageType, int preferenceOffset)
        {
            MessageBehavior messageBehavior = SimpleMessage.GetMessageBehavior(messageType);
            if (messageBehavior == MessageBehavior.TransactionsRequired)
            {
                return CreateInstance(AdapterInterfaceType.TransactionService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.DataReporting)
            {
                return CreateInstance(AdapterInterfaceType.DataService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.TransactionsSupported)
            {
                return CreateInstance(AdapterInterfaceType.DataService | AdapterInterfaceType.TransactionService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.FaultReporting)
            {
                return CreateInstance(AdapterInterfaceType.ExceptionService, preferenceOffset);
            }
            //else if (messageBehavior == MessageBehavior.SubscriptionControl)
            //{
            //    return CreateInstance(AdapterInterfaceType.SubscriptionService, preferenceOffset);
            //}

            EventLogUtility.LogWarningMessage("The ServiceInterfaceType for the message could not be determined.  This is typically due to the improper use of parent types for the message.");

            return null;
        }

        protected internal static IMessagingAdapter CreateInstance(AdapterInterfaceType interfaceType)
        {
            return CreateInstance(interfaceType, 0);
        }

        protected internal static IMessagingAdapter CreateInstance(AdapterInterfaceType interfaceType, int preferenceOffset)
        {
            IServiceLocator locator = MessagingAdapterLocator.GetLocatorInstance();
            IMessagingResolver resolver = locator.GetInstance<IMessagingResolver>();
            List<string> instanceNames = resolver.ResolveInstanceNames(interfaceType);
            if ((instanceNames == null) || (instanceNames.Count <= preferenceOffset))
                return null;

            IMessagingAdapter adapter = CreateInstance(instanceNames[preferenceOffset]);
            if (adapter != null)
            {
                if (!adapter.CanSupportInterface(interfaceType))
                {
                    throw new MessagingConfigurationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", adapter.GetType().FullName, interfaceType.ToString()));
                }
            }

            return adapter;
        }

        protected internal static IMessagingAdapter CreateInstance(string instanceName)
        {
            IServiceLocator locator = MessagingAdapterLocator.GetLocatorInstance();
            IMessagingAdapter adapter = null;
            if (!String.IsNullOrEmpty(instanceName))
            {
                adapter = locator.GetInstance<IMessagingAdapter>(instanceName);
            }

            return adapter;
        }

        protected internal static AdapterInterfaceType AdapterInterfaceLookup(string adapterInterfaceName)
        {
            if (String.Compare(Open.MOF.Messaging.Adapters.AdapterInterfaceType.DataService.ToString(), adapterInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Adapters.AdapterInterfaceType.DataService;
            else if (String.Compare(Open.MOF.Messaging.Adapters.AdapterInterfaceType.TransactionService.ToString(), adapterInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Adapters.AdapterInterfaceType.TransactionService;
            else if (String.Compare(Open.MOF.Messaging.Adapters.AdapterInterfaceType.ExceptionService.ToString(), adapterInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Adapters.AdapterInterfaceType.ExceptionService;
            //else if (String.Compare(Open.MOF.Messaging.Adapters.AdapterInterfaceType.SubscriptionService.ToString(), adapterInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
            //    return Open.MOF.Messaging.Adapters.AdapterInterfaceType.SubscriptionService;
            else
                return Open.MOF.Messaging.Adapters.AdapterInterfaceType.TransactionService;
        }
    }
}
