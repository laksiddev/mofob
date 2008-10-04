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
    }
}
