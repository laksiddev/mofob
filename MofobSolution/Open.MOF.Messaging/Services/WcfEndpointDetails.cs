using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    internal class WcfEndpointDetails
    {
        public WcfEndpointDetails()
        {
        }

        public WcfEndpointDetails(string channelEndpointName, Type endpointInterfaceType, Type factoryConstructedType, System.Reflection.MethodInfo interfaceMethod, string bindingType)
        {
            ChannelEndpointName = channelEndpointName;
            EndpointInterfaceType = endpointInterfaceType;
            FactoryConstructedType = factoryConstructedType;
            InterfaceMethod = interfaceMethod;
            BindingType = bindingType;
        }

        public string ChannelEndpointName { get; set; }
        public System.Type EndpointInterfaceType { get; set; }
        public System.Type FactoryConstructedType { get; set; }
        public System.Reflection.MethodInfo InterfaceMethod { get; set; }
        public string BindingType { get; set; }
    }
}
