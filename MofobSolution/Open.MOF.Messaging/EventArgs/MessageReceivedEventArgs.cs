using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessageReceivedEventArgs : MessagingEventArgs
    {
        public MessageReceivedEventArgs(MessageBase message) : base(message)
        {
        }
    }
}
