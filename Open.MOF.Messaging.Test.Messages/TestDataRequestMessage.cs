using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Test.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "TestDataRequestMessage", WrapperNamespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = false, RequiresTransactions = false)]
    [TwoWayResponseMessageType(typeof(TestDataResponseMessage))]
    public class TestDataRequestMessage : FrameworkMessage
    {
        public TestDataRequestMessage() : base()
        {
            _name = null;
        }

        public TestDataRequestMessage(string name) : base()
        {
            _name = name;
        }

        [MessageBodyMember(Name = "name", Order = 1, Namespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
        protected string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
