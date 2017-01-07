using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel.Configuration;

namespace Open.MOF.Messaging
{
    internal interface IContractConfig
    {
        ChannelEndpointElement FindEndpointByName(string channelEndpointName);

        Type GetChannelInterfaceType(ChannelEndpointElement channelEndpoint);

        Type FrameworkMessageTypeLookup(string messageXmlType);
    }
}
