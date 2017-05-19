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
    internal class OneWayAddressedQueuedEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.ProcessRequestChannel>
    {
        public OneWayAddressedQueuedEsbMessageHandler()
            : base()
        {
        }

        public OneWayAddressedQueuedEsbMessageHandler(string channelEndpointName)
            : base()
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
                MapMessageToEsbRequest(messagingState.RequestMessage);

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
                MapMessageToEsbRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestResponse response =
                channel.SubmitRequest(itineraryRequest);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.Queued.ItineraryOneWayBundledServiceInstance.SubmitRequestRequest MapMessageToEsbRequest(SimpleMessage requestMessage)
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
