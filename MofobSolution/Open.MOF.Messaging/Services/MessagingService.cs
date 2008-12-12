using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

using Open.MOF.Messaging;
//using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public abstract class MessagingService : IDisposable
    {
        //private static ServiceConfigurationSettings _configurationSettings = null;
        //private static SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>> _serviceConfigurationLookup = null;
        private static IUnityContainer _container = null;

        protected string _serviceBindingName;

        public MessagingService(string serviceBindingName) 
        {
            _serviceBindingName = serviceBindingName;
        }

        public string ServiceBindingName
        {
            get { return _serviceBindingName; }
        }

        public void SubmitMessage(MessageBase message)
        {
            SubmitMessage(message, null);
        }

        public void SubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            BeginSubmitMessage(message, messageResponseCallback, null);
        }

        public abstract IAsyncResult BeginSubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        public abstract MessageBase EndSubmitMessage(IAsyncResult ar);

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
                return TryResolveInstance(ServiceInterfaceType.TransactionService);
            }
            else if ((messageType.BaseType.IsGenericType) && 
                (typeof(DataRequestMessage<>).IsAssignableFrom(messageType.BaseType.GetGenericTypeDefinition())))
            {
                return TryResolveInstance(ServiceInterfaceType.DataService);
            }
            else if (messageType == typeof(FaultMessage))
            {
                return TryResolveInstance(ServiceInterfaceType.ExceptionService);
            }
            else if ((messageType == typeof(SubscribeRequestMessage)) || (messageType == typeof(UnsubscribeRequestMessage)))    
            {
                return TryResolveInstance(ServiceInterfaceType.SubscriptionService);
            }

            return null;
        }

        private static MessagingService TryResolveInstance(ServiceInterfaceType interfaceType)
        {
            if (_container == null)
            {
                _container = new UnityContainer();
                UnityConfigurationSection section
                  = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
                section.Containers.Default.Configure(_container);
            }

            MessagingService service = null;
            try
            {
                service = _container.Resolve<MessagingService>(interfaceType.ToString() + "Type");
            }
            catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore */ }

            if (service != null)
            {
                if (!service.CanSupportInterface(interfaceType))
                {
                    throw new ApplicationException(String.Format("The configured Messaging Service {0} type does not support the {1} service interface.", service.GetType().FullName, interfaceType.ToString()));
                }
            }

            return service;
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

        //private static MessagingService TryCreateInstance(ServiceConfigurationElement item)
        //{
        //    MessagingService serviceInstance = null;
        //    if (typeof(MessagingService).IsAssignableFrom(item.ServiceType)) 
        //    {
        //        System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
        //        System.Reflection.ConstructorInfo constructorMethod = item.ServiceType.GetConstructor(flags, null, new Type[] { typeof(string) }, null);
        //        if (constructorMethod != null)
        //        {
        //            serviceInstance = (MessagingService)constructorMethod.Invoke(new object[] { item.ServiceBindingName });
        //        }
        //        else
        //        {
        //            serviceInstance = (MessagingService)Activator.CreateInstance(item.ServiceType, flags, null, new object[] { item.ServiceBindingName }, System.Globalization.CultureInfo.CurrentCulture, null);
        //        }
        //    }

        //    if (!serviceInstance.CanSupportInterface(item.InterfaceType))
        //        return null;

        //    return serviceInstance;
        //}

        //private static void InitializeSettings()
        //{
        //    _configurationSettings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");

        //    _serviceConfigurationLookup = new SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>>();
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
