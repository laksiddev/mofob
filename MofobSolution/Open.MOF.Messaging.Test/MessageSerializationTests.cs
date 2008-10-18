using System;
using System.Text;
using System.ServiceModel;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Open.MOF.Messaging.Test
{
    /// <summary>
    /// Summary description for MessageSerializationTests
    /// </summary>
    [TestClass]
    public class MessageSerializationTests
    {
        public MessageSerializationTests()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }
        
        [TestMethod]
        public void DataRequestMessageSerializationTest()
        {
            TestDataRequestMessage message = new TestDataRequestMessage();
            message.Name = "TestMessage";
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            TestDataRequestMessage testMessage = TestDataRequestMessage.FromXmlString(messageText);

            Assert.IsNotNull(testMessage);
            Assert.AreEqual(message.Name, testMessage.Name, "An unexpected value was returned.");
            Assert.AreEqual(message.MessageId, testMessage.MessageId, "An unexpected value was returned.");
            Assert.IsNotNull(testMessage.From);
            Assert.AreEqual(message.From.Uri, testMessage.From.Uri, "An unexpected value was returned.");
            Assert.AreEqual(message.From.Action, testMessage.From.Action, "An unexpected value was returned.");
        }

        [TestMethod]
        public void TransactionRequestMessageSerializationTest()
        {
            TestTransactionRequestMessage message = new TestTransactionRequestMessage();
            message.Name = "TestMessage";
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            TestTransactionRequestMessage testMessage = TestTransactionRequestMessage.FromXmlString(messageText);

            Assert.IsNotNull(testMessage);
            Assert.AreEqual(message.Name, testMessage.Name, "An unexpected value was returned.");
            Assert.AreEqual(message.MessageId, testMessage.MessageId, "An unexpected value was returned.");
            Assert.IsNotNull(testMessage.From);
            Assert.AreEqual(message.From.Uri, testMessage.From.Uri, "An unexpected value was returned.");
            Assert.AreEqual(message.From.Action, testMessage.From.Action, "An unexpected value was returned.");
        }

        [TestMethod]
        public void FaultMessageSerializationTest()
        {
            FaultMessage message = new FaultMessage();
            message.ExceptionInstanceId = Guid.NewGuid();
            message.ApplicationName = "ApplicationName";
            message.ServiceName = "ServiceName";
            message.SetExceptionDetail(new ApplicationException("Test Exception"));
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            FaultMessage testMessage = FaultMessage.FromXmlString(messageText);

            Assert.IsNotNull(testMessage);
            Assert.IsNotNull(testMessage.ExceptionDetail);
            Assert.AreEqual(message.ExceptionDetail.ExceptionType, testMessage.ExceptionDetail.ExceptionType, "An unexpected value was returned.");
            Assert.AreEqual(message.ExceptionDetail.Message, testMessage.ExceptionDetail.Message, "An unexpected value was returned.");
            Assert.AreEqual(message.ApplicationName, testMessage.ApplicationName, "An unexpected value was returned.");
            Assert.AreEqual(message.ServiceName, testMessage.ServiceName, "An unexpected value was returned.");
            Assert.AreEqual(message.ExceptionInstanceId, testMessage.ExceptionInstanceId, "An unexpected value was returned.");
            Assert.AreEqual(message.MessageId, testMessage.MessageId, "An unexpected value was returned.");
            Assert.IsNotNull(testMessage.From);
            Assert.AreEqual(message.From.Uri, testMessage.From.Uri, "An unexpected value was returned.");
            Assert.AreEqual(message.From.Action, testMessage.From.Action, "An unexpected value was returned.");
        }

        [TestMethod]
        public void SubscribeRequestMessageSerializationTest()
        {
            SubscribeRequestMessage message = new SubscribeRequestMessage();
            message.EndpointUri = "http://endpointuri/";
            message.Action = "messageaction";
            message.SubscriptionMessageXmlType = MessageBase.GetMessageXmlType(typeof(TestDataRequestMessage));
            Assert.IsTrue((!String.IsNullOrEmpty(message.SubscriptionMessageXmlType)), "No message xml information was available.");
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            SubscribeRequestMessage testMessage = SubscribeRequestMessage.FromXmlString(messageText);

            Assert.IsNotNull(testMessage);
            Assert.AreEqual(message.EndpointUri, testMessage.EndpointUri, "An unexpected value was returned.");
            Assert.AreEqual(message.Action, testMessage.Action, "An unexpected value was returned.");
            Assert.AreEqual(message.SubscriptionMessageXmlType, testMessage.SubscriptionMessageXmlType, "An unexpected value was returned.");
            Assert.AreEqual(message.MessageId, testMessage.MessageId, "An unexpected value was returned.");
            Assert.IsNotNull(testMessage.From);
            Assert.AreEqual(message.From.Uri, testMessage.From.Uri, "An unexpected value was returned.");
            Assert.AreEqual(message.From.Action, testMessage.From.Action, "An unexpected value was returned.");
        }

        [TestMethod]
        public void UnsubscribeRequestMessageSerializationTest()
        {
            UnsubscribeRequestMessage message = new UnsubscribeRequestMessage();
            message.EndpointUri = "http://endpointuri/";
            message.Action = "messageaction";
            message.SubscriptionMessageXmlType = MessageBase.GetMessageXmlType(typeof(TestDataRequestMessage));
            Assert.IsTrue((!String.IsNullOrEmpty(message.SubscriptionMessageXmlType)), "No message xml information was available.");
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            UnsubscribeRequestMessage testMessage = UnsubscribeRequestMessage.FromXmlString(messageText);

            Assert.IsNotNull(testMessage);
            Assert.AreEqual(message.EndpointUri, testMessage.EndpointUri, "An unexpected value was returned.");
            Assert.AreEqual(message.Action, testMessage.Action, "An unexpected value was returned.");
            Assert.AreEqual(message.SubscriptionMessageXmlType, testMessage.SubscriptionMessageXmlType, "An unexpected value was returned.");
            Assert.AreEqual(message.MessageId, testMessage.MessageId, "An unexpected value was returned.");
            Assert.IsNotNull(testMessage.From);
            Assert.AreEqual(message.From.Uri, testMessage.From.Uri, "An unexpected value was returned.");
            Assert.AreEqual(message.From.Action, testMessage.From.Action, "An unexpected value was returned.");
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void RunBeforeAllTests(TestContext testContext)
        {
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void RunAfterAllTests()
        {
        }

        // Use TestInitialize to run code before running each test 
        [TestInitialize()]
        public void RunBeforeEachTest()
        {
        }

        // Use TestCleanup to run code after each test has run
        [TestCleanup()]
        public void RunAfterEachTest()
        {
        }

        #endregion
    }
}
