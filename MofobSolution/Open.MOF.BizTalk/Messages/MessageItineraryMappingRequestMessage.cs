using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageItineraryMappingRequestMessage", WrapperNamespace = "http://mof.open/BizTalkEsb/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    [TwoWayResponseMessageType(typeof(MessageItineraryMappingResponseMessage))]
    public class MessageItineraryMappingRequestMessage : FrameworkMessage
    {
        public MessageItineraryMappingRequestMessage() : base()
        {
            _messageDescriptorLookup = null;
        }

        public MessageItineraryMappingRequestMessage(string messageDescriptorLookup)
            : base()
        {
            _messageDescriptorLookup = messageDescriptorLookup;
        }

        [MessageBodyMember(Name = "messageDescriptorLookup", Order = 1, Namespace = "http://mof.open/BizTalkEsb/MessageContracts/1/0/")]
        protected string _messageDescriptorLookup;
        public string MessageDescriptorLookup
        {
            get { return _messageDescriptorLookup; }
            set { _messageDescriptorLookup = value; }
        }
    }
}