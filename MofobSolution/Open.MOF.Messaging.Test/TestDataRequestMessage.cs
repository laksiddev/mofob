using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Test
{
    [MessageContract(IsWrapped = true, WrapperName = "TestDataRequestMessage", WrapperNamespace = "http://mofob.open/MessagingTests/ServiceContracts/1/0/")]
    public class TestDataRequestMessage : DataRequestMessage<TestDataRequestMessage>
    {
        public TestDataRequestMessage() : base()
        {
            _name = null;
        }

        [MessageBodyMember(Name = "name", Order = 1, Namespace = "http://mofob.open/MessagingTests/DataContracts/1/0/")]
        protected string _name;
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}
