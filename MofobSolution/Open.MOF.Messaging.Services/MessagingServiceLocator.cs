using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Configuration;

namespace Open.MOF.Messaging.Services
{
    public class MessagingServiceLocator : ServiceLocatorImplBase
    {
        private IUnityContainer _container = null;
        private SortedList<ServiceInterfaceType, SortedList<int, string>> _serviceConfigurationLookup = null;
       
        public MessagingServiceLocator()
        {
        }
        
        public List<string> ResolveInstanceNames(ServiceInterfaceType interfaceType)
        {
            if (_serviceConfigurationLookup == null)
                InitializeContainer();

            SortedList<int, string> instanceNames = new SortedList<int, string>();
            if ((interfaceType & ServiceInterfaceType.TransactionService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(ServiceInterfaceType.TransactionService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[ServiceInterfaceType.TransactionService];
                    foreach (int preferenceNumber in serviceConfigurationInstances.Keys)
                    {
                        if (!instanceNames.ContainsKey(preferenceNumber))
                        {
                            instanceNames.Add(preferenceNumber, serviceConfigurationInstances[preferenceNumber]);
                        }
                        else
                            throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique between similar service interface types.");
                    }
                }
            }
            if ((interfaceType & ServiceInterfaceType.DataService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(ServiceInterfaceType.DataService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[ServiceInterfaceType.DataService];
                    foreach (int preferenceNumber in serviceConfigurationInstances.Keys)
                    {
                        if (!instanceNames.ContainsKey(preferenceNumber))
                        {
                            instanceNames.Add(preferenceNumber, serviceConfigurationInstances[preferenceNumber]);
                        }
                        else
                            throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique between similar service interface types.");
                    }
                }
            }
            if ((interfaceType & ServiceInterfaceType.ExceptionService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(ServiceInterfaceType.ExceptionService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[ServiceInterfaceType.ExceptionService];
                    foreach (int preferenceNumber in serviceConfigurationInstances.Keys)
                    {
                        if (!instanceNames.ContainsKey(preferenceNumber))
                        {
                            instanceNames.Add(preferenceNumber, serviceConfigurationInstances[preferenceNumber]);
                        }
                        else
                            throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique between similar service interface types.");
                    }
                }
            }
            if ((interfaceType & ServiceInterfaceType.SubscriptionService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(ServiceInterfaceType.SubscriptionService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[ServiceInterfaceType.SubscriptionService];
                    foreach (int preferenceNumber in serviceConfigurationInstances.Keys)
                    {
                        if (!instanceNames.ContainsKey(preferenceNumber))
                        {
                            instanceNames.Add(preferenceNumber, serviceConfigurationInstances[preferenceNumber]);
                        }
                        else
                            throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique between similar service interface types.");
                    }
                }
            }

            return instanceNames.Values.ToList();
        }

        private void InitializeContainer()
        {
            _container = new UnityContainer();
            _serviceConfigurationLookup = new SortedList<ServiceInterfaceType, SortedList<int, string>>();
            ServiceConfigurationSettings configurationSettings = (ServiceConfigurationSettings)ConfigurationManager.GetSection("messagingServiceConfiguration");

            foreach (ServiceConfigurationElement item in configurationSettings.ServiceConfigurationItems)
            {
                // HACK RegisterType does not work with non-parameterless constructors
                // So we have to new-up our own instances using reflection and use RegisterInstance insted

                IMessagingService service = TryCreateInstance(item);
                if (service != null)
                {
                    _container.RegisterInstance<IMessagingService>(item.Name, service, new ContainerControlledLifetimeManager());
                    ServiceInterfaceType interfaceType = MessagingService.ServiceInterfaceLookup(item.ServiceInterfaceName);
                    RegisterInstanceName(interfaceType, item.Name, item.PreferenceNumber, service.GetType());
                }
            }
        }

        private static IMessagingService TryCreateInstance(ServiceConfigurationElement item)
        {
            IMessagingService serviceInstance = null;
            if (typeof(IMessagingService).IsAssignableFrom(item.ServiceType))
            {
                System.Reflection.BindingFlags flags = System.Reflection.BindingFlags.CreateInstance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
                System.Reflection.ConstructorInfo constructorMethod = item.ServiceType.GetConstructor(flags, null, new Type[] { typeof(string) }, null);
                if (constructorMethod != null)
                {
                    serviceInstance = (IMessagingService)constructorMethod.Invoke(new object[] { item.ChannelEndpointName });
                }
                else
                {
                    serviceInstance = (IMessagingService)Activator.CreateInstance(item.ServiceType, flags, null, new object[] { item.ChannelEndpointName }, System.Globalization.CultureInfo.CurrentCulture, null);
                }
            }

            ServiceInterfaceType interfaceType = MessagingService.ServiceInterfaceLookup(item.ServiceInterfaceName);
            if (!serviceInstance.CanSupportInterface(interfaceType))
                return null;

            return serviceInstance;
        }

        private void RegisterInstanceName(ServiceInterfaceType interfaceType, string instanceName, int preferenceNumber, Type instanceType)
        {
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
            else
                throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique.");
        }
        
        /// <summary>
        ///             When implemented by inheriting classes, this method will do the actual work of resolving
        ///             the requested service instance.
        /// </summary>
        /// <param name="serviceType">Type of instance requested.</param>
        /// <param name="key">Name of registered service you want. May be null.</param>
        /// <returns>
        /// The requested service instance.
        /// </returns>
        protected override object DoGetInstance(Type serviceType, string key)
        {
            if (_container == null)
                InitializeContainer();

            try
            {
                return _container.Resolve(serviceType, key);
            }
            catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore - return null */ }

            return null;
        }

        /// <summary>
        ///             When implemented by inheriting classes, this method will do the actual work of
        ///             resolving all the requested service instances.
        /// </summary>
        /// <param name="serviceType">Type of service requested.</param>
        /// <returns>
        /// Sequence of service instance objects.
        /// </returns>
        protected override IEnumerable<object> DoGetAllInstances(Type serviceType)
        {
            if (_container == null)
                InitializeContainer();

            try
            {
                return _container.ResolveAll(serviceType);
            }
            catch (Microsoft.Practices.Unity.ResolutionFailedException) { /* ignore - return null */ }

            return null;
        }
  }
}
