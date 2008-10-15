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
        public void InstantiateServiceTest()
        {
            MessagingService service = MessagingService.CreateInstance("WcfClientMessagingService");

            Assert.IsNotNull(service, "No item was returned.");
            Assert.IsTrue((typeof(IMessageService).IsAssignableFrom(service.GetType())), "An incorrect item was returned.");
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
