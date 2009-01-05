using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Test;

namespace Open.MOF.Messaging.Test.WcfService
{
    public class TestDataService : ITestDataService
    {
        public TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage message)
        {
            TestDataResponseMessage responseMessage = new TestDataResponseMessage(message.Name);
            responseMessage.RelatedMessageId = message.MessageId;

            return responseMessage;
        }
    }
}
