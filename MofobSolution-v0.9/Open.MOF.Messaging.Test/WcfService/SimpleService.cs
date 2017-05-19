using System;
using System.Collections.Generic;
using System.Xml;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging.Test.WcfService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public class SimpleService : ISimpleService
    {
        private const string __performSimpleMethodResponseAction = "http://mof.open/MessagingTests/ServiceContracts/1/0/ISimpleService/PerformSimpleMethodResponse";
        private Action<string> _MessageSubmittedHandler = null;

        public System.ServiceModel.Channels.Message PerformSimpleMethod(System.ServiceModel.Channels.Message message)
        {
            XmlDictionaryReader reader = message.GetReaderAtBodyContents();
            XmlDocument requestBody = new XmlDocument();
            requestBody.Load(reader);

            string request = requestBody.OuterXml;
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("Web service method called: {0}\n{1}", "Open.MOF.Messaging.Test.WcfService.SimpleService.PerformSimpleMethod()", request));

            OnMessageSubmitted(request);

            System.ServiceModel.Channels.Message responseMessage;
            XmlDocument responseBody = new XmlDocument();
            responseBody.AppendChild(responseBody.CreateElement("PerformSimpleMethodResponse"));
            responseBody.DocumentElement.InnerText = "MessageResponse";
            responseMessage = Open.MOF.Messaging.Adapters.WcfClientMessagingAdapter.CreateMessageFromXmlDocument(responseBody, __performSimpleMethodResponseAction);   

            return responseMessage;
        }

        public void RegisterMessageHandler(Action<string> MessageSubmittedHandler)
        {
            if (MessageSubmittedHandler != null)
                _MessageSubmittedHandler = MessageSubmittedHandler;
        }

        private void OnMessageSubmitted(string request)
        {
            if (_MessageSubmittedHandler != null)
                _MessageSubmittedHandler(request);
        }
    }
}
