using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.BizTalk.Adapters.MessageHandlers;
//using Open.MOF.BizTalk.Services.Proxy;

namespace Open.MOF.BizTalk.Adapters
{
    public class EsbMessagingAdapter : Open.MOF.Messaging.Adapters.MessagingAdapter
    {
        private AsyncResult<MessagingState> _senderAsyncResult;
        private IAsyncResult _handlerAsyncResult;

        protected EsbMessagingAdapter(string channelEndpointName) : base(channelEndpointName)
        {
        }

        protected override bool NativeAsyncSupported
        {
            get 
            {
                return EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName).AsyncSupported; 
            }
        }

        public override string AdapterContext
        {
            get 
            {
                IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);
                string adapterType = this.GetType().AssemblyQualifiedName;
                string handlerContext = ((handler != null) ? handler.HandlerContext : "<Handler type\"null\" />");
                return String.Format("<AdapterContext><Adapter type=\"{0}\" endpointName=\"{1}\">{2}</Adapter></AdapterContext>", adapterType, _channelEndpointName, handlerContext); 
            }
        }

        protected override void PerformSubmitMessageAsync(object state)
        {
            IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);

            if (handler.AsyncSupported)
            {
                _senderAsyncResult = (AsyncResult<MessagingState>)state;
                MessagingState messagingState = (MessagingState)_senderAsyncResult.AsyncState;
                SimpleMessage requestMessage = messagingState.RequestMessage;

                if (!CanSupportMessage(requestMessage))
                    throw new MessagingException("ESB Framework is attempting to deliver a message using an invalid endpoint.");

                _handlerAsyncResult = handler.BeginSubmitMessage(messagingState, new AsyncCallback(MessageDeliveredCallback));
            }
            else
            {
                base.PerformSubmitMessageAsync(state);
            }
        }

        protected override MessagingState PerformSubmitMessage(SimpleMessage requestMessage)
        {
            IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);

            if (!handler.CanSupportMessage(requestMessage))
                throw new MessagingException("ESB Framework is attempting to deliver a message using an invalid endpoint.");

            MessagingState messagingState = handler.PerformSubmitMessage(requestMessage);
            messagingState.HandlingSummary.AdapterContext = AdapterContext;

            return messagingState;
        }

        //protected void Initialize()
        //{
        //    if (_handler == null)
        //    {
        //        if (String.IsNullOrEmpty(_channelEndpointName))
        //            throw new MessagingConfigurationException("ESB Channel Endpoint Name was not found in the application settings.");

        //        ChannelEndpointElement channel = WcfUtilities.FindEndpointByName(_channelEndpointName);
        //        if (channel == null)
        //            throw new MessagingConfigurationException("ESB Channel Endpoint for the defined name was not properly configured in application settings.");

        //        _handler = EsbMessageHandlerFactory.CreateHander(channel);
        //        if (_handler == null)
        //            throw new MessagingConfigurationException("ESB Channel Endpoint is not of a supported interface type.");
        //    }
        //}

        protected void MessageDeliveredCallback(IAsyncResult ar)
        {
            IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);
            MessagingState messagingState = handler.EndSubmitMessage(ar);
            messagingState.HandlingSummary.AdapterContext = AdapterContext;

            if (!_senderAsyncResult.IsCompleted)    // this would only show completed after an exception
            {
                _senderAsyncResult.SetAsCompleted(messagingState, false);

                // For a TwoWay message, signal that the response was received
                if ((messagingState.RequestMessage.RequiresTwoWay) && (messagingState.ResponseMessage != null) && (!(messagingState.ResponseMessage is MessageSubmittedResponse)))
                {
                    if (messagingState.MessageResponseCallback != null)
                        messagingState.MessageResponseCallback(this, new MessageReceivedEventArgs(messagingState.ResponseMessage));
                }
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

        protected override Open.MOF.Messaging.Adapters.AdapterInterfaceType SuportedAdapterInterfaces
        {
            get 
            {
                return Open.MOF.Messaging.Adapters.AdapterInterfaceType.TransactionService;
            }
        }

        protected override bool CanSupportMessage(SimpleMessage message)
        {
            MessageBehavior behavior = message.GetMessageBehavior();
            bool messageHasValidBehavior = ((behavior == MessageBehavior.TransactionsRequired) || (behavior == MessageBehavior.TransactionsSupported));

            bool handlerCanSupportMessage = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName).CanSupportMessage(message);
            bool isMessageSupported = ((messageHasValidBehavior) && (handlerCanSupportMessage));

            return (isMessageSupported);
        }

        public override void Dispose()
        {
            //if (_handler != null)
            //{
            //    _handler.Dispose();
            //    _handler = null;
            //}
        }
    }
}
