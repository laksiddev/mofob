using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace Open.MOF.Messaging
{

    public static class WcfUtility
    {
        private static IContractConfig _contractConfig;   

        public static ChannelEndpointElement FindEndpointByName(string channelEndpointName)
        {
            return ContractConfig.FindEndpointByName(channelEndpointName);
        }

        public static Type GetChannelInterfaceType(ChannelEndpointElement channelEndpoint)
        {
            return ContractConfig.GetChannelInterfaceType(channelEndpoint);
        }

        public static bool DoesAddressMatchBinding(string addressUri, string bindingType)
        {
            if (((addressUri.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase)) ||
                (addressUri.StartsWith("https://", StringComparison.CurrentCultureIgnoreCase))) &&
                ((bindingType == "wsHttpBinding") || (bindingType == "basicHttpBinding")))
            {
                return true;
            }
            else if ((addressUri.StartsWith("net.tcp://", StringComparison.CurrentCultureIgnoreCase)) &&
                (bindingType == "netTcpBinding"))
            {
                return true;
            }

            return false;
        }

        public static Type FrameworkMessageTypeLookup(string messageXmlType)
        {
            return ContractConfig.FrameworkMessageTypeLookup(messageXmlType);
        }

        private static IContractConfig ContractConfig
        {
            get
            {
                if (_contractConfig == null)
                {
                    _contractConfig = ContractConfigBase.GetContractConfigInstance();
                }

                return _contractConfig;
            }
        }
    }
}
