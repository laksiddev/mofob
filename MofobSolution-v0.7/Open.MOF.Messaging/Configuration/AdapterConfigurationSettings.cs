using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    [Serializable]
    public class AdapterConfigurationSettings : ConfigurationSection
    {
        public AdapterConfigurationSettings()
        {
        }

        [ConfigurationProperty("adapterConfigurationItems", IsRequired = true)]
        [ConfigurationCollection(typeof(AdapterConfigurationCollection))]
        public AdapterConfigurationCollection AdapterConfigurationItems
        {
            get { return (AdapterConfigurationCollection)this["adapterConfigurationItems"]; }
            set { this["adapterConfigurationItems"] = value; }
        }
    }
}
