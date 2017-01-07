using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingState
    {
        public MessagingState()
        {
            RequestMessage = null;
            HandlingSummary = null;
            ResponseMessage = null;
            MessageResponseCallback = null;
        }

        public MessagingState(SimpleMessage requestMessage) 
            : base()
        {
            RequestMessage = requestMessage;
        }

        public MessagingState(SimpleMessage requestMessage, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
            : base()
        {
            RequestMessage = requestMessage;
            MessageResponseCallback = messageResponseCallback;
        }

        public MessagingState(SimpleMessage requestMessage, MessageHandlingSummary handlingSummary, SimpleMessage responseMessage)
        {
            RequestMessage = requestMessage;
            HandlingSummary = handlingSummary;
            ResponseMessage = responseMessage;
        }

        public SimpleMessage RequestMessage { get; set; }
        public MessageHandlingSummary HandlingSummary { get; set; }
        public SimpleMessage ResponseMessage { get; set; }
        public EventHandler<MessageReceivedEventArgs> MessageResponseCallback { get; set; }
    }
}
