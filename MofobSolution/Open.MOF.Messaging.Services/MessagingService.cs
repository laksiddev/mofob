using System;
using System.Configuration;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public abstract class MessagingService : IMessagingService //, IDisposable
    {
        private static MessagingServiceLocator _locator = null;

        protected string _channelEndpointName;

        protected MessagingService(string channelEndpointName) 
        {
            _channelEndpointName = channelEndpointName;
        }

        public string ChannelEndpointName
        {
            get { return _channelEndpointName; }
        }

        public FrameworkMessage SubmitMessage(FrameworkMessage message)
        {
            return SubmitMessage(message, null);
        }

        public FrameworkMessage SubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            IAsyncResult ar = BeginSubmitMessage(message, messageResponseCallback, null);
            return EndSubmitMessage(ar); 
        }

        public IAsyncResult BeginSubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
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
            FrameworkMessage requestMessage = messagingResult.RequestMessage;

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

        protected abstract MessagingResult PerformSubmitMessage(FrameworkMessage message);

        public FrameworkMessage EndSubmitMessage(IAsyncResult ar)
        {
            MessagingResult messagingResult = ((AsyncResult<MessagingResult>)ar).EndInvoke();

            return messagingResult.ResponseMessage;
        }

        protected abstract ServiceInterfaceType SuportedServiceInterfaces { get; }

        protected abstract bool CanSupportMessage(FrameworkMessage message);

        public bool CanSupportInterface(ServiceInterfaceType interfaceType)
        {
            return ((SuportedServiceInterfaces & interfaceType) != 0);
        }

        public abstract void Dispose();

        public static IMessagingService CreateInstance(FrameworkMessage message)
        {
            int preferenceOffset = 0;
            bool areServiceInstancesAvailable = true;
            while (areServiceInstancesAvailable)
            {
                MessagingService service = (MessagingService)CreateInstance(message.GetType(), preferenceOffset);
                if (service == null)
                {
                    areServiceInstancesAvailable = false;
                }
                else if (service.CanSupportMessage(message))
                {
                    return service;
                }
                else
                {
                    preferenceOffset++;
                }
            }

            EventLogUtility.LogWarningMessage(String.Format("WARNING:  No Messaging Service was located for the message type: {0}", message.GetType().FullName));
            return null;
        }

        protected internal static IMessagingService CreateInstance<TMessage>()
        {
            return CreateInstance(typeof(TMessage));
        }

        protected internal static IMessagingService CreateInstance(System.Type messageType)
        {
            return CreateInstance(messageType, 0);
        }

        protected internal static IMessagingService CreateInstance(System.Type messageType, int preferenceOffset)
        {
            MessageBehavior messageBehavior = FrameworkMessage.MessageBehaviorForType(messageType);
            if (messageBehavior == MessageBehavior.TransactionsRequired)
            {
                return CreateInstance(ServiceInterfaceType.TransactionService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.DataReporting)
            {
                return CreateInstance(ServiceInterfaceType.DataService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.TransactionsSupported)
            {
                return CreateInstance(ServiceInterfaceType.DataService | ServiceInterfaceType.TransactionService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.FaultReporting)
            {
                return CreateInstance(ServiceInterfaceType.ExceptionService, preferenceOffset);
            }
            else if (messageBehavior == MessageBehavior.SubscriptionControl)
            {
                return CreateInstance(ServiceInterfaceType.SubscriptionService, preferenceOffset);
            }

            EventLogUtility.LogWarningMessage("The ServiceInterfaceType for the message could not be determined.  This is typically due to the improper use of parent types for the message.");

            return null;
        }

        protected internal static IMessagingService CreateInstance(ServiceInterfaceType interfaceType)
        {
            return CreateInstance(interfaceType, 0);
        }

        protected internal static IMessagingService CreateInstance(ServiceInterfaceType interfaceType, int preferenceOffset)
        {
            if (_locator == null)
            {
                _locator = new MessagingServiceLocator();
            }
            
            List<string> instanceNames = _locator.ResolveInstanceNames(interfaceType);
            if ((instanceNames == null) || (instanceNames.Count <= preferenceOffset))
                return null;

            IMessagingService service = CreateInstance(instanceNames[preferenceOffset]);
            if (service != null)
            {
                if (!service.CanSupportInterface(interfaceType))
                {
                    throw new MessagingConfigurationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", service.GetType().FullName, interfaceType.ToString()));
                }
            }

            return service;
        }

        protected internal static IMessagingService CreateInstance(string instanceName)
        {
            if (_locator == null)
            {
                _locator = new MessagingServiceLocator();
            }

            IMessagingService service = null;
            if (!String.IsNullOrEmpty(instanceName))
            {
                service = _locator.GetInstance<IMessagingService>(instanceName);
            }

            return service;
        }

        internal protected static ServiceInterfaceType ServiceInterfaceLookup(string serviceInterfaceName)
        {
            if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.DataService.ToString(), serviceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Services.ServiceInterfaceType.DataService;
            else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService.ToString(), serviceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService;
            else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService.ToString(), serviceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService;
            else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService.ToString(), serviceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                return Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService;
            else
                return Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService;
        }

        protected static class WcfUtilities
        {
            private static ServiceModelSectionGroup _serviceModelGroup;
            private static Dictionary<string, Type> _serviceContracts;

            public static ChannelEndpointElement FindEndpointByName(string channelEndpointName)
            {
                Initialize();

                for (int i = 0; i < _serviceModelGroup.Client.Endpoints.Count; i++)
                {
                    if (_serviceModelGroup.Client.Endpoints[i].Name == channelEndpointName)
                        return _serviceModelGroup.Client.Endpoints[i];
                }

                return null;
            }

            public static Type GetChannelInterfaceType(ChannelEndpointElement channelEndpoint)
            {
                Initialize();

                if (_serviceContracts.ContainsKey(channelEndpoint.Contract))
                    return _serviceContracts[channelEndpoint.Contract];

                return null;
            }

            public static bool DoesAddressMatchBinding(string addressUri, string bindingType)
            {
                if (((addressUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) ||
                    (addressUri.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))) &&
                    ((bindingType == "wsHttpBinding") || (bindingType == "basicHttpBinding")))
                {
                    return true;
                }
                else if ((addressUri.StartsWith("net.tcp://", StringComparison.CurrentCultureIgnoreCase)) &&
                    (bindingType == "netTcpBinding"))
                {
                    return true;
                }

                return false;
            }

            private static void Initialize()
            {
                if (_serviceModelGroup == null)
                {
                    _serviceModelGroup = ServiceModelSectionGroup.GetSectionGroup(ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None));
                }

                if (_serviceContracts == null)
                {
                    _serviceContracts = new Dictionary<string, Type>();

                    Assembly[] loadedAssemblies = AppDomain.CurrentDomain.GetAssemblies();
                    foreach (Assembly assembly in loadedAssemblies)
                    {
                        Type[] assemblyTypes = assembly.GetTypes();
                        foreach (Type type in assemblyTypes)
                        {
                            if (type.IsInterface)
                            {
                                ServiceContractAttribute[] attributes = (ServiceContractAttribute[])type.GetCustomAttributes(typeof(ServiceContractAttribute), false);
                                if ((attributes != null) && (attributes.Length > 0))
                                {
                                    string configurationName;
                                    if (!String.IsNullOrEmpty(attributes[0].ConfigurationName))
                                    {
                                        configurationName = attributes[0].ConfigurationName;
                                    }
                                    else
                                    {
                                        configurationName = type.FullName;
                                    }

                                    _serviceContracts.Add(configurationName, type);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
