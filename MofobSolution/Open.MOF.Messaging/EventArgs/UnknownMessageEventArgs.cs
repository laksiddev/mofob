using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class UnknownMessageEventArgs : MessagingEventArgs
    {
        public UnknownMessageEventArgs(FrameworkMessage message)
            : base(message)
        {
        }
   }
}
