using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
//using Open.MOF.BizTalk.Services.Proxy;

namespace Open.MOF.BizTalk.Services
{
    public class EsbMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        private IEsbMessageHandler _handler = null;

        protected EsbMessagingService(string channelEndpointName) : base(channelEndpointName)
        {
        }
        
        protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            Initialize();

            if (!CanSupportMessage(message))
                throw new MessagingException("ESB Framework is attempting to deliver a message using an invalid endpoint.");

            return _handler.PerformSubmitMessage(message);
        }

        protected void Initialize()
        {
            if (_handler == null)
            {
                if (String.IsNullOrEmpty(_channelEndpointName))
                    throw new MessagingConfigurationException("ESB Channel Endpoint Name was not found in the application settings.");

                ChannelEndpointElement channel = WcfUtilities.FindEndpointByName(_channelEndpointName);
                if (channel == null)
                    throw new MessagingConfigurationException("ESB Channel Endpoint for the defined name not properly configured in application settings.");

                _handler = EsbMessageHandlerFactory.CreateHander(channel);
            }
        }

        //private MessagingResult PerformSubmitTopicMessage(FrameworkMessage message, bool isOneWayContract)
        //{
        //    bool wasMessageDelivered = false;
        //    MessageSubmittedResponse responseMessage = null;

        //    if (isOneWayContract)
        //    {
        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.SubmitRequestRequest topicRequest = new Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.SubmitRequestRequest(message.ToXmlString());

        //        if (_topicOneWayChannelFactory == null)
        //        {
        //            _topicOneWayChannelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.ProcessRequestOneWayChannel>(_channelEndpointName);
        //            _topicOneWayChannelFactory.Open();
        //        }

        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.ProcessRequestOneWayChannel channel = _topicOneWayChannelFactory.CreateChannel();
        //        channel.Open();
        //        channel.SubmitRequest(topicRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new MessageSubmittedResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }
        //    else
        //    {
        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.SubmitRequestRequest topicRequest = new Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.SubmitRequestRequest(message.ToXmlString());

        //        if (_topicChannelFactory == null)
        //        {
        //            _topicChannelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.ProcessRequestChannel>(_channelEndpointName);
        //            _topicChannelFactory.Open();
        //        }

        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesGenericOneWay.ProcessRequestChannel channel = _topicChannelFactory.CreateChannel();
        //        channel.Open();
        //        channel.SubmitRequest(topicRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new MessageSubmittedResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }

        //    return new MessagingResult(message, wasMessageDelivered, responseMessage);
        //}

        //private MessagingResult PerformSubmitItineraryMessage(FrameworkMessage message, bool isOneWayContract)
        //{
        //    bool wasMessageDelivered = false;
        //    MessageSubmittedResponse responseMessage = null;
        //    ItineraryConverter itineraryConverter = new ItineraryConverter();
        //    Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.Itinerary itinerary = (Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.Itinerary)itineraryConverter.ConvertFrom(message);

        //    System.Xml.Serialization.XmlSerializer ser = new System.Xml.Serialization.XmlSerializer(typeof(Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.Itinerary));
        //    StringBuilder sb = new StringBuilder();
        //    System.IO.StringWriter sw = new System.IO.StringWriter(sb);
        //    ser.Serialize(sw, itinerary);
        //    string test = sb.ToString();

        //    if (isOneWayContract)
        //    {
        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequestOneWay itineraryRequest = new Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequestOneWay(itinerary, message.ToXmlString());

        //        if (_itineraryOneWayChannelFactory == null)
        //        {
        //            _itineraryOneWayChannelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestOneWayChannel>(_channelEndpointName);
        //            _itineraryOneWayChannelFactory.Open();
        //        }

        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestOneWayChannel channel = _itineraryOneWayChannelFactory.CreateChannel();
        //        channel.Open();
        //        channel.SubmitRequest(itineraryRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new MessageSubmittedResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }
        //    else
        //    {
        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequest itineraryRequest = new Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.SubmitRequestRequest(itinerary, message.ToXmlString());

        //        if (_itineraryChannelFactory == null)
        //        {
        //            _itineraryChannelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestChannel>(_channelEndpointName);
        //            _itineraryChannelFactory.Open();
        //        }

        //        Open.MOF.BizTalk.Services.Proxy.ItineraryServicesStaticOneWay.ProcessRequestChannel channel = _itineraryChannelFactory.CreateChannel();
        //        channel.Open();
        //        channel.SubmitRequest(itineraryRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new MessageSubmittedResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }

        //    return new MessagingResult(message, wasMessageDelivered, responseMessage);
        //}

        protected override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get 
            {
                return Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService;
            }
        }

        protected override bool CanSupportMessage(FrameworkMessage message)
        {
            Initialize();

            return _handler.CanSupportMessage(message);
        }

        public override void Dispose()
        {
            if (_handler != null)
            {
                _handler.Dispose();
                _handler = null;
            }
        }
    }
}
