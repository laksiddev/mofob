using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    public class ServiceConfigurationElement : ConfigurationElement
    {
        public ServiceConfigurationElement()
        {
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("serviceInterfaceName", IsRequired = true)]
        public string ServiceInterfaceName
        {
            get { return (string)this["serviceInterfaceName"]; }
            set { this["serviceInterfaceName"] = value; }
        }

        public Open.MOF.Messaging.Services.ServiceInterfaceType InterfaceType
        {
            get
            {
                if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.DataService.ToString(), ServiceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return Open.MOF.Messaging.Services.ServiceInterfaceType.DataService;
                else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService.ToString(), ServiceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService;
                else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService.ToString(), ServiceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService;
                else if (String.Compare(Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService.ToString(), ServiceInterfaceName, StringComparison.CurrentCultureIgnoreCase) == 0)
                    return Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService;
                else
                    return Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService;
            }
        }

        [ConfigurationProperty("serviceTypeName", IsRequired = true)]
        public string ServiceTypeName
        {
            get { return (string)this["serviceTypeName"]; }
            set { this["serviceTypeName"] = value; }
        }

        public System.Type ServiceType
        {
            get
            {
                if (String.IsNullOrEmpty(ServiceTypeName))
                    throw new ApplicationException("The service type name was not properly defined.");

                System.Type serviceType = null;
                try
                {
                    serviceType = System.Type.GetType(ServiceTypeName);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while attempting to retrieve the Service Type definition.", ex);
                }

                return serviceType;
            }
        }   //

        [ConfigurationProperty("channelEndpointName", IsRequired = true)]
        public string ChannelEndpointName
        {
            get { return (string)this["channelEndpointName"]; }
            set { this["channelEndpointName"] = value; }
        }

        [ConfigurationProperty("preferenceNumber", IsRequired = true)]
        public int PreferenceNumber
        {
            get { return (int)this["preferenceNumber"]; }
            set { this["preferenceNumber"] = value; }
        }
    }
}
