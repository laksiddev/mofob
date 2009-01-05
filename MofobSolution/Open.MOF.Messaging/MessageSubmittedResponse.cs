using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageSubmittedResponse", WrapperNamespace = "http://mofob.open/Messaging/ServiceContracts/1/0/")]
    public class MessageSubmittedResponse : ResponseMessage<MessageSubmittedResponse>
    {
        public MessageSubmittedResponse() : base()
        {
        }
    }
}
