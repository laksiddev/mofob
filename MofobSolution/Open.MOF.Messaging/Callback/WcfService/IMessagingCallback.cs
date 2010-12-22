using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Callback.WcfService
{
    [ServiceContract(Name = "IMessagingCallback", ConfigurationName = "MOF.Messaging.IMessagingCallback", Namespace = "http://mof.open/Messaging/ServiceContracts/1/0/")]
    public interface IMessagingCallback
    {
        [OperationContract(Name = "ProcessResponse", Action = "http://mof.open/Messaging/ServiceContracts/1/0/IMessagingCallback/ProcessMessage")]
        //[OperationContract(Name = "ProcessResponse", Action = "*")]
        void ProcessResponse(object message);
    }
}
