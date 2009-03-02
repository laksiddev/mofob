using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageSubmittedResponse", WrapperNamespace = "http://mof.open/Messaging/ServiceContracts/1/0/")]
    [MessageTransactionBehavior(true, false)]
    public class MessageSubmittedResponse : FrameworkMessage
    {
        public MessageSubmittedResponse() : base()
        {
        }
    }
}
