using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestItineraryLookupService
{
    class Program
    {
        static void Main(string[] args)
        {
            string messageDescriptorLookup = FrameworkMessage.GetMessageDescriptor(typeof(Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage));

            Open.MOF.BizTalk.Messages.MessageItineraryMappingRequestMessage requestMessage = new Open.MOF.BizTalk.Messages.MessageItineraryMappingRequestMessage(messageDescriptorLookup);

            FrameworkMessage responseMessage;
            using (Open.MOF.Messaging.Adapters.IMessagingAdapter adapter = Open.MOF.Messaging.Adapters.MessagingAdapter.CreateInstance(requestMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                responseMessage = adapter.SubmitMessage(requestMessage);
            }

            if (responseMessage == null)
            {
                Console.WriteLine("No response returned.");
                return;
            }

            Open.MOF.BizTalk.Messages.MessageItineraryMappingResponseMessage mappingResponse = (Open.MOF.BizTalk.Messages.MessageItineraryMappingResponseMessage)responseMessage;
            Console.WriteLine(mappingResponse.ItineraryName);
        }
    }
}
