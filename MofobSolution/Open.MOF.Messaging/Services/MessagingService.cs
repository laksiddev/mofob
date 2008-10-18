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
        private static ServiceConfigurationSettings _configurationSettings = null;
        private static SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>> _serviceConfigurationLookup = null;

        protected string _serviceBindingName;

        protected MessagingService(string serviceBindingName) 
        {
            _serviceBindingName = serviceBindingName;
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

        public static MessagingService CreateInstance(System.Type messageType)
        {
            if ((messageType.BaseType.IsGenericType) && (typeof(TransactionRequestMessage<>).IsAssignableFrom(messageType.BaseType.GetGenericTypeDefinition())))
            {
                return CreateInstance(ServiceInterfaceType.TransactionService);
            }
            else if ((messageType.BaseType.IsGenericType) && (typeof(DataRequestMessage<>).IsAssignableFrom(messageType.BaseType.GetGenericTypeDefinition())))
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
            if (_configurationSettings == null)
                InitializeSettings();
            if ((_configurationSettings == null) || (_configurationSettings.ServiceConfigurationItems.Count == 0))
            {
                EventLogUtility.LogWarningMessage("Data services were not configured in the application config file.");
                return null;
            }

            MessagingService serviceInstance = null;
            if (_serviceConfigurationLookup.ContainsKey(interfaceType))
            {
                SortedList<int, ServiceConfigurationElement> interfaceLookup = _serviceConfigurationLookup[interfaceType];
                for (int i = 0; i < interfaceLookup.Keys.Count; i++)
                {
                    int iKey = interfaceLookup.Keys[i];
                    ServiceConfigurationElement item = interfaceLookup[iKey];
                    serviceInstance = TryCreateInstance(item);
                    if (serviceInstance != null)
                        break;
                }
            }
            
            return serviceInstance;
        }

        public static MessagingService CreateInstance(string serviceConfigurationName)
        {
            if (_configurationSettings == null)
                InitializeSettings();
            if ((_configurationSettings == null) || (_configurationSettings.ServiceConfigurationItems.Count == 0))
            {
                EventLogUtility.LogWarningMessage("Data services were not configured in the application config file.");
                return null;
            }

            MessagingService serviceInstance = null;
            foreach (ServiceConfigurationElement item in _configurationSettings.ServiceConfigurationItems)
            {
                if (String.Compare(item.Name, serviceConfigurationName, StringComparison.CurrentCultureIgnoreCase) == 0)
                {
                    serviceInstance = TryCreateInstance(item);
                    if (serviceInstance != null)
                        break;
                }
            }

            return serviceInstance;
        }

        private static MessagingService TryCreateInstance(ServiceConfigurationElement item)
        {
            MessagingService serviceInstance = null;
            if (typeof(MessagingService).IsAssignableFrom(item.ServiceType)) 
            {
                System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
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

        private static void InitializeSettings()
        {
            _configurationSettings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");

            _serviceConfigurationLookup = new SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>>();
            foreach (ServiceConfigurationElement item in _configurationSettings.ServiceConfigurationItems)
            {
                AddServiceToLookup(_serviceConfigurationLookup, item);
            }
        }

        private static void AddServiceToLookup(SortedList<ServiceInterfaceType, SortedList<int, ServiceConfigurationElement>> serviceConfigurationLookup, ServiceConfigurationElement item)
        {
            SortedList<int, ServiceConfigurationElement> interfaceLookup;
            if (serviceConfigurationLookup.ContainsKey(item.InterfaceType))
            {
                interfaceLookup = serviceConfigurationLookup[item.InterfaceType];
            }
            else
            {
                interfaceLookup = new SortedList<int, ServiceConfigurationElement>();
                serviceConfigurationLookup.Add(item.InterfaceType, interfaceLookup);
            }

            if (!interfaceLookup.ContainsKey(item.PreferenceNumber))
            {
                interfaceLookup.Add(item.PreferenceNumber, item);
            }   // else ignore additional types with the same preference number
        }
    }
}
