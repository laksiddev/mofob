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
            TestRequestMessage message = new TestRequestMessage();
            message.Name = "TestMessage";
            message.MessageId = Guid.NewGuid();
            message.From = new MessagingEndpoint("http://me/", "myaction");
            string messageText = message.ToXmlString();

            Assert.IsNotNull(messageText);

            TestRequestMessage testMessage = TestRequestMessage.FromXmlString(messageText);
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

        [MessageContract(IsWrapped = true, WrapperName = "TestRequestMessage", WrapperNamespace = "http://mofob.open/MessagingTests/ServiceContracts/1/0/")]
        public class TestRequestMessage : DataRequestMessage<TestRequestMessage>
        {
            public TestRequestMessage() : base()
            {
                _name = null;
            }

            [MessageBodyMember(Name = "name", Order = 1, Namespace = "http://mofob.open/MessagingTests/DataContracts/1/0/")]
            protected string _name;
            public string Name
            {
                get { return _name; }
                set { _name = value; }
            }
        }
    }
}
