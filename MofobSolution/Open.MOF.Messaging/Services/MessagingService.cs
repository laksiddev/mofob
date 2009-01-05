using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public abstract class MessagingService : IDisposable
    {
        //private static ServiceConfigurationSettings _configurationSettings = null;
        private static SortedList<ServiceInterfaceType, SortedList<int, string>> _serviceConfigurationLookup = null;
        private static IUnityContainer _container = null;

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
            string instanceName = ResolveInstanceName(interfaceType, 0);
            MessagingService service = CreateInstance(instanceName);
            if (service != null)
            {
                if (!service.CanSupportInterface(interfaceType))
                {
                    throw new ApplicationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", service.GetType().FullName, interfaceType.ToString()));
                }
            }

            return service;
        }

        public static MessagingService CreateInstance(string instanceName)
        {
            if (_container == null)
            {
                _container = InitializeContainer();
            }

            MessagingService service = null;
            if (!String.IsNullOrEmpty(instanceName))
            {
                try
                {
                    service = _container.Resolve<MessagingService>(instanceName);
                }
                catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore */ }
            }

            return service;
        }

        private static IUnityContainer InitializeContainer()
        {
            IUnityContainer container = new UnityContainer();
            ServiceConfigurationSettings configurationSettings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");

            foreach (ServiceConfigurationElement item in configurationSettings.ServiceConfigurationItems)
            {
                // HACK RegisterType does not work with non-parameterless constructors
                // So we have to new-up our own instances using reflection and use RegisterInstance insted

                MessagingService service = TryCreateInstance(item);
                if (service != null)
                {
                    container.RegisterInstance<MessagingService>(item.Name, service, new ContainerControlledLifetimeManager());
                    RegisterInstanceName(item.InterfaceType, item.Name, item.PreferenceNumber);
                }
            }

            return container;
        }

        //public static MessagingService CreateInstance(ServiceInterfaceType interfaceType)
        //{
        //    if (_configurationSettings == null)
        //        InitializeSettings();
        //    if ((_configurationSettings == null) || (_configurationSettings.ServiceConfigurationItems.Count == 0))
        //    {
        //        EventLogUtility.LogWarningMessage("Data services were not configured in the application config file.");
        //        return null;
        //    }

        //    MessagingService serviceInstance = null;
        //    if (_serviceConfigurationLookup.ContainsKey(interfaceType))
        //    {
        //        SortedList<int, ServiceConfigurationElement> interfaceLookup = _serviceConfigurationLookup[interfaceType];
        //        for (int i = 0; i < interfaceLookup.Keys.Count; i++)
        //        {
        //            int iKey = interfaceLookup.Keys[i];
        //            ServiceConfigurationElement item = interfaceLookup[iKey];
        //            serviceInstance = TryCreateInstance(item);
        //            if (serviceInstance != null)
        //                break;
        //        }
        //    }
            
        //    return serviceInstance;
        //}

        //public static MessagingService CreateInstance(string serviceConfigurationName)
        //{
        //    if (_configurationSettings == null)
        //        InitializeSettings();
        //    if ((_configurationSettings == null) || (_configurationSettings.ServiceConfigurationItems.Count == 0))
        //    {
        //        EventLogUtility.LogWarningMessage("Data services were not configured in the application config file.");
        //        return null;
        //    }

        //    MessagingService serviceInstance = null;
        //    foreach (ServiceConfigurationElement item in _configurationSettings.ServiceConfigurationItems)
        //    {
        //        if (String.Compare(item.Name, serviceConfigurationName, StringComparison.CurrentCultureIgnoreCase) == 0)
        //        {
        //            serviceInstance = TryCreateInstance(item);
        //            if (serviceInstance != null)
        //                break;
        //        }
        //    }

        //    return serviceInstance;
        //}

        private static MessagingService TryCreateInstance(ServiceConfigurationElement item)
        {
            MessagingService serviceInstance = null;
            if (typeof(MessagingService).IsAssignableFrom(item.ServiceType))
            {
                System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                System.Reflection.ConstructorInfo constructorMethod = item.ServiceType.GetConstructor(flags, null, new Type[] { typeof(string) }, null);
                if (constructorMethod != null)
                {
                    serviceInstance = (MessagingService)constructorMethod.Invoke(new object[] { item.ServiceBindingName });
                }
                else
                {
                    serviceInstance = (MessagingService)Activator.CreateInstance(item.ServiceType, flags, null, new object[] { item.ServiceBindingName }, System.Globalization.CultureInfo.CurrentCulture, null);
                }
            }

            if (!serviceInstance.CanSupportInterface(item.InterfaceType))
                return null;

            return serviceInstance;
        }

        private static void RegisterInstanceName(ServiceInterfaceType interfaceType, string instanceName, int preferenceNumber)
        {
            if (_serviceConfigurationLookup == null)
            {
                _serviceConfigurationLookup = new SortedList<ServiceInterfaceType, SortedList<int, string>>();
            }

            SortedList<int, string> innerLookup = null;
            if (_serviceConfigurationLookup.ContainsKey(interfaceType))
            {
                innerLookup = _serviceConfigurationLookup[interfaceType];
            }
            else
            {
                innerLookup = new SortedList<int, string>();
                _serviceConfigurationLookup.Add(interfaceType, innerLookup);
            }

            if (!innerLookup.ContainsKey(preferenceNumber))
            {
                innerLookup.Add(preferenceNumber, instanceName);
            }
        }

        private static string ResolveInstanceName(ServiceInterfaceType interfaceType, int preferenceSkipFactor)
        {
            if (_container == null)
            {
                _container = InitializeContainer();
            }

            string instanceName = string.Empty;
            if (_serviceConfigurationLookup.ContainsKey(interfaceType))
            {
                SortedList<int, string> innerLookup = _serviceConfigurationLookup[interfaceType];
                if (innerLookup.Count > preferenceSkipFactor)
                {
                    instanceName = innerLookup.Values[preferenceSkipFactor];
                }
            }            

            return instanceName;
        }

        //private static void InitializeSettings()
        //{
        //    _configurationSettings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");

        //    foreach (ServiceConfigurationElement item in _configurationSettings.ServiceConfigurationItems)
        //    {
        //        AddServiceToLookup(_serviceConfigurationLookup, item);
        //    }
        //}

        //private static void AddServiceToLookup(SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>> serviceConfigurationLookup, ServiceConfigurationElement item)
        //{
        //    SortedList<int, ServiceConfigurationElement> interfaceLookup;
        //    if (serviceConfigurationLookup.ContainsKey(item.InterfaceType))
        //    {
        //        interfaceLookup = serviceConfigurationLookup[item.InterfaceType];
        //    }
        //    else
        //    {
        //        interfaceLookup = new SortedList<int, ServiceConfigurationElement>();
        //        serviceConfigurationLookup.Add(item.InterfaceType, interfaceLookup);
        //    }

        //    if (!interfaceLookup.ContainsKey(item.PreferenceNumber))
        //    {
        //        interfaceLookup.Add(item.PreferenceNumber, item);
        //    }   // else ignore additional types with the same preference number
        //}
    }
}
