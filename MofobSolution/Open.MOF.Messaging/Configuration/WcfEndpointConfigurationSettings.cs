using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    [Serializable]
    public class WcfEndpointConfigurationSettings : ConfigurationSection
    {
        public WcfEndpointConfigurationSettings()
        {
        }

        [ConfigurationProperty("wcfEndpointConfigurationItems", IsRequired = true)]
        [ConfigurationCollection(typeof(WcfEndpointConfigurationCollection))]
        public WcfEndpointConfigurationCollection WcfEndpointConfigurationItems
        {
            get { return (WcfEndpointConfigurationCollection)this["wcfEndpointConfigurationItems"]; }
            set { this["wcfEndpointConfigurationItems"] = value; }
        }
    }
}
