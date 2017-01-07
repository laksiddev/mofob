using System;
using System.Text;
using System.Collections.Generic;

using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Services;

namespace Open.MOF.Messaging.Test
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestClass]
    public class MessagingServiceInstantiationTests
    {
        public MessagingServiceInstantiationTests()
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
        public void InstantiateServiceByNameTest()
        {
            IMessagingService service = MessagingService.CreateInstance("WcfClientMessagingDataServiceDefinitionWSHttpBinding");

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance("BOGUS");

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(String.Empty);

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance((string)null);

            Assert.IsNull(service, "An item was returned when none was expected.");
        }

        [TestMethod]
        public void InstantiateServiceByInterfaceTypeTest()
        {
            IMessagingService service = MessagingService.CreateInstance(ServiceInterfaceType.DataService);

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance(ServiceInterfaceType.TransactionService);

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(ServiceInterfaceType.ExceptionService);

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(ServiceInterfaceType.SubscriptionService);

            Assert.IsNull(service, "An item was returned when none was expected.");
        }

        [TestMethod]
        public void InstantiateServiceByMessageTypeGenericTest()
        {
            IMessagingService service = MessagingService.CreateInstance<TestDataRequestMessage>();

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance<TestTransactionRequestMessage>();

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance<FaultMessage>();

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance<SubscribeRequestMessage>();

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance<UnsubscribeRequestMessage>();

            Assert.IsNull(service, "An item was returned when none was expected.");
        }

        [TestMethod]
        public void InstantiateServiceByMessageTypeTest()
        {
            IMessagingService service = MessagingService.CreateInstance(typeof(TestDataRequestMessage));

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance(typeof(TestTransactionRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(TestDataResponseMessage));

            Assert.IsNotNull(service, "No item was returned.");

            service = MessagingService.CreateInstance(typeof(FaultMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(SubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(UnsubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");
        }

        [TestMethod]
        public void InstantiateServiceByMessageDataRequestTest()
        {
            TestDataRequestMessage request = new TestDataRequestMessage();
            request.Name = "MessageName";

            IMessagingService service = MessagingService.CreateInstance(request);

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance(typeof(TestTransactionRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(FaultMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(SubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(UnsubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");
        }
        [TestMethod]
        public void InstantiateServiceByMessageResponseTest()
        {
            TestDataResponseMessage response = new TestDataResponseMessage();
            response.Value = "This is the content of my message.";
            IMessagingService service = MessagingService.CreateInstance(response);

            Assert.IsNotNull(service, "No item was returned.");
            Assert.AreEqual(typeof(WcfClientMessagingService), service.GetType(), "An incorrect type was returned.");
            Assert.AreEqual("WSHttpBinding_ITestDataService", ((MessagingService)service).ChannelEndpointName, "An incorrect item was returned.");
            Assert.IsTrue((service.CanSupportInterface(ServiceInterfaceType.DataService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.TransactionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.ExceptionService)), "An incorrect item was returned.");
            Assert.IsFalse((service.CanSupportInterface(ServiceInterfaceType.SubscriptionService)), "An incorrect item was returned.");

            service = MessagingService.CreateInstance(typeof(TestTransactionRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(FaultMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(SubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");

            service = MessagingService.CreateInstance(typeof(UnsubscribeRequestMessage));

            Assert.IsNull(service, "An item was returned when none was expected.");
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
