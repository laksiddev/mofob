using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

namespace Open.MOF.Messaging.Configuration
{
    public class WcfEndpointConfigurationElement : ConfigurationElement
    {
        public WcfEndpointConfigurationElement()
        {
        }

        [ConfigurationProperty("channelEndpointName", IsRequired = true, IsKey = true)]
        public string ChannelEndpointName
        {
            get { return (string)this["channelEndpointName"]; }
            set { this["channelEndpointName"] = value; }
        }

        //[ConfigurationProperty("endpointInterfaceName", IsRequired = true)]
        //public string EndpointInterfaceName
        //{
        //    get { return (string)this["endpointInterfaceName"]; }
        //    set { this["endpointInterfaceName"] = value; }
        //}

        //public System.Type EndpointInterfaceType
        //{
        //    get
        //    {
        //        if (String.IsNullOrEmpty(EndpointInterfaceName))
        //            throw new MessagingConfigurationException("The Wcf Endpoint Configuration is not valid.  The Interface Type name was not defined.");

        //        System.Type interfaceType = null;
        //        try
        //        {
        //            interfaceType = System.Type.GetType(EndpointInterfaceName);
        //        }
        //        catch (Exception ex)
        //        {
        //            throw new MessagingConfigurationException("An error occurred while attempting to retrieve the Service Type definition.", ex);
        //        }

        //        if (interfaceType == null)
        //        {
        //            throw new MessagingConfigurationException("The Wcf Endpoint Configuration is not valid.  The configured Interface Type was not found.");
        //        }

        //        ServiceContractAttribute[] serviceContracts = (ServiceContractAttribute[])interfaceType.GetCustomAttributes(typeof(ServiceContractAttribute), false);
        //        if ((serviceContracts.Length < 1) ||
        //            (serviceContracts.Length > 1))
        //        {
        //            throw new MessagingConfigurationException("The Wcf Endpoint Configuration is not valid.  The configured interface does not represent a valid Service Contract.");
        //        }
                
        //        return interfaceType;
        //    }
        //}

        //public string EndpointConfigurationName
        //{
        //    get
        //    {
        //        System.Type interfaceType = EndpointInterfaceType;
        //        ServiceContractAttribute[] serviceContracts = (ServiceContractAttribute[])interfaceType.GetCustomAttributes(typeof(ServiceContractAttribute), false);
        //        ServiceContractAttribute serviceContract = serviceContracts[0];

        //        if (!String.IsNullOrEmpty(serviceContract.ConfigurationName))
        //        {
        //            return serviceContract.ConfigurationName;
        //        }
        //        else
        //        {
        //            return interfaceType.FullName;
        //        }
        //    }
        //}
    }
}
