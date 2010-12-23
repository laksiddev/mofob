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
    internal class TwoWayItineraryEsbMessageHandler: EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ProcessRequestResponseChannel>
    {
        public TwoWayItineraryEsbMessageHandler()
            : base()
        {
        }

        public TwoWayItineraryEsbMessageHandler(string channelEndpointName)
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

        public override bool CanSupportMessage(SimpleMessage message)
        {
            IMessageItineraryMapper mapper = ServiceLocator.Current.GetInstance<IMessageItineraryMapper>();
            _cachedItineraryDescription = mapper.MapMessageToItinerary(message);
            List<Type> responseTypes = ((message is FrameworkMessage) ? ((FrameworkMessage)message).ResponseTypes : new List<Type>());

            bool messageHasItinerary = (_cachedItineraryDescription != null);
            // Open question: Should a two way interface be used if no response type has been defined?
            //bool messageSupportsTwoWay = ((responseTypes != null) && (responseTypes.Count > 0));
            // Until all use cases are determined, if a message does not require two-way, it should not be processed as two-way
            bool messageSupportsTwoWay = message.RequiresTwoWay;
            bool isMessageSupported = (messageHasItinerary && messageSupportsTwoWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToItineraryRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequestResponse(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseResponse itineraryResponse
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

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ProcessRequestResponseChannel channel,
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToItineraryRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseResponse itineraryResponse =
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

        private Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseRequest MapMessageToItineraryRequest(SimpleMessage requestMessage)
        {
            _cachedItineraryDescription = MapMessageToItinerary(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ItineraryDescription itineraryDescription = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.ItineraryDescription();
            itineraryDescription.Name = _cachedItineraryDescription.ItineraryName;
            if (_cachedItineraryDescription.ItineraryVersion != null)
                itineraryDescription.Version = _cachedItineraryDescription.ItineraryVersion;

            Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseRequest itineraryRequest = new Open.MOF.BizTalk.Adapters.Proxy.ItineraryTwoWayServiceInstance.SubmitRequestResponseRequest(itineraryDescription, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
