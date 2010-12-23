using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class OneWayBundledItineraryQueuedEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.ProcessRequestChannel>
    {
        public OneWayBundledItineraryQueuedEsbMessageHandler()
            : base()
        {
        }

        public OneWayBundledItineraryQueuedEsbMessageHandler(string channelEndpointName)
            : base()
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
            bool messageHasSendToAddress = false;
            if (message is FrameworkMessage)
            {
                messageHasSendToAddress = ((((FrameworkMessage)message).To != null) && (((FrameworkMessage)message).To.IsValid()));
            }
            bool messageSupportsOneWay = !message.RequiresTwoWay;
            bool isMessageSupported = (messageHasSendToAddress && messageSupportsOneWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.ProcessRequestChannel channel, 
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest itineraryRequest =
                MapMessageToItineraryRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequest(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.ProcessRequestChannel channel, 
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestResponse response =
                channel.EndSubmitRequest(ar);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.ProcessRequestChannel channel, 
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest itineraryRequest =
                MapMessageToItineraryRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestResponse response =
                channel.SubmitRequest(itineraryRequest);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest MapMessageToItineraryRequest(SimpleMessage requestMessage)
        {
            System.ComponentModel.TypeConverter converter = new OneWayQueuedItineraryConverter();
            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.Itinerary itinerary =
                (Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.Itinerary)converter.ConvertFrom(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest itineraryRequest =
                new Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest(itinerary, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
