using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Unity;
using Microsoft.Practices.ServiceLocation;

namespace Open.MOF.Messaging.Adapters
{
    public interface IMessagingResolver
    {
        void RegisterInstanceName(AdapterInterfaceType interfaceType, string instanceName, int preferenceNumber, Type instanceType);

        List<string> ResolveInstanceNames(AdapterInterfaceType interfaceType);
    }
}
