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

        public MessagingResult(MessageBase requestMessage) : base()
        {
            RequestMessage = requestMessage;
        }

        public MessagingResult(MessageBase requestMessage, bool wasMessageDelivered, MessageBase responseMessage)
        {
            RequestMessage = requestMessage;
            WasMessageDelivered = (bool?)wasMessageDelivered;
            ResponseMessage = responseMessage;
        }

        public MessageBase RequestMessage { get; set; }
        public bool? WasMessageDelivered { get; set; }
        public MessageBase ResponseMessage { get; set; }
    }
}
