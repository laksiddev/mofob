using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.BizTalk.Test.TestStubs;
using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test
{
    /// <summary>
    /// Summary description for EsbExceptionServiceTests
    /// </summary>
    [TestClass]
    public class EsbServiceTests
    {
        private static System.ServiceModel.ServiceHost _serviceHost;

        //private IAsyncResult _asyncResult;
        private System.Threading.AutoResetEvent _asyncWaitHandle;
        private System.Threading.AutoResetEvent _receivedWaitHandle;

        private static Type _serviceType;
        private static Type _messageType;
        private static string _methodName;

        private SimpleMessage _messageReceivedCallbackMessage;
        
        public EsbServiceTests()
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
        public void SubmitFaultTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Exception ex = new Exception("This is a test exception");
            Open.MOF.Messaging.FaultMessage fault = new Open.MOF.Messaging.FaultMessage(Guid.NewGuid(), "EsbExceptionSerivce", "Test stubs", ex);

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbExceptionAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbExceptionAdapter));
                SimpleMessage simpleMessage = adapter.SubmitMessage(fault);
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
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFaultRequest), _messageType);
            Assert.AreEqual("ExceptionHandling.SubmitFault", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(fault.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitFaultAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Exception ex = new Exception("This is a test exception");
            Open.MOF.Messaging.FaultMessage fault = new Open.MOF.Messaging.FaultMessage(Guid.NewGuid(), "EsbExceptionSerivce", "Test stubs", ex);

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbExceptionAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbExceptionAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(fault, null, new AsyncCallback(MessageDeliveredCallback));

                _asyncWaitHandle = new System.Threading.AutoResetEvent(false);
                _asyncWaitHandle.WaitOne();

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
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFaultRequest), _messageType);
            Assert.AreEqual("ExceptionHandling.SubmitFault", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(fault.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitOneWayMessageTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbOneWayMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.OneWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("ProcessRequest.SubmitRequest", _methodName);

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
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbOneWayMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, null, new AsyncCallback(MessageDeliveredCallback));

                _asyncWaitHandle = new System.Threading.AutoResetEvent(false);
                _asyncWaitHandle.WaitOne();

                SimpleMessage simpleMessage = adapter.EndSubmitMessage(asyncResult);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.OneWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("ProcessRequest.SubmitRequest", _methodName);

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
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.TwoWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);
        }

        [TestMethod]
        public void SubmitTwoWayMessageCallbackTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;
            _messageReceivedCallbackMessage = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayMessagingAdapterDefinition"))
            {
                _receivedWaitHandle = new System.Threading.AutoResetEvent(false);

                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message, new EventHandler<MessageReceivedEventArgs>(CallbackMessageReceivedHandler));
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                _receivedWaitHandle.WaitOne();

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.TwoWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);

            // The _messageReceivedCallbackMessage should have been set in the callback method
            Assert.IsNotNull(_messageReceivedCallbackMessage);
            Assert.IsInstanceOfType(_messageReceivedCallbackMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.IsInstanceOfType(_messageReceivedCallbackMessage, typeof(FrameworkMessage));
            Assert.AreEqual(message.MessageId, ((FrameworkMessage)_messageReceivedCallbackMessage).RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)_messageReceivedCallbackMessage).Context);
        }

        [TestMethod]
        public void SubmitTwoWayMessageAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, null, new AsyncCallback(MessageDeliveredCallback));

                _asyncWaitHandle = new System.Threading.AutoResetEvent(false);
                _asyncWaitHandle.WaitOne();

                SimpleMessage simpleMessage = adapter.EndSubmitMessage(asyncResult);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.TwoWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);
        }

        [TestMethod]
        public void SubmitTwoWayMessageAsyncCallbackTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;
            _messageReceivedCallbackMessage = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbTwoWayMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, new EventHandler<MessageReceivedEventArgs>(CallbackMessageReceivedHandler), new AsyncCallback(MessageDeliveredCallback));

                _receivedWaitHandle = new System.Threading.AutoResetEvent(false);
                _asyncWaitHandle = new System.Threading.AutoResetEvent(false);
                _asyncWaitHandle.WaitOne();
                _receivedWaitHandle.WaitOne();

                SimpleMessage simpleMessage = adapter.EndSubmitMessage(asyncResult);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.TwoWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseRequest), _messageType);
            Assert.AreEqual("ProcessRequestResponse.SubmitRequestResponse", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)responseMessage).Context);

            // The _messageReceivedCallbackMessage should have been set in the callback method
            Assert.IsNotNull(_messageReceivedCallbackMessage);
            Assert.IsInstanceOfType(_messageReceivedCallbackMessage, typeof(Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage));
            Assert.IsInstanceOfType(_messageReceivedCallbackMessage, typeof(FrameworkMessage));
            Assert.AreEqual(message.MessageId, ((FrameworkMessage)_messageReceivedCallbackMessage).RelatedMessageId);
            Assert.AreEqual("TestTransactionRequestMessageItinerary", ((Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage)_messageReceivedCallbackMessage).Context);
        }

        [TestMethod]
        public void SubmitPubSubMessageCallbackTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage message = new Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbOneWayMessagingAdapterDefinition"))
            {
                _receivedWaitHandle = new System.Threading.AutoResetEvent(false);

                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message, new EventHandler<MessageReceivedEventArgs>(CallbackMessageReceivedHandler));
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                _receivedWaitHandle.WaitOne();

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.IsTrue(adapter.MessageHandlingSummary.AdapterContext.IndexOf("Open.MOF.BizTalk.Adapters.MessageHandlers.OneWayItineraryEsbMessageHandler", StringComparison.CurrentCulture) >= 0);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("ProcessRequest.SubmitRequest", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
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
        protected void CallbackMessageReceivedHandler(object sender, MessageReceivedEventArgs args)
        {
            _messageReceivedCallbackMessage = args.Message;
            _receivedWaitHandle.Set();
        }

        protected static void RequestMessageReceivedHandler(object sender, RequestMessageReceivedEventArgs args)
        {
            _serviceType = sender.GetType();
            _messageType = args.Message.GetType();
            _methodName = args.MethodName;

            if (_messageType == typeof(Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest))
            {
                string messagePart = ((Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest)args.Message).part.ToString();
                FrameworkMessage message = FrameworkMessage.FromXmlString(messagePart);
            }
            //_waitHandle.Set();
        }

        protected void MessageDeliveredCallback(IAsyncResult ar)
        {
            //_asyncResult = ar;
            _asyncWaitHandle.Set();
        }

        private static void RunServiceHost()
        {
            EsbServiceImpl service = new EsbServiceImpl();
            service.RequestMessageReceived += new EventHandler<RequestMessageReceivedEventArgs>(EsbServiceTests.RequestMessageReceivedHandler);
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
