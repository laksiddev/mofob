using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "FaultResponse", WrapperNamespace = "http://mofob.open/Messaging/ServiceContracts/1/0/")]
    public class FaultResponse : ResponseMessage<FaultMessage>
    {
        public FaultResponse() : base()
        {
        }
    }
}
