using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageDescriptorAttribute : Attribute
    {
        public MessageDescriptorAttribute(string messageDescriptor)
        {
            if (messageDescriptor == null)
                throw new ArgumentException("MessageDescriptor is a required paramter.", "MessageDescriptor");

            _messageDescriptor = messageDescriptor;
        }

        protected string _messageDescriptor;
        public string MessageDescriptor
        {
            get { return _messageDescriptor; }
            set { _messageDescriptor = value; }
        }
    }
}
