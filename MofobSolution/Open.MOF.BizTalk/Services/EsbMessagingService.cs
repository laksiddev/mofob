using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.BizTalk.Services.Proxy;

namespace Open.MOF.BizTalk.Services
{
    public class EsbMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        protected ChannelFactory<ProcessTopicOneWayChannel> _topicOneWayChannelFactory = null;
        protected ChannelFactory<ProcessTopicChannel> _topicChannelFactory = null;
        protected ChannelFactory<ProcessRequestOneWayChannel> _itineraryOneWayChannelFactory = null;
        protected ChannelFactory<ProcessRequestChannel> _itineraryChannelFactory = null;

        protected EsbMessagingService(string bindingName) : base(bindingName)
        {
        }
        
        protected override MessagingResult PerformSubmitMessage(MessageBase message)
        {
            if (String.IsNullOrEmpty(_channelEndpointName))
                throw new MessagingConfigurationException("ESB Exception Binding Name not properly configured in application settings.");

            ChannelEndpointElement channel = WcfUtilities.FindEndpointByName(_channelEndpointName);
            bool isOneWayContract = (channel.Contract.IndexOf("OneWay", StringComparison.CurrentCultureIgnoreCase) != -1);

            if (message.To.IsValid())
            {
                return PerformSubmitItineraryMessage(message, isOneWayContract);
            }
            else
            {
                return PerformSubmitTopicMessage(message, isOneWayContract);
            }
        }

        private MessagingResult PerformSubmitTopicMessage(MessageBase message, bool isOneWayContract)
        {
            bool wasMessageDelivered = false;
            MessageSubmittedResponse responseMessage = null;

            if (isOneWayContract)
            {
                SubmitTopicRequestOneWay topicRequest = new SubmitTopicRequestOneWay(message.ToXmlString());

                if (_topicOneWayChannelFactory == null)
                {
                    _topicOneWayChannelFactory = new ChannelFactory<ProcessTopicOneWayChannel>(_channelEndpointName);
                    _topicOneWayChannelFactory.Open();
                }

                ProcessTopicOneWayChannel channel = _topicOneWayChannelFactory.CreateChannel();
                channel.Open();
                channel.SubmitTopic(topicRequest);
                channel.Close();
         
                wasMessageDelivered = true;
                responseMessage = new MessageSubmittedResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }
            else
            {
                SubmitTopicRequest topicRequest = new SubmitTopicRequest(message.ToXmlString());

                if (_topicChannelFactory == null)
                {
                    _topicChannelFactory = new ChannelFactory<ProcessTopicChannel>(_channelEndpointName);
                    _topicChannelFactory.Open();
                }

                ProcessTopicChannel channel = _topicChannelFactory.CreateChannel();
                channel.Open();
                channel.SubmitTopic(topicRequest);
                channel.Close();

                wasMessageDelivered = true;
                responseMessage = new MessageSubmittedResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }

            return new MessagingResult(message, wasMessageDelivered, responseMessage);
        }

        private MessagingResult PerformSubmitItineraryMessage(MessageBase message, bool isOneWayContract)
        {
            bool wasMessageDelivered = false;
            MessageSubmittedResponse responseMessage = null;
            ItineraryConverter itineraryConverter = new ItineraryConverter();
            Itinerary itinerary = (Itinerary)itineraryConverter.ConvertFrom(message.To);

            if (isOneWayContract)
            {
                SubmitRequestRequestOneWay itineraryRequest = new SubmitRequestRequestOneWay(itinerary, message.ToXmlString());

                if (_itineraryOneWayChannelFactory == null)
                {
                    _itineraryOneWayChannelFactory = new ChannelFactory<ProcessRequestOneWayChannel>(_channelEndpointName);
                    _itineraryOneWayChannelFactory.Open();
                }

                ProcessRequestOneWayChannel channel = _itineraryOneWayChannelFactory.CreateChannel();
                channel.Open();
                channel.SubmitRequest(itineraryRequest);
                channel.Close();

                wasMessageDelivered = true;
                responseMessage = new MessageSubmittedResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }
            else
            {
                SubmitRequestRequest itineraryRequest = new SubmitRequestRequest(itinerary, message.ToXmlString());

                if (_itineraryChannelFactory == null)
                {
                    _itineraryChannelFactory = new ChannelFactory<ProcessRequestChannel>(_channelEndpointName);
                    _itineraryChannelFactory.Open();
                }

                ProcessRequestChannel channel = _itineraryChannelFactory.CreateChannel();
                channel.Open();
                channel.SubmitRequest(itineraryRequest);
                channel.Close();

                wasMessageDelivered = true;
                responseMessage = new MessageSubmittedResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }

            return new MessagingResult(message, wasMessageDelivered, responseMessage);
        }

        protected override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService); }
        }
        
        public override void Dispose()
        {
            if (_topicOneWayChannelFactory != null)
            {
                _topicOneWayChannelFactory.Close();
                _topicOneWayChannelFactory = null;
            }
            if (_topicChannelFactory != null)
            {
                _topicChannelFactory.Close();
                _topicChannelFactory = null;
            }
            if (_itineraryOneWayChannelFactory != null)
            {
                _itineraryOneWayChannelFactory.Close();
                _itineraryOneWayChannelFactory = null;
            }
            if (_itineraryChannelFactory != null)
            {
                _itineraryChannelFactory.Close();
                _itineraryChannelFactory = null;
            }
        }
    }
}
