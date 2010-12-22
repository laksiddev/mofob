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

        public MessagingState(FrameworkMessage requestMessage) 
            : base()
        {
            RequestMessage = requestMessage;
        }

        public MessagingState(FrameworkMessage requestMessage, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
            : base()
        {
            RequestMessage = requestMessage;
            MessageResponseCallback = messageResponseCallback;
        }

        public MessagingState(FrameworkMessage requestMessage, MessageHandlingSummary handlingSummary, FrameworkMessage responseMessage)
        {
            RequestMessage = requestMessage;
            HandlingSummary = handlingSummary;
            ResponseMessage = responseMessage;
        }

        public FrameworkMessage RequestMessage { get; set; }
        public MessageHandlingSummary HandlingSummary { get; set; }
        public FrameworkMessage ResponseMessage { get; set; }
        public EventHandler<MessageReceivedEventArgs> MessageResponseCallback { get; set; }
    }
}
