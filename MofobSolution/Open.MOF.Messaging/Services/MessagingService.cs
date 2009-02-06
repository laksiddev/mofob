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

        protected abstract ServiceInterfaceType SuportedServiceInterfaces { get; }

        public bool CanSupportInterface(ServiceInterfaceType interfaceType)
        {
            return ((SuportedServiceInterfaces & interfaceType) != 0);
        }

        public abstract void Dispose();

        public static IMessagingService CreateInstance<TMessage>()
        {
            return CreateInstance(typeof(TMessage));
        }

        public static IMessagingService CreateInstance(System.Type messageType)
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

        public static IMessagingService CreateInstance(ServiceInterfaceType interfaceType)
        {
            if (_locator == null)
            {
                _locator = new MessagingServiceLocator();
            }
            
            List<string> instanceNames = _locator.ResolveInstanceNames(interfaceType);
            if ((instanceNames == null) || (instanceNames.Count == 0))
                return null;

            IMessagingService service = CreateInstance(instanceNames[0]);
            if (service != null)
            {
                if (!service.CanSupportInterface(interfaceType))
                {
                    throw new MessagingConfigurationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", service.GetType().FullName, interfaceType.ToString()));
                }
            }

            return service;
        }

        public static IMessagingService CreateInstance(string instanceName)
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
