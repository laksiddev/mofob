using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Messaging.Test
{
    /// <summary>
    /// Summary description for WcfClientMessagingTests
    /// </summary>
    [TestClass]
    public class WcfClientMessagingTests
    {
        private static System.ServiceModel.ServiceHost _serviceHost;
        private IAsyncResult _asyncResult;
        private System.Threading.AutoResetEvent _waitHandle;

        public WcfClientMessagingTests()
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
        public void TestSubmitMesssage()
        {
            TestDataRequestMessage testMessage = new TestDataRequestMessage("MessageName");
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage).MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void TestSubmitMesssageAsync()
        {
            TestDataRequestMessage testMessage = new TestDataRequestMessage("MessageName");
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                _asyncResult = null;
                IAsyncResult ar = adapter.BeginSubmitMessage(testMessage, null, new AsyncCallback(MessageDeliveredCallback));

                _waitHandle = new System.Threading.AutoResetEvent(false);
                _waitHandle.WaitOne();

                FrameworkMessage responseMessage = null;
                if (_asyncResult != null)
                {
                    SimpleMessage simpleMessage = adapter.EndSubmitMessage(_asyncResult);
                    Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                    responseMessage = (FrameworkMessage)simpleMessage;
                }

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage).MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void TestSubmitMesssageClientProxy()
        {
            Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataRequestMessage testMessage = new Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataRequestMessage();
            testMessage.name = "MessageName";
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ITestDataService_ClientProxy", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.AreEqual(typeof(Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage), responseMessage.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.name, ((Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage)responseMessage).value, "An incorrect item was returned.");
                Assert.AreNotEqual(((Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage)responseMessage).MessageId, ((Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((Open.MOF.Messaging.Test.WcfServiceProxyReference.TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void TestMultipleSubmitMesssage()
        {
            TestDataRequestMessage testMessage = new TestDataRequestMessage("MessageName");
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage1 = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage1, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage1.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage1).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage1).MessageId, ((TestDataResponseMessage)responseMessage1).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage1).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage1).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);

                simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage2 = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage2, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage2.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage2).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage2).MessageId, ((TestDataResponseMessage)responseMessage2).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage2).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage2).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage1).MessageId, ((TestDataResponseMessage)responseMessage2).MessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);

                simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage3 = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage3, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage3.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage3).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage3).MessageId, ((TestDataResponseMessage)responseMessage3).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage3).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage3).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage1).MessageId, ((TestDataResponseMessage)responseMessage3).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage2).MessageId, ((TestDataResponseMessage)responseMessage3).MessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);

                simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage4 = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage4, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage4.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage4).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage4).MessageId, ((TestDataResponseMessage)responseMessage4).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage4).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage4).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage1).MessageId, ((TestDataResponseMessage)responseMessage4).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage2).MessageId, ((TestDataResponseMessage)responseMessage4).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage3).MessageId, ((TestDataResponseMessage)responseMessage4).MessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);

                simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage5 = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage5, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage5.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage5).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage5).MessageId, ((TestDataResponseMessage)responseMessage5).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage5).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage5).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage1).MessageId, ((TestDataResponseMessage)responseMessage5).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage2).MessageId, ((TestDataResponseMessage)responseMessage5).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage3).MessageId, ((TestDataResponseMessage)responseMessage5).MessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage4).MessageId, ((TestDataResponseMessage)responseMessage5).MessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void TestToHttpAddressSubmitMesssage()
        {
            TestDataRequestMessage testMessage = new TestDataRequestMessage("MessageName");
            testMessage.To = new MessagingEndpoint("http://localhost:8931/WcfServiceProject/TestDataService/", "http://mof.open/MessagingTests/ServiceContracts/1/0/ITestDataService/ProcessTestDataRequest");
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage).MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void TestToTcpAddressSubmitMesssage()
        {
            TestDataRequestMessage testMessage = new TestDataRequestMessage("MessageName");
            testMessage.To = new MessagingEndpoint("net.tcp://localhost:7631/WcfServiceProject/TestDataService/", "http://mof.open/MessagingTests/ServiceContracts/1/0/ITestDataService/ProcessTestDataRequest");
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(testMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("NetTcpBinding_ITestDataService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage simpleMessage = adapter.SubmitMessage(testMessage);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                FrameworkMessage responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.AreEqual(typeof(TestDataResponseMessage), responseMessage.GetType(), "An incorrect type was returned.");
                Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage).Value, "An incorrect item was returned.");
                Assert.AreNotEqual(((TestDataResponseMessage)responseMessage).MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
        }

        [TestMethod]
        public void AdapterCanSupportMessageTest()
        {
            // Try a topic (not addressed) message first
            TestDataRequestMessage dataMessage = new TestDataRequestMessage();
            TestTransactionSubmitMessage onewayMessage = new TestTransactionSubmitMessage();
            TestTransactionRequestMessage twowayMessage = new TestTransactionRequestMessage();
            TestPubSubRequestMessage pubsubMessage = new TestPubSubRequestMessage();
            FaultMessage faultMessage = new FaultMessage();

            IMessagingAdapter adapter = MessagingAdapter.CreateInstance("WcfClientMessagingDataAdapterDefinitionWSHttpBinding");

            Assert.IsNotNull(adapter);
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");

            System.Reflection.MethodInfo method = adapter.GetType().GetMethod("CanSupportMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, new Type[] { typeof(FrameworkMessage) }, null);

            Assert.IsNotNull(method, "Method could not be found.");

            bool canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { dataMessage });
            Assert.IsTrue(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { onewayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { twowayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { pubsubMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");    // The adapter supports TransactionSupported, but the EndPoint does not support the message type
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { faultMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");

            adapter.Dispose();
            adapter = null;
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        [ClassInitialize()]
        public static void RunBeforeAllTests(TestContext testContext)
        {
            RunServiceHost();
        }

        // Use ClassCleanup to run code after all tests in a class have run
        [ClassCleanup()]
        public static void RunAfterAllTests()
        {
            StopServiceHost();
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

        protected void MessageDeliveredCallback(IAsyncResult ar)
        {
            _asyncResult = ar;
            _waitHandle.Set();
        }

        private static void RunServiceHost()
        {
            _serviceHost = new System.ServiceModel.ServiceHost(typeof(Open.MOF.Messaging.Test.WcfService.TestDataService));
            _serviceHost.Open();
        }
        private static void StopServiceHost()
        {

            if (_serviceHost != null)
                _serviceHost.Close();
        }
    }
}
