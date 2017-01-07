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
    public abstract class EsbMessageHandler<T> : IEsbMessageHandler
    {
        protected string _channelEndpointName;
        protected ChannelFactory<T> _channelFactory = null;
        protected Dictionary<SimpleMessage, T> _asyncChannelCache;
        //internal ItineraryDescription _cachedItineraryDescription;
 
        public EsbMessageHandler()
        {
            _channelEndpointName = null;
            _channelFactory = new ChannelFactory<T>();
            _asyncChannelCache = new Dictionary<SimpleMessage, T>();
            //_cachedItineraryDescription = null;
        }

        public EsbMessageHandler(string channelEndpointName)
        {
            _channelEndpointName = channelEndpointName;
            _channelFactory = new ChannelFactory<T>(_channelEndpointName);
            _asyncChannelCache = new Dictionary<SimpleMessage, T>();
            //_cachedItineraryDescription = null;
        }

        #region IESBMessageHandler Members
        
        public bool AsyncSupported
        {
            get { return true; }
        }

        public string EndpointConctactName
        {
            get { return _channelFactory.Endpoint.Contract.ConfigurationName; }
        }

        public abstract string HandlerContext { get; }
        //{
        //    get
        //    {
        //        string handlerType = this.GetType().AssemblyQualifiedName;
        //        string itineraryName = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryName != null)) ? _cachedItineraryDescription.ItineraryName : String.Empty);
        //        string itineraryVersion = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.ItineraryVersion != null)) ? _cachedItineraryDescription.ItineraryVersion : String.Empty);
        //        string itineraryLocation = (((_cachedItineraryDescription != null) && (_cachedItineraryDescription.WasItineraryInCache.HasValue)) ? ((_cachedItineraryDescription.WasItineraryInCache.Value) ? "incache" : "lookup") : "notfound");
        //        return String.Format("<Handler type=\"{0}\"><Channel endpoint=\"{1}\" /><RecentItinerary name=\"{2}\" version=\"{3}\" location=\"{4}\" /></Handler>", handlerType, _channelEndpointName, itineraryName, itineraryVersion, itineraryLocation);
        //    }
        //}

        public IAsyncResult BeginSubmitMessage(MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            IAsyncResult asyncResult;
            lock (_channelFactory)
            {
                if (_channelFactory.State != CommunicationState.Opened)
                {
                    _channelFactory.Open();
                }

                T channel = _channelFactory.CreateChannel();
                _asyncChannelCache.Add(messagingState.RequestMessage, channel);
                ((ICommunicationObject)channel).Open();
                asyncResult = InvokeChannelBeginAync(channel, messagingState, messageDeliveredCallback);
            }

            return asyncResult;
        }

        public MessagingState EndSubmitMessage(IAsyncResult ar)
        {
            MessagingState messagingState = (MessagingState)ar.AsyncState;
            SimpleMessage requestMessage = messagingState.RequestMessage;
            if (!_asyncChannelCache.ContainsKey(requestMessage))
                throw new ApplicationException("Invalid condition detected.  Communication channel was not found in the cache.");

            SimpleMessage responseMessage = null;
            lock (_asyncChannelCache)
            {
                T channel = _asyncChannelCache[requestMessage];
                _asyncChannelCache.Remove(requestMessage);
                responseMessage = InvokeChannelEndAsync(channel, ar);

                // HACK to prevent Exceptions when closing connection to hide other exceptions
                // See http://msdn.microsoft.com/en-us/library/aa355056.aspx
                try
                {
                    ((ICommunicationObject)channel).Close();
                }
                catch (CommunicationException)
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch (TimeoutException)
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch (Exception)
                {
                    ((ICommunicationObject)channel).Abort();
                    throw;
                }
            }

            bool wasDelivered = true;
            bool responseReceived = (!(responseMessage is MessageSubmittedResponse));
            bool processedAsync = true;
            MessageHandlingSummary handlingSummary = new MessageHandlingSummary(wasDelivered, responseReceived, processedAsync);

            if ((requestMessage is FrameworkMessage) && (responseMessage is FrameworkMessage))
            {
                ((FrameworkMessage)responseMessage).RelatedMessageId = ((FrameworkMessage)requestMessage).MessageId;
            }
            messagingState.ResponseMessage = responseMessage;
            messagingState.HandlingSummary = handlingSummary;

            return messagingState;
        }

        public MessagingState PerformSubmitMessage(SimpleMessage requestMessage)
        {
            SimpleMessage responseMessage = null;
            lock (_channelFactory)
            {
                if (_channelFactory.State != CommunicationState.Opened)
                {
                    _channelFactory.Open();
                }

                T channel = _channelFactory.CreateChannel();
                ((ICommunicationObject)channel).Open();
                responseMessage = InvokeChannelSync(channel, requestMessage);

                // HACK to prevent Exceptions when closing connection to hide other exceptions
                // See http://msdn.microsoft.com/en-us/library/aa355056.aspx
                try
                {
                    ((ICommunicationObject)channel).Close();
                }
                catch (CommunicationException)
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch (TimeoutException)
                {
                    ((ICommunicationObject)channel).Abort();
                }
                catch (Exception)
                {
                    ((ICommunicationObject)channel).Abort();
                    throw;
                }
            }

            bool wasDelivered = true;
            bool responseReceived = (!(responseMessage is MessageSubmittedResponse));
            bool processedAsync = false;
            MessageHandlingSummary handlingSummary = new MessageHandlingSummary(wasDelivered, responseReceived, processedAsync);

            if ((requestMessage is FrameworkMessage) && (responseMessage is FrameworkMessage))
            {
                ((FrameworkMessage)responseMessage).RelatedMessageId = ((FrameworkMessage)requestMessage).MessageId;
            }

            return new MessagingState(requestMessage, handlingSummary, responseMessage);
        }

        public abstract bool CanSupportMessage(SimpleMessage message);

        public void Dispose()
        {
            if (_channelFactory != null)
            {
                _channelFactory.Close();
                _channelFactory = null;
            }
        }

        #endregion

        protected abstract IAsyncResult InvokeChannelBeginAync(T channel, MessagingState messagingState, AsyncCallback messageDeliveredCallback);

        protected abstract SimpleMessage InvokeChannelEndAsync(T channel, IAsyncResult ar);

        protected abstract SimpleMessage InvokeChannelSync(T channel, SimpleMessage requestMessage);

        //internal ItineraryDescription MapMessageToItinerary(SimpleMessage message)
        //{
        //    IMessageItineraryMapper mapper = ServiceLocator.Current.GetInstance<IMessageItineraryMapper>();
        //    ItineraryDescription itineraryDescription = mapper.MapMessageToItinerary(message);
        //    if (itineraryDescription == null)
        //        throw new MessagingException("An ESB Itinerary was not found for this message when attempting to send a message.  This indicates an error in the framework since the framework should not attempt to send the message without an Itinerary.");

        //    return itineraryDescription;
        //}
    }
}
