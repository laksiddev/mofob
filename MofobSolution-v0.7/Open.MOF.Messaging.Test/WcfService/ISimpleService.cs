using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging.Test.WcfService
{
    [ServiceContract(Name = "ISimpleService", ConfigurationName = "Open.MOF.Messaging.Test.WcfService.ISimpleService", Namespace = "http://mof.open/MessagingTests/ServiceContracts/1/0/")]
    public interface ISimpleService
    {
        [OperationContract(Name = "PerformSimpleMethod", Action = "http://mof.open/MessagingTests/ServiceContracts/1/0/ISimpleService/PerformSimpleMethod")]
        System.ServiceModel.Channels.Message PerformSimpleMethod(System.ServiceModel.Channels.Message message);
    }

}
