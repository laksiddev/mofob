using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.BizTalk.Test.TestStubs.Bundled;
using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test
{
    /// <summary>
    /// Summary description for EsbExceptionServiceTests
    /// </summary>
    [TestClass]
    public class EsbBundledServiceTests
    {
        private static System.ServiceModel.ServiceHost _serviceHost;

        //private IAsyncResult _asyncResult;
        private System.Threading.AutoResetEvent _waitHandle;

        private static Type _serviceType;
        private static Type _messageType;
        private static string _methodName;

        public EsbBundledServiceTests()
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
        public void SubmitOneWayMessageTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbOneWayBundledMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                Assert.IsFalse(((Open.MOF.Messaging.Adapters.MessagingAdapter)adapter).CanSupportMessage(message));
                message.To = new MessagingEndpoint("http://someendpoint/", "someAction");
                Assert.IsTrue(((Open.MOF.Messaging.Adapters.MessagingAdapter)adapter).CanSupportMessage(message));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("Bundled.ProcessRequest.SubmitRequest", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
        }


        [TestMethod]
        public void SubmitOneWayMessageAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbOneWayBundledMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                message.To = new MessagingEndpoint("http://someendpoint/", "someAction");

                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, null, new AsyncCallback(MessageDeliveredCallback));

                _waitHandle = new System.Threading.AutoResetEvent(false);
                _waitHandle.WaitOne();

                SimpleMessage simpleMessage = adapter.EndSubmitMessage(asyncResult);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("Bundled.ProcessRequest.SubmitRequest", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitTwoWayMessageTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayBundledMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                Assert.IsFalse(((Open.MOF.Messaging.Adapters.MessagingAdapter)adapter).CanSupportMessage(message));
                message.To = new MessagingEndpoint("http://someendpoint/", "someAction");
                Assert.IsTrue(((Open.MOF.Messaging.Adapters.MessagingAdapter)adapter).CanSupportMessage(message));
                
                SimpleMessage simpleMessage = adapter.SubmitMessage(message);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("Bundled.ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            //Assert.AreEqual("this.is.the.itinerary.name", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);
        }

        [TestMethod]
        public void SubmitTwoWayMessageAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayBundledMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                message.To = new MessagingEndpoint("http://someendpoint/", "someAction");

                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, null, new AsyncCallback(MessageDeliveredCallback));

                _waitHandle = new System.Threading.AutoResetEvent(false);
                _waitHandle.WaitOne();

                SimpleMessage simpleMessage = adapter.EndSubmitMessage(asyncResult);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("Bundled.ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            //Assert.AreEqual("this.is.the.itinerary.name", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);
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

        //protected void MessageDeliveredCallback(IAsyncResult ar)
        //{
        //    _asyncResult = ar;
        //    _waitHandle.Set();
        //}

        protected static void RequestMessageReceivedHandler(object sender, Open.MOF.BizTalk.Test.TestStubs.RequestMessageReceivedEventArgs args)
        {
            _serviceType = sender.GetType();
            _messageType = args.Message.GetType();
            _methodName = args.MethodName;

            //_waitHandle.Set();
        }

        protected void MessageDeliveredCallback(IAsyncResult ar)
        {
            //_asyncResult = ar;
            _waitHandle.Set();
        }

        private static void RunServiceHost()
        {
            EsbServiceImpl service = new EsbServiceImpl();
            service.RequestMessageReceived += new EventHandler<Open.MOF.BizTalk.Test.TestStubs.RequestMessageReceivedEventArgs>(EsbBundledServiceTests.RequestMessageReceivedHandler);
            _serviceHost = new System.ServiceModel.ServiceHost(service);
            _serviceHost.Open();
        }

        private static void StopServiceHost()
        {

            if (_serviceHost != null)
                _serviceHost.Close();
        }
    }
}
