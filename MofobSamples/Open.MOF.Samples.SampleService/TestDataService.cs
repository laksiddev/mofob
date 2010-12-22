using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.SampleService
{
    // NOTE: If you change the class name "Service1" here, you must also update the reference to "Service1" in App.config.
    public class TestDataService : ITestDataService
    {
        public TestDataResponseMessage ProcessTestDataRequest(TestDataRequestMessage requestMessage)
        {
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.SampleService.TestDataService.ProcessTestDataRequest() method called\nmessage received : {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), requestMessage.Name));
            TestDataResponseMessage responseMessage = new TestDataResponseMessage(requestMessage.ToXmlString());
            responseMessage.RelatedMessageId = requestMessage.MessageId;

            return responseMessage;
        }

        public TestTransactionResponseMessage ProcessTestTransactionRequest(TestTransactionRequestMessage requestMessage)
        {
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.SampleService.TestDataService.ProcessTestTransactionRequest() method called\nmessage received : {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), requestMessage.Name));
            TestTransactionResponseMessage responseMessage = new TestTransactionResponseMessage(requestMessage.ToXmlString(), requestMessage.SenderDescription);
            responseMessage.RelatedMessageId = requestMessage.MessageId;

            return responseMessage;
        }

        public void ProcessTestTransactionSubmit(TestTransactionSubmitMessage message)
        {
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.SampleService.TestDataService.ProcessTestTransactionSubmit() method called\nmessage received : {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), message.Name));
        }

        public void ProcessTestPubSubRequest(TestPubSubRequestMessage requestMessage)
        {
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.SampleService.TestDataService.ProcessTestPubSubRequest() method called\nmessage received : {2}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), requestMessage.Name));
            TestPubSubResponseMessage responseMessage = new TestPubSubResponseMessage(requestMessage.ToXmlString(), requestMessage.SenderDescription);
            responseMessage.RelatedMessageId = requestMessage.MessageId;
            responseMessage.To = requestMessage.ReplyTo;
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(responseMessage))
            {
                adapter.SubmitMessage(responseMessage);
            }
        }
    }
}
