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

        public WcfEndpointDetails(string bindingName, Type endpointInterfaceType, Type factoryConstructedType, System.Reflection.MethodInfo interfaceMethod)
        {
            BindingName = bindingName;
            EndpointInterfaceType = endpointInterfaceType;
            FactoryConstructedType = factoryConstructedType;
            InterfaceMethod = interfaceMethod;
        }

        public string BindingName { get; set; }
        public System.Type EndpointInterfaceType { get; set; }
        public System.Type FactoryConstructedType { get; set; }
        public System.Reflection.MethodInfo InterfaceMethod { get; set; }
    }
}
