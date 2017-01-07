using System;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging.Test.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    public class SimpleService : ISimpleService
    {
        private const string __performSimpleMethodResponseAction = "http://mof.open/MessagingTests/ServiceContracts/1/0/ISimpleService/PerformSimpleMethodResponse";

        public System.ServiceModel.Channels.Message PerformSimpleMethod(System.ServiceModel.Channels.Message message)
        {
            XmlDictionaryReader reader = message.GetReaderAtBodyContents();
            XmlDocument requestBody = new XmlDocument();
            requestBody.Load(reader);

            string request = requestBody.OuterXml;
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("Web service method called: {0}\n{1}", "Open.MOF.Messaging.Test.WcfService.SimpleService.PerformSimpleMethod()", request));

            System.ServiceModel.Channels.Message responseMessage;
            XmlDocument responseBody = new XmlDocument();
            responseBody.AppendChild(responseBody.CreateElement("PerformSimpleMethodResponse"));
            responseBody.DocumentElement.InnerText = "MessageResponse";
            responseMessage = Open.MOF.Messaging.Adapters.WcfClientMessagingAdapter.CreateMessageFromXmlDocument(responseBody, __performSimpleMethodResponseAction);   

            return responseMessage;
        }
    }
}
