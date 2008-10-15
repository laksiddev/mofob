using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingEventArgs : EventArgs
    {
        private MessageBase _message;

        public MessagingEventArgs(MessageBase message) : base()
        {
            _message = message;
        }

        public MessageBase Message
        {
            get { return _message; }
        }
    }
}
