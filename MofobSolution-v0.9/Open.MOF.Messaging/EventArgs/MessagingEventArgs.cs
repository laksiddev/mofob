using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingEventArgs : EventArgs
    {
        private SimpleMessage _message;

        public MessagingEventArgs(SimpleMessage message) : base()
        {
            _message = message;
        }

        public SimpleMessage Message
        {
            get { return _message; }
        }
    }
}
