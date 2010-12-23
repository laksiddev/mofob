using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class UnknownMessageEventArgs : MessagingEventArgs
    {
        public UnknownMessageEventArgs(SimpleMessage message)
            : base(message)
        {
        }
   }
}
