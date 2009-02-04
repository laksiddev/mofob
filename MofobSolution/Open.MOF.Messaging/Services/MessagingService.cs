using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public abstract class MessagingService : IDisposable
    {
        private static MessagingServiceLocator _locator = null;

        protected string _bindingName;

        protected MessagingService(string bindingName) 
        {
            _bindingName = bindingName;
        }

        public string BindingName
        {
            get { return _bindingName; }
        }

        public MessageBase SubmitMessage(MessageBase message)
        {
            return SubmitMessage(message, null);
        }

        public MessageBase SubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            IAsyncResult ar = BeginSubmitMessage(message, messageResponseCallback, null);
            return EndSubmitMessage(ar); 
        }

        public IAsyncResult BeginSubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            if (messageDeliveredCallback != null)
            {
                MessagingResult messagingResult = new MessagingResult(message);
                IAsyncResult asyncResult = new AsyncResult<MessagingResult>(messageDeliveredCallback, messagingResult);
                System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(PerformSubmitMessageAsync), asyncResult);

                return asyncResult;
            }
            else
            {
                MessagingResult messagingResult = new MessagingResult(message);
                AsyncResult<MessagingResult> asyncResult = new AsyncResult<MessagingResult>(messageDeliveredCallback, messagingResult);
                messagingResult = PerformSubmitMessage(message);
                asyncResult.SetAsCompleted(messagingResult, true);

                return asyncResult;
            }
        }

        protected void PerformSubmitMessageAsync(object state)
        {
            AsyncResult<MessagingResult> asyncResult = (AsyncResult<MessagingResult>)state;
            MessagingResult messagingResult = (MessagingResult)asyncResult.AsyncState;
            MessageBase requestMessage = messagingResult.RequestMessage;

            try
            {
                messagingResult = PerformSubmitMessage(requestMessage);
            }
            catch (Exception ex)
            {
                EventLogUtility.LogException(ex);
                asyncResult.SetAsCompleted(ex, false);
            }

            if (!asyncResult.IsCompleted)    // this would only show completed after an exception
            {
                asyncResult.SetAsCompleted(messagingResult, false);
            }
        }

        protected abstract MessagingResult PerformSubmitMessage(MessageBase message);

        public MessageBase EndSubmitMessage(IAsyncResult ar)
        {
            MessagingResult messagingResult = ((AsyncResult<MessagingResult>)ar).EndInvoke();

            return messagingResult.ResponseMessage;
        }

        public abstract ServiceInterfaceType SuportedServiceInterfaces { get; }

        public bool CanSupportInterface(ServiceInterfaceType interfaceType)
        {
            return ((SuportedServiceInterfaces & interfaceType) != 0);
        }

        public abstract void Dispose();

        public static MessagingService CreateInstance<TMessage>()
        {
            return CreateInstance(typeof(TMessage));
        }

        public static MessagingService CreateInstance(System.Type messageType)
        {
            if ((messageType.BaseType.IsGenericType) &&
                (typeof(TransactionRequestMessage<>).IsAssignableFrom(messageType.BaseType.GetGenericTypeDefinition())))
            {
                return CreateInstance(ServiceInterfaceType.TransactionService);
            }
            else if ((messageType.BaseType.IsGenericType) && 
                (typeof(DataRequestMessage<>).IsAssignableFrom(messageType.BaseType.GetGenericTypeDefinition())))
            {
                return CreateInstance(ServiceInterfaceType.DataService);
            }
            else if (messageType == typeof(FaultMessage))
            {
                return CreateInstance(ServiceInterfaceType.ExceptionService);
            }
            else if ((messageType == typeof(SubscribeRequestMessage)) || (messageType == typeof(UnsubscribeRequestMessage)))    
            {
                return CreateInstance(ServiceInterfaceType.SubscriptionService);
            }

            return null;
        }

        public static MessagingService CreateInstance(ServiceInterfaceType interfaceType)
        {
            if (_locator == null)
            {
                _locator = new MessagingServiceLocator();
            }
            
            List<string> instanceNames = _locator.ResolveInstanceNames(interfaceType);
            if ((instanceNames == null) || (instanceNames.Count == 0))
                return null;

            MessagingService service = CreateInstance(instanceNames[0]);
            if (service != null)
            {
                if (!service.CanSupportInterface(interfaceType))
                {
                    throw new MessagingConfigurationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", service.GetType().FullName, interfaceType.ToString()));
                }
            }

            return service;
        }

        public static MessagingService CreateInstance(string instanceName)
        {
            if (_locator == null)
            {
                _locator = new MessagingServiceLocator();
            }

            MessagingService service = null;
            if (!String.IsNullOrEmpty(instanceName))
            {
                service = _locator.GetInstance<MessagingService>(instanceName);
            }

            return service;
        }
    }
}
