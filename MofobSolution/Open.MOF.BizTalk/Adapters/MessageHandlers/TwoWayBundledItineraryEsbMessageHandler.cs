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
    internal class TwoWayBundledItineraryEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.ProcessRequestResponseChannel>
    {
        public TwoWayBundledItineraryEsbMessageHandler()
            : base()
        {
        }

        public TwoWayBundledItineraryEsbMessageHandler(string channelEndpointName)
            : base(channelEndpointName)
        {
        }
 
        #region IESBMessageHandler Members

        public override string HandlerContext
        {
            get
            {
                string handlerType = this.GetType().AssemblyQualifiedName;
                string itineraryName = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryName != null)) ? _cachedItineraryDescription.ItineraryName : String.Empty);
                string itineraryVersion = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryVersion != null)) ? _cachedItineraryDescription.ItineraryVersion : String.Empty);
                string itineraryLocation = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.WasItineraryInCache.HasValue)) ? ((_cachedItineraryDescription.WasItineraryInCache.Value) ? "incache" : "lookup") : "notfound");
                return String.Format("<Handler type=\"{0}\"><Channel endpoint=\"{1}\" /><RecentItinerary name=\"{2}\" version=\"{3}\" location=\"{4}\" /></Handler>", handlerType, _channelEndpointName, itineraryName, itineraryVersion, itineraryLocation);
            }
        }

        public override bool CanSupportMessage(FrameworkMessage message)
        {
            List<Type> responseTypes = message.ResponseTypes;

            bool messageHasSendToAddress = ((message.To != null) && (message.To.IsValid()));
            // Open question: Should a two way interface be used if no response type has been defined?
            //bool messageSupportsTwoWay = ((responseTypes != null) && (responseTypes.Count > 0));
            // Until all use cases are determined, if a message does not require two-way, it should not be processed as two-way
            bool messageSupportsTwoWay = message.RequiresTwoWay;
            bool isMessageSupported = (messageHasSendToAddress && messageSupportsTwoWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.ProcessRequestResponseChannel channel, 
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToItineraryRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequestResponse(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override FrameworkMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.ProcessRequestResponseChannel channel, 
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseResponse itineraryResponse
                = channel.EndSubmitRequestResponse(ar);

            FrameworkMessage responseMessage = null;
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
                EventLogUtility.LogWarningMessage(String.Format("Message could not be decoded: {0}", messageXml));
                responseMessage = new MessageSubmittedResponse();
            }

            return responseMessage;
        }

        protected override FrameworkMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.ProcessRequestResponseChannel channel, 
            FrameworkMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToItineraryRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseResponse itineraryResponse =
                channel.SubmitRequestResponse(itineraryRequest);

            FrameworkMessage responseMessage = null;
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
                EventLogUtility.LogWarningMessage(String.Format("Message could not be decoded: {0}", messageXml));
                responseMessage = new MessageSubmittedResponse();
            }

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseRequest MapMessageToItineraryRequest(FrameworkMessage requestMessage)
        {
            // TODO need to change to static itinerary
            IMessageItineraryMapper mapper = ServiceLocator.Current.GetInstance<IMessageItineraryMapper>();
            _cachedItineraryDescription = mapper.MapMessageToItinerary(requestMessage);
            if (_cachedItineraryDescription == null)
                throw new MessagingException("An ESB Itinerary was not found for this message when attempting to send a message.  This indicates an error in the framework since the framework should not attempt to send the message without an Itinerary.");

            System.ComponentModel.TypeConverter converter = new TwoWayItineraryConverter();
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.Itinerary itinerary = (Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.Itinerary)converter.ConvertFrom(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseRequest itineraryRequest = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayBundledServiceInstance.SubmitRequestResponseRequest(itinerary, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
