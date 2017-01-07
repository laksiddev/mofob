using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Test.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "TestPubSubResponseMessage", WrapperNamespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    public class TestPubSubResponseMessage : FrameworkMessage
    {
        public TestPubSubResponseMessage() : base()
        {
            _value = null;
            _context = null;
        }

        public TestPubSubResponseMessage(string value, string context)
            : base()
        {
            _value= value;
            _context = context;
        }

        [MessageBodyMember(Name = "value", Order = 1, Namespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
        protected string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }

        [MessageBodyMember(Name = "context", Order = 2, Namespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
        protected string _context;
        public string Context
        {
            get { return _context; }
            set { _context = value; }
        }
    }
}