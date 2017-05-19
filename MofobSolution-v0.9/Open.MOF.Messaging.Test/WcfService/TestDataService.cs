using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Messaging.Test.WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class TestDataService : ITestDataService
    {
        private Action<string> _MessageSubmittedHandler = null;

        public TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage message)
        {
            OnMessageSubmitted(message.ToXmlString());

            TestDataResponseMessage responseMessage = new TestDataResponseMessage(message.Name);
            responseMessage.RelatedMessageId = message.MessageId;

            return responseMessage;
        }

        public void ProcessTestDataRequest(TestDataResponseMessage message)
        {
        }

        public void RegisterMessageHandler(Action<string> MessageSubmittedHandler)
        {
            if (MessageSubmittedHandler != null)
                _MessageSubmittedHandler = MessageSubmittedHandler;
        }

        private void OnMessageSubmitted(string request)
        {
            if (_MessageSubmittedHandler != null)
                _MessageSubmittedHandler(request);
        }
    }
}
