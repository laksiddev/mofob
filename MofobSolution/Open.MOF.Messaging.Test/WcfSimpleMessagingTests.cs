using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;

namespace Open.MOF.Messaging.Test
{
    [TestClass]
    public class WcfSimpleMessagingTests
    {
        private static System.ServiceModel.ServiceHost _serviceHost;
        private IAsyncResult _asyncResult;
        private System.Threading.AutoResetEvent _waitHandle;

        public WcfSimpleMessagingTests()
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
        public void TestSubmitSimpleMessage()
        {
            TwoWayMessage requestMessage = new TwoWayMessage();
            requestMessage.LoadContent("<PerformSimpleMethodRequest>ThisIsMyRequest</PerformSimpleMethodRequest>");

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(requestMessage))
            {
                Assert.IsNotNull(adapter, "No item was returned.");
                Assert.AreEqual(typeof(WcfClientMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
                Assert.AreEqual("WSHttpBinding_ISimpleService", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
                Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
                Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
                //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

                SimpleMessage responseMessage = adapter.SubmitMessage(requestMessage);

                Assert.IsNotNull(responseMessage, "No item was returned.");
                Assert.IsInstanceOfType(responseMessage, typeof(SimpleMessage), "An incorrect type was returned.");
                //Assert.AreEqual(testMessage.Name, ((TestDataResponseMessage)responseMessage).Value, "An incorrect item was returned.");
                //Assert.AreNotEqual(((TestDataResponseMessage)responseMessage).MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                //Assert.AreNotEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).MessageId, "An incorrect item was returned.");
                //Assert.AreEqual(testMessage.MessageId, ((TestDataResponseMessage)responseMessage).RelatedMessageId, "An incorrect item was returned.");
                Assert.IsNotNull(adapter.MessageHandlingSummary);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.WasDelivered);
                Assert.AreEqual(true, adapter.MessageHandlingSummary.ResponseReceived);
                Assert.AreEqual(false, adapter.MessageHandlingSummary.ProcessedAsync);
            }
     
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
            _serviceHost = new System.ServiceModel.ServiceHost(typeof(Open.MOF.Messaging.Test.WcfService.SimpleService));
            _serviceHost.Open();
        }

        private static void StopServiceHost()
        {

            if (_serviceHost != null)
                _serviceHost.Close();
        }
    }
}
