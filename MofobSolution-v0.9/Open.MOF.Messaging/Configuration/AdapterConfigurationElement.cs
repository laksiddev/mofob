using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Configuration
{
    public class AdapterConfigurationElement : ConfigurationElement
    {
        public AdapterConfigurationElement()
        {
        }

        [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
        public string Name
        {
            get { return (string)this["name"]; }
            set { this["name"] = value; }
        }

        [ConfigurationProperty("adapterInterfaceName", IsRequired = true)]
        public string AdapterInterfaceName
        {
            get { return (string)this["adapterInterfaceName"]; }
            set { this["adapterInterfaceName"] = value; }
        }

        [ConfigurationProperty("adapterTypeName", IsRequired = true)]
        public string AdapterTypeName
        {
            get { return (string)this["adapterTypeName"]; }
            set { this["adapterTypeName"] = value; }
        }

        public System.Type AdapterType
        {
            get
            {
                if (String.IsNullOrEmpty(AdapterTypeName))
                    throw new ApplicationException("The adapter type name was not properly defined.");

                System.Type adapterType = null;
                try
                {
                    adapterType = System.Type.GetType(AdapterTypeName);
                }
                catch (Exception ex)
                {
                    throw new ApplicationException("An error occurred while attempting to retrieve the Adapter Type definition.", ex);
                }

                return adapterType;
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
