using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingEventArgs : EventArgs
    {
        private FrameworkMessage _message;

        public MessagingEventArgs(FrameworkMessage message) : base()
        {
            _message = message;
        }

        public FrameworkMessage Message
        {
            get { return _message; }
        }
    }
}
