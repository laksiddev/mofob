using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class TwoWayEsbMessageHandler: EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.ProcessRequestResponseChannel>
    {
        public TwoWayEsbMessageHandler()
            : base()
        {
        }

        public TwoWayEsbMessageHandler(string channelEndpointName)
            : base(channelEndpointName)
        {
        }
 
        #region IESBMessageHandler Members

        public override string HandlerContext
        {
            get
            {
                string handlerType = this.GetType().AssemblyQualifiedName;
                return String.Format("<Handler type=\"{0}\"><Channel endpoint=\"{1}\" /></Handler>", handlerType, _channelEndpointName);
            }
        }

        public override bool CanSupportMessage(SimpleMessage message)
        {
            List<Type> responseTypes = ((message is FrameworkMessage) ? ((FrameworkMessage)message).ResponseTypes : new List<Type>());

            bool messageHasSendToAddress = false;
            if (message is FrameworkMessage)
            {
                messageHasSendToAddress = ((((FrameworkMessage)message).To != null) && (((FrameworkMessage)message).To.IsValid()));
            }
            // Until all use cases are determined, if a message does not require two-way, it should not be processed as two-way
            bool messageSupportsTwoWay = message.RequiresTwoWay;
            bool isMessageSupported = (!messageHasSendToAddress && messageSupportsTwoWay);    // (messageHasItinerary && messageSupportsTwoWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequestResponse(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseResponse itineraryResponse
                = channel.EndSubmitRequestResponse(ar);

            SimpleMessage responseMessage = null;
            string messageXml = null;
            // HACK The data coming back from BizTalk is in an odd format.  It needs to be parsed as follows.
            if (itineraryResponse.part is XmlNode[])
            {
                XmlDocument responseDoc = new XmlDocument();
                XmlElement root = responseDoc.CreateElement("root");
                foreach (XmlNode fragment in (XmlNode[])itineraryResponse.part)
                {
                    root.AppendChild(responseDoc.ImportNode(fragment, false));
                }

                messageXml = root.InnerText;
                responseMessage = FrameworkMessage.FromXmlString(messageXml);
            }
            else if (itineraryResponse.part is string)
            {
                messageXml = (string)itineraryResponse.part;
                responseMessage = FrameworkMessage.FromXmlString(messageXml);
            }

            if (responseMessage == null)
            {
                responseMessage = new Open.MOF.Messaging.TwoWayResponseMessage();
                responseMessage.LoadContent(messageXml);
            }

            return responseMessage;
        }

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseResponse itineraryResponse =
                channel.SubmitRequestResponse(itineraryRequest);

            SimpleMessage responseMessage = null;
            string messageXml = null;
            // HACK The data coming back from BizTalk is in an odd format.  It needs to be parsed as follows.
            if (itineraryResponse.part is XmlNode[])
            {
                XmlDocument responseDoc = new XmlDocument();
                XmlElement root = responseDoc.CreateElement("root");
                foreach (XmlNode fragment in (XmlNode[])itineraryResponse.part)
                {
                    root.AppendChild(responseDoc.ImportNode(fragment, false));
                }

                messageXml = root.InnerText;
                responseMessage = FrameworkMessage.FromXmlString(messageXml);
            }
            else if (itineraryResponse.part is string)
            {
                messageXml = (string)itineraryResponse.part;
                responseMessage = FrameworkMessage.FromXmlString(messageXml);
            }

            if (responseMessage == null)
            {
                responseMessage = new Open.MOF.Messaging.TwoWayResponseMessage();
                responseMessage.LoadContent(messageXml);
            }

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseRequest MapMessageToEsbRequest(SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.ItineraryDescription itineraryDescription = null;
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest = new Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayServiceInstance.SubmitRequestResponseRequest(itineraryDescription, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
