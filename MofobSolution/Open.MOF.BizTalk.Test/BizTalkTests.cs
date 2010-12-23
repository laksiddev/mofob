using System;
using System.Text;
using System.Collections.Generic;
using System.Xml;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;

namespace Open.MOF.BizTalk.Test
{
    /// <summary>
    /// Summary description for BizTalkTests
    /// </summary>
    [TestClass]
    public class BizTalkTests
    {
        private const string __sampleDriverQueryMessageContent = "<kbi-dl-request:DriverLicenseRequest xmlns:j=\"http://niem.gov/niem/domains/jxdm/4.1\" xmlns:nc=\"http://niem.gov/niem/niem-core/2.0\" xmlns:kbi-dl-request=\"http://www.kcjis.state.ks.us/IEPD/DriverLicense/1.0/Request\" xmlns:kbi-dl=\"http://www.kcjis.state.ks.us/IEPD/DriverLicense/1.0\" xmlns:kbi-dl-request-ext=\"http://www.kcjis.state.ks.us/IEPD/DriverLicense/1.0/Request/Extension\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></kbi-dl-request:DriverLicenseRequest>";
        private const string __sampleVehicleQueryMessageContent = "<kbi-vr-request:VehicleSearchRequest xmlns:j=\"http://niem.gov/niem/domains/jxdm/4.1\" xmlns:nc=\"http://niem.gov/niem/niem-core/2.0\" xmlns:kbi-vr-request=\"http://www.kcjis.state.ks.us/IEPD/VehicleRegistration/SearchRequest/1.0\" xmlns:kbi-vr-req-ext=\"http://www.kcjis.state.ks.us/IEPD/VehicleRegistration/extensions/SearchRequest/1.0\" xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\"></kbi-vr-request:VehicleSearchRequest>";

        public BizTalkTests()
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
        public void BizTalkVehicleTest()
        {
            //
            // TODO: Add test logic here
            //
            SimpleMessage requestMessage = new TwoWayMessage();
            XmlDocument messageBody = new XmlDocument();
            messageBody.LoadXml(__sampleVehicleQueryMessageContent);

            requestMessage.LoadContent(messageBody);

            string methodResult;
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance("BizTalkTwoWayMessagingAdapterDefinition"))
            {
                SimpleMessage responseMessage = adapter.SubmitMessage(requestMessage);
                methodResult = responseMessage.ToXmlString();
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void RunBeforeAllTests(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void RunAfterAllTests() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void RunBeforeEachTest() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void RunAfterEachTest() { }
        //
        #endregion
    }
}
