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
    internal class TwoWayAddressedEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.ProcessRequestResponseChannel>
    {
        public TwoWayAddressedEsbMessageHandler()
            : base()
        {
        }

        public TwoWayAddressedEsbMessageHandler(string channelEndpointName)
            : base(channelEndpointName)
        {
        }
 
        #region IESBMessageHandler Members

        public override string HandlerContext
        {
            get
            {
                string handlerType = this.GetType().AssemblyQualifiedName;
                //string itineraryName = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryName != null)) ? _cachedItineraryDescription.ItineraryName : String.Empty);
                //string itineraryVersion = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryVersion != null)) ? _cachedItineraryDescription.ItineraryVersion : String.Empty);
                //string itineraryLocation = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.WasItineraryInCache.HasValue)) ? ((_cachedItineraryDescription.WasItineraryInCache.Value) ? "incache" : "lookup") : "notfound");
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
            // Open question: Should a two way interface be used if no response type has been defined?
            //bool messageSupportsTwoWay = ((responseTypes != null) && (responseTypes.Count > 0));
            // Until all use cases are determined, if a message does not require two-way, it should not be processed as two-way
            bool messageSupportsTwoWay = message.RequiresTwoWay;
            bool isMessageSupported = (messageHasSendToAddress && messageSupportsTwoWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.ProcessRequestResponseChannel channel, 
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequestResponse(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.ProcessRequestResponseChannel channel, 
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseResponse itineraryResponse
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

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.ProcessRequestResponseChannel channel, 
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseResponse itineraryResponse =
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

        private Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseRequest MapMessageToEsbRequest(SimpleMessage requestMessage)
        {
            System.ComponentModel.TypeConverter converter = new TwoWayItineraryConverter();
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.Itinerary itinerary = 
                (Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.Itinerary)converter.ConvertFrom(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseRequest itineraryRequest = 
                new Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWayAddressedServiceInstance.SubmitRequestResponseRequest(itinerary, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
