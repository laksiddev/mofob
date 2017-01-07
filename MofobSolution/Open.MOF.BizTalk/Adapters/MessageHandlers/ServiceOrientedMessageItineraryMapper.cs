using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class ServiceOrientedMessageItineraryMapper : IMessageItineraryMapper
    {
        private Dictionary<string, ItineraryDescription> _messageItineraryMapCache;

        public ServiceOrientedMessageItineraryMapper()
        {
            _messageItineraryMapCache = new Dictionary<string, ItineraryDescription>();
        }

        public ItineraryDescription MapMessageToItinerary(SimpleMessage message)
        {
            if (message is Open.MOF.BizTalk.Messages.MessageItineraryMappingRequestMessage)
            {
                throw new MessagingException("Unable to locate an endpoint for Service Oriented Message Itinerary Mapping.  Are you sure that a static WCF Endpoint has been defined for mapping?");
            }

            ItineraryDescription itineraryDescription = null;

            lock (_messageItineraryMapCache)
            {
                if (_messageItineraryMapCache.ContainsKey(message.MessageDescriptor))
                {
                    itineraryDescription = _messageItineraryMapCache[message.MessageDescriptor];
                    if (itineraryDescription != null)       // can be null if no itinerary exists
                        itineraryDescription.WasItineraryInCache = true;
                }
                else
                {
                    Open.MOF.BizTalk.Messages.MessageItineraryMappingRequestMessage mappingRequestMessage = new Open.MOF.BizTalk.Messages.MessageItineraryMappingRequestMessage(message.MessageDescriptor);

                    Open.MOF.BizTalk.Messages.MessageItineraryMappingResponseMessage mappingResponseMessage = null;
                    using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance(mappingRequestMessage))
                    {
                        mappingResponseMessage = (Open.MOF.BizTalk.Messages.MessageItineraryMappingResponseMessage)adapter.SubmitMessage(mappingRequestMessage);
                    }

                    if (mappingResponseMessage != null)
                    {
                        if (mappingResponseMessage.ItineraryName != null)
                        {
                            itineraryDescription = new ItineraryDescription();
                            itineraryDescription.ItineraryName = mappingResponseMessage.ItineraryName;
                            if (mappingResponseMessage.ItineraryVersion != null)
                                itineraryDescription.ItineraryVersion = mappingResponseMessage.ItineraryVersion;
                            itineraryDescription.WasItineraryInCache = false;

                            _messageItineraryMapCache.Add(message.MessageDescriptor, itineraryDescription);
                        }
                        else
                        {
                            _messageItineraryMapCache.Add(message.MessageDescriptor, null);
                        }
                    }
                }
            }

            return itineraryDescription;
        }
    }
}
