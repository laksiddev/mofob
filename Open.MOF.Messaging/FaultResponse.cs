using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "FaultResponse", WrapperNamespace = "http://mof.open/Messaging/MessageContracts/1/0/")]
    public class FaultResponse : FrameworkMessage
    {
        public FaultResponse() : base()
        {
        }

        public static MessageBehavior Behavior
        {
            get
            {
                return MessageBehavior.TransactionsSupported;
            }
        }
    }
}
