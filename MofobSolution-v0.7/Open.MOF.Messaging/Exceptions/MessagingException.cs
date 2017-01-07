using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingException : ApplicationException
    {
        public MessagingException() : base()
        {
        }

        public MessagingException(string message) : base(message)
        {
        }

        public MessagingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
