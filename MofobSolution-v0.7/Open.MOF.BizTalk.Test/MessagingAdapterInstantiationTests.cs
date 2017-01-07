using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.BizTalk.Adapters;
using Open.MOF.Messaging.Test.Messages;

// Note that the test messages from the Open.MOF.Messaging.Test project are reused in this test project.

namespace Open.MOF.BizTalk.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MessagingAdapterInstantiationTests
    {
        public MessagingAdapterInstantiationTests()
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

        //[TestMethod]
        public void InstantiateAdapterByMessageTypeGenericTest()
        {
            IMessagingAdapter service;
            
            //service = MessagingAdapter.CreateInstance(typeof(TestDataRequestMessage));

            //Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingAdapter.CreateInstance<TestTransactionRequestMessage>();

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(EsbMessagingAdapter), service.GetType(), "An incorrect type was returned.");
            string endpointName = ((MessagingAdapter)service).ChannelEndpointName;
            Assert.IsTrue(((endpointName == "WSHttpBinding_ITwoWayAsyncVoidTopic") || (endpointName == "WSHttpBinding_ITwoWayAsyncVoidAddressed") || (endpointName == "myMessagingServiceBinding")), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingAdapter.CreateInstance<FaultMessage>();

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(EsbExceptionAdapter), service.GetType(), "An incorrect type was returned.");
            endpointName = ((MessagingAdapter)service).ChannelEndpointName;
            Assert.IsTrue(((endpointName == "myExceptionServiceBinding") || (endpointName == "myQueuedExceptionServiceBinding")), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            //service = MessagingAdapter.CreateInstance<SubscribeRequestMessage>();

            //Assert.IsNotNull(service, "No item was returned.");
            //Assert.AreEqual(service.GetType(), typeof(PubSubMessagingAdapter), "An incorrect type was returned.");
            //Assert.AreEqual(((MessagingAdapter)service).ChannelEndpointName, "myPubSubServiceBinding", "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            //service = MessagingAdapter.CreateInstance<UnsubscribeRequestMessage>();

            //Assert.IsNotNull(service, "No item was returned.");
            //Assert.AreEqual(service.GetType(), typeof(PubSubMessagingAdapter), "An incorrect type was returned.");
            //Assert.AreEqual(((MessagingAdapter)service).ChannelEndpointName, "myPubSubServiceBinding", "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

        }

        [TestMethod]
        public void InstantiateAdapterByMessageTypeTest()
        {
            IMessagingAdapter service;
            
            //service = MessagingAdapter.CreateInstance(typeof(TestDataRequestMessage));

            //Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingAdapter.CreateInstance(typeof(TestTransactionRequestMessage));

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(EsbMessagingAdapter), service.GetType(), "An incorrect type was returned.");
            string endpointName = ((MessagingAdapter)service).ChannelEndpointName;
            Assert.IsTrue(((endpointName == "WSHttpBinding_ITwoWayAsyncVoidTopic") || (endpointName == "WSHttpBinding_ITwoWayAsyncVoidAddressed") || (endpointName == "myOneWayMessagingServiceBinding")), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingAdapter.CreateInstance(typeof(FaultMessage));

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(EsbExceptionAdapter), service.GetType(), "An incorrect type was returned.");
            endpointName = ((MessagingAdapter)service).ChannelEndpointName;
            Assert.IsTrue(((endpointName == "myExceptionServiceBinding") || (endpointName == "myQueuedExceptionServiceBinding")), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            //service = MessagingAdapter.CreateInstance(typeof(SubscribeRequestMessage));

            //Assert.IsNotNull(service, "No item was returned.");
            //Assert.AreEqual(service.GetType(), typeof(PubSubMessagingAdapter), "An incorrect type was returned.");
            //Assert.AreEqual(((MessagingAdapter)service).ChannelEndpointName, "myPubSubServiceBinding", "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            //service = MessagingAdapter.CreateInstance(typeof(UnsubscribeRequestMessage));

            //Assert.IsNotNull(service, "No item was returned.");
            //Assert.AreEqual(service.GetType(), typeof(PubSubMessagingAdapter), "An incorrect type was returned.");
            //Assert.AreEqual(((MessagingAdapter)service).ChannelEndpointName, "myPubSubServiceBinding", "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            //Assert.IsFalse((service.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsTrue((service.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

        }

        [TestMethod]
        public void InstantiateAdapterByMessageTest()
        {
            IMessagingAdapter adapter;

            FrameworkMessage message = new TestTransactionSubmitMessage();

            adapter = MessagingAdapter.CreateInstance(message);

            Assert.IsNotNull(adapter, "No item was returned.");
            Assert.AreEqual(typeof(EsbMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("myOneWayMessagingServiceBinding", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            adapter.Dispose();
            adapter = null;

            message = new TestTransactionRequestMessage();

            adapter = MessagingAdapter.CreateInstance(message);

            Assert.IsNotNull(adapter, "No item was returned.");
            Assert.AreEqual(typeof(EsbMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("myTwoWayMessagingServiceBinding", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            adapter.Dispose();
            adapter = null;

            message = new TestUnrouteableTransactionRequestMessage();
            message.To = new MessagingEndpoint("http://somewhere.com/SomeService", "someEndpointAction");

            adapter = MessagingAdapter.CreateInstance(message);

            Assert.IsNotNull(adapter, "No item was returned.");
            Assert.AreEqual(typeof(EsbMessagingAdapter), adapter.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("myOneWayBundledMessagingServiceBinding", ((MessagingAdapter)adapter).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            FaultMessage faultMessage = new FaultMessage();

            adapter = MessagingAdapter.CreateInstance(faultMessage);

            Assert.IsNotNull(adapter, "No item was returned.");
            Assert.AreEqual(typeof(EsbExceptionAdapter), adapter.GetType(), "An incorrect type was returned.");
            string endpointName = ((MessagingAdapter)adapter).ChannelEndpointName;
            Assert.IsTrue(((endpointName == "myExceptionServiceBinding") || (endpointName == "myQueuedExceptionServiceBinding")), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");
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

            IMessagingAdapter adapter = MessagingAdapter.CreateInstance("EsbOneWayMessagingAdapterDefinition");

            Assert.IsNotNull(adapter);
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");

            System.Reflection.MethodInfo method = adapter.GetType().GetMethod("CanSupportMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, new Type[] { typeof(FrameworkMessage) }, null);

            Assert.IsNotNull(method, "Method could not be found.");

            bool canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { onewayMessage });
            Assert.IsTrue(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { twowayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { pubsubMessage });
            Assert.IsTrue(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { dataMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { faultMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");

            adapter.Dispose();
            adapter = null;

            adapter = MessagingAdapter.CreateInstance("EsbTwoWayMessagingAdapterDefinition");

            Assert.IsNotNull(adapter);
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            method = adapter.GetType().GetMethod("CanSupportMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, new Type[] { typeof(FrameworkMessage) }, null);

            Assert.IsNotNull(method, "Method could not be found.");

            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { onewayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { twowayMessage });
            Assert.IsTrue(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { pubsubMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { dataMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { faultMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");

            adapter.Dispose();
            adapter = null;

            adapter = MessagingAdapter.CreateInstance("EsbExceptionAdapterDefinition");

            Assert.IsNotNull(adapter);
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsTrue((adapter.CanSupportInterface(AdapterInterfaceType.ExceptionService)), "An incorrect item was returned.");
            //Assert.IsFalse((adapter.CanSupportInterface(AdapterInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            method = adapter.GetType().GetMethod("CanSupportMessage", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.InvokeMethod, null, new Type[] { typeof(FrameworkMessage) }, null);

            Assert.IsNotNull(method, "Method could not be found.");

            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { faultMessage });
            Assert.IsTrue(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { onewayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { twowayMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { pubsubMessage });
            Assert.IsFalse(canAdapterSupportMessage, "Adapter is not properly configured.");
            canAdapterSupportMessage = (bool)method.Invoke(adapter, new object[] { dataMessage });
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
