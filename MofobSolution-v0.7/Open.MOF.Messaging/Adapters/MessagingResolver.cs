using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging.Adapters
{
    internal class MessagingResolver : IMessagingResolver
    {
        private SortedList<AdapterInterfaceType, SortedList<int, string>> _serviceConfigurationLookup = null;
        
        public MessagingResolver()
        {
            _serviceConfigurationLookup = new SortedList<AdapterInterfaceType, SortedList<int, string>>();
        }

        public void RegisterInstanceName(AdapterInterfaceType interfaceType, string instanceName, int preferenceNumber, Type instanceType)
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

        public List<string> ResolveInstanceNames(AdapterInterfaceType interfaceType)
        {
            //if (_serviceConfigurationLookup == null)
            //    InitializeContainer();

            SortedList<int, string> instanceNames = new SortedList<int, string>();
            if ((interfaceType & AdapterInterfaceType.TransactionService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(AdapterInterfaceType.TransactionService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[AdapterInterfaceType.TransactionService];
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
            if ((interfaceType & AdapterInterfaceType.DataService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(AdapterInterfaceType.DataService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[AdapterInterfaceType.DataService];
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
            if ((interfaceType & AdapterInterfaceType.ExceptionService) != 0)
            {
                if (_serviceConfigurationLookup.ContainsKey(AdapterInterfaceType.ExceptionService))
                {
                    SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[AdapterInterfaceType.ExceptionService];
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
            //if ((interfaceType & AdapterInterfaceType.SubscriptionService) != 0)
            //{
            //    if (_serviceConfigurationLookup.ContainsKey(AdapterInterfaceType.SubscriptionService))
            //    {
            //        SortedList<int, string> serviceConfigurationInstances = _serviceConfigurationLookup[AdapterInterfaceType.SubscriptionService];
            //        foreach (int preferenceNumber in serviceConfigurationInstances.Keys)
            //        {
            //            if (!instanceNames.ContainsKey(preferenceNumber))
            //            {
            //                instanceNames.Add(preferenceNumber, serviceConfigurationInstances[preferenceNumber]);
            //            }
            //            else
            //                throw new MessagingConfigurationException("Duplicate preference numbers for messaging service configuration are not supported.  Preference numbers must be unique between similar service interface types.");
            //        }
            //    }
            //}

            return instanceNames.Values.ToList();
        }
    }
}
