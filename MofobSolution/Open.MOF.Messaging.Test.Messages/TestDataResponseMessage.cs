using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Test.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "TestDataResponseMessage", WrapperNamespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    public class TestDataResponseMessage : FrameworkMessage
    {
        public TestDataResponseMessage() : base()
        {
            _value = null;
        }

        public TestDataResponseMessage(string value) : base()
        {
            _value = value;
        }

        [MessageBodyMember(Name = "value", Order = 1, Namespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
        protected string _value;
        public string Value
        {
            get { return _value; }
            set { _value = value; }
        }
    }
}
