using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Messages
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageItineraryMappingResponseMessage", WrapperNamespace = "http://mof.open/BizTalkEsb/MessageContracts/1/0/")]
    [MessageTransactionBehavior(SupportsTransactions = true, RequiresTransactions = false)]
    public class MessageItineraryMappingResponseMessage : FrameworkMessage
    {
        public MessageItineraryMappingResponseMessage() : base()
        {
            _itineraryName = null;
            _itineraryVersion = null;
        }

        public MessageItineraryMappingResponseMessage(string itineraryName, string itineraryVersion)
            : base()
        {
            _itineraryName = itineraryName;
            _itineraryVersion = itineraryVersion;
        }

        [MessageBodyMember(Name = "itineraryName", Order = 1, Namespace = "http://mof.open/BizTalkEsb/MessageContracts/1/0/")]
        protected string _itineraryName;
        public string ItineraryName
        {
            get { return _itineraryName; }
            set { _itineraryName = value; }
        }

        [MessageBodyMember(Name = "itineraryVersion", Order = 1, Namespace = "http://mof.open/BizTalkEsb/MessageContracts/1/0/")]
        protected string _itineraryVersion;
        public string ItineraryVersion
        {
            get { return _itineraryVersion; }
            set { _itineraryVersion = value; }
        }
    }
}