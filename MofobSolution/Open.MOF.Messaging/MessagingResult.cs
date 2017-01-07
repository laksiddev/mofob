using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingResult
    {
        public MessagingResult()
        {
            RequestMessage = null;
            WasMessageDelivered = null;
            ResponseMessage = null;
        }

        public MessagingResult(FrameworkMessage requestMessage) : base()
        {
            RequestMessage = requestMessage;
        }

        public MessagingResult(FrameworkMessage requestMessage, bool wasMessageDelivered, FrameworkMessage responseMessage)
        {
            RequestMessage = requestMessage;
            WasMessageDelivered = (bool?)wasMessageDelivered;
            ResponseMessage = responseMessage;
        }

        public FrameworkMessage RequestMessage { get; set; }
        public bool? WasMessageDelivered { get; set; }
        public FrameworkMessage ResponseMessage { get; set; }
    }
}
