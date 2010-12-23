using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.BizTalk.Test.TestStubs.Queued;
using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test
{
    /// <summary>
    /// Summary description for EsbExceptionServiceTests
    /// </summary>
    [TestClass]
    public class EsbQueuedServiceTests
    {
        private static System.ServiceModel.ServiceHost _serviceHost;

        //private IAsyncResult _asyncResult;
        private static System.Threading.AutoResetEvent _waitHandle;

        private static Type _serviceType;
        private static Type _messageType;
        private static string _methodName;
        
        public EsbQueuedServiceTests()
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
        public void SubmitQueuedMessageTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbQueuedMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));

                SimpleMessage simpleMessage = adapter.SubmitMessage(message);
                Assert.IsInstanceOfType(simpleMessage, typeof(FrameworkMessage));
                responseMessage = (FrameworkMessage)simpleMessage;

                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }

            // wait for callback
            _waitHandle = new System.Threading.AutoResetEvent(false);
            _waitHandle.WaitOne();

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Queued.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("ProcessRequestQueued.SubmitRequest", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitQueuedMessageAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message = new Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage("Message");

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbQueuedMessagingAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbMessagingAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(message, null, new AsyncCallback(MessageDeliveredCallback));

                // wait for callback
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
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Queued.ItineraryOneWayService.SubmitRequestRequest), _messageType);
            Assert.AreEqual("ProcessRequestQueued.SubmitRequest", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(message.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitFaultQueuedTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Exception ex = new Exception("This is a test exception");
            Open.MOF.Messaging.FaultMessage fault = new Open.MOF.Messaging.FaultMessage(Guid.NewGuid(), "EsbExceptionSerivce", "Test stubs", ex);

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbQueuedExceptionAdapterDefinition"))
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

            // wait for callback
            _waitHandle = new System.Threading.AutoResetEvent(false);
            _waitHandle.WaitOne();

            Assert.IsNotNull(_serviceType);
            Assert.IsNotNull(_messageType);
            Assert.IsNotNull(_methodName);

            Assert.AreEqual(typeof(EsbServiceImpl), _serviceType);
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.SubmitFaultRequest), _messageType);
            Assert.AreEqual("ExceptionHandlingQueued.SubmitFault", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(fault.MessageId, responseMessage.RelatedMessageId);
        }

        [TestMethod]
        public void SubmitFaultQueuedAsyncTest()
        {
            _serviceType = null;
            _messageType = null;
            _methodName = null;

            Exception ex = new Exception("This is a test exception");
            Open.MOF.Messaging.FaultMessage fault = new Open.MOF.Messaging.FaultMessage(Guid.NewGuid(), "EsbExceptionSerivce", "Test stubs", ex);

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance("EsbQueuedExceptionAdapterDefinition"))
            {
                Assert.IsNotNull(adapter);
                Assert.IsInstanceOfType(adapter, typeof(Open.MOF.BizTalk.Adapters.EsbExceptionAdapter));
                IAsyncResult asyncResult = adapter.BeginSubmitMessage(fault, null, new AsyncCallback(MessageDeliveredCallback));

                // wait for callback
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
            Assert.AreEqual(typeof(Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.SubmitFaultRequest), _messageType);
            Assert.AreEqual("ExceptionHandlingQueued.SubmitFault", _methodName);

            Assert.IsNotNull(responseMessage);
            Assert.IsInstanceOfType(responseMessage, typeof(MessageSubmittedResponse));
            Assert.AreEqual(fault.MessageId, responseMessage.RelatedMessageId);
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

        protected static void RequestMessageReceivedHandler(object sender, Open.MOF.BizTalk.Test.TestStubs.RequestMessageReceivedEventArgs args)
        {
            _serviceType = sender.GetType();
            _messageType = args.Message.GetType();
            _methodName = args.MethodName;

            _waitHandle.Set();
        }

        protected void MessageDeliveredCallback(IAsyncResult ar)
        {
            //_asyncResult = ar;
            //_waitHandle.Set();
        }

        private static void RunServiceHost()
        {
            EsbServiceImpl service = new EsbServiceImpl();
            service.RequestMessageReceived += new EventHandler<Open.MOF.BizTalk.Test.TestStubs.RequestMessageReceivedEventArgs>(EsbQueuedServiceTests.RequestMessageReceivedHandler);
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
