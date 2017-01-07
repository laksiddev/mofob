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
    internal class OneWayEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.ProcessRequestChannel>
    {
        public OneWayEsbMessageHandler()
            : base()
        {
        }

        public OneWayEsbMessageHandler(string channelEndpointName)
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
            //IMessageItineraryMapper mapper = ServiceLocator.Current.GetInstance<IMessageItineraryMapper>();
            //_cachedItineraryDescription = mapper.MapMessageToItinerary(message);

            //bool messageHasItinerary = (_cachedItineraryDescription != null);
            bool messageHasSendToAddress = false;
            if (message is FrameworkMessage)
            {
                messageHasSendToAddress = ((((FrameworkMessage)message).To != null) && (((FrameworkMessage)message).To.IsValid()));
            }
            bool messageSupportsOneWay = !message.RequiresTwoWay;
            bool isMessageSupported = (!messageHasSendToAddress && messageSupportsOneWay); // (messageHasItinerary && messageSupportsOneWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.ProcessRequestChannel channel,
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestRequest itineraryRequest =
                MapMessageToEsbRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequest(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.ProcessRequestChannel channel,
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestResponse response =
                channel.EndSubmitRequest(ar);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.ProcessRequestChannel channel,
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestRequest itineraryRequest =
                MapMessageToEsbRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestResponse response =
                channel.SubmitRequest(itineraryRequest);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestRequest MapMessageToEsbRequest(SimpleMessage requestMessage)
        {
            //_cachedItineraryDescription = MapMessageToItinerary(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.ItineraryDescription itineraryDescription = null;    // new Open.MOF.BizTalk.Adapters.Proxy.ItineraryOneWayServiceInstance.ItineraryDescription();
            //itineraryDescription.Name = _cachedItineraryDescription.ItineraryName;
            //if (_cachedItineraryDescription.ItineraryVersion != null)
            //    itineraryDescription.Version = _cachedItineraryDescription.ItineraryVersion;

            Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestRequest itineraryRequest = new Open.MOF.BizTalk.Adapters.Proxy.EsbOneWayServiceInstance.SubmitRequestRequest(itineraryDescription, requestMessage.ToXmlString());

            return itineraryRequest;
        }
    }
}
