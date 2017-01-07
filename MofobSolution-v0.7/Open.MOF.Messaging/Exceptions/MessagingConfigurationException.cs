using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingConfigurationException : ApplicationException
    {
        public MessagingConfigurationException() : base()
        {
        }

        public MessagingConfigurationException(string message) : base(message)
        {
        }

        public MessagingConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
