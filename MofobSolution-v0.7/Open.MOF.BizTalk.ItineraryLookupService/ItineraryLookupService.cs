using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

using Open.MOF.BizTalk.Messages;
using Open.MOF.BizTalk.ItineraryLookupService.DataAccess;

namespace Open.MOF.BizTalk.ItineraryLookupService
{
    [ServiceBehavior()]
    public class ItineraryLookupService : IItineraryLookupService
    {
        public MessageItineraryMappingResponseMessage ProcessEsbItineraryLookupRequest(MessageItineraryMappingRequestMessage requestMessage)
        {
            MessageItineraryMappingResponseMessage responseMessage = new MessageItineraryMappingResponseMessage();
            responseMessage.RelatedMessageId = requestMessage.MessageId;

            try
            {
                string[] itinerayInfo = ItineraryLookupDac.FindItineraryConnectionStringFromMessageDescriptor(requestMessage.MessageDescriptorLookup);

                if ((itinerayInfo != null) && (itinerayInfo.Length == 2))
                {
                    responseMessage.ItineraryName = itinerayInfo[0];
                    responseMessage.ItineraryVersion = itinerayInfo[1];
                }
            }
            catch (Exception ex)
            {
                Open.MOF.Messaging.EventLogUtility.LogException(ex);
            }
            Open.MOF.Messaging.EventLogUtility.LogInformationMessage("MessageDescriptor = " + requestMessage.MessageDescriptorLookup + ": ItineraryName = " + responseMessage.ItineraryName);

            return responseMessage;
        }
    }
}
