using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    internal class StaticItineraryQueuedEsbMessageHandler : IEsbMessageHandler
    {
        private string _channelEndpointName;
        private ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestQueuedChannel> _channelFactory = null;

        public StaticItineraryQueuedEsbMessageHandler(string channelEndpointName)
        {
            _channelEndpointName = channelEndpointName;
        }

        #region IESBMessageHandler Members

        public MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            ItineraryConverter itineraryConverter = new ItineraryConverter();
            Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.Itinerary itinerary = (Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.Itinerary)itineraryConverter.ConvertFrom(message);

            Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequestQueued itineraryRequest = new Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequestQueued(itinerary, message.ToXmlString());

            if (_channelFactory == null)
            {
                _channelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestQueuedChannel>(_channelEndpointName);
                _channelFactory.Open();
            }

            Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestQueuedChannel channel = _channelFactory.CreateChannel();
            channel.Open();
            channel.SubmitRequest(itineraryRequest);
            channel.Close();

            bool wasMessageDelivered = true;
            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();
            responseMessage.RelatedMessageId = message.MessageId;

            return new MessagingResult(message, wasMessageDelivered, responseMessage);
        }

        public bool CanSupportMessage(FrameworkMessage message)
        {
            bool doesMessageHaveAddress = ((message.To != null) && (message.To.IsValid()));
            bool isMessageRoutable = (doesMessageHaveAddress);

            return (isMessageRoutable);
        }

        #endregion

        public void Dispose()
        {
            if (_channelFactory != null)
            {
                _channelFactory.Close();
                _channelFactory = null;
            }
        }
    }
}
