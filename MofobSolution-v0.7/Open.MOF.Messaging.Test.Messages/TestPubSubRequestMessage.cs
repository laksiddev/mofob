using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Test.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "TestPubSubRequestMessage", WrapperNamespace = "http://mof.open/MessagingTests/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    [PubSubResponseMessageType(typeof(TestPubSubResponseMessage))]
    public class TestPubSubRequestMessage : FrameworkMessage
    {
        public TestPubSubRequestMessage() : base()
        {
            _name = null;
        }

        public TestPubSubRequestMessage(string name) : base()
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