using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageSubmittedResponse", WrapperNamespace = "http://mof.open/Messaging/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    public class MessageSubmittedResponse : FrameworkMessage
    {
        public MessageSubmittedResponse() : base()
        {
        }
    }
}
