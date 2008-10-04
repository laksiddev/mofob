using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    [Serializable]
    public class ServiceConfigurationSettings : ConfigurationSection
    {
        public ServiceConfigurationSettings()
        {
        }

        [ConfigurationProperty("serviceConfigurationItems", IsRequired = true)]
        [ConfigurationCollection(typeof(ServiceConfigurationCollection))]
        public ServiceConfigurationCollection ServiceConfigurationItems
        {
            get { return (ServiceConfigurationCollection)this["serviceConfigurationItems"]; }
            set { this["serviceConfigurationItems"] = value; }
        }
    }
}
