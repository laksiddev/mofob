using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "UnsubscribeRequestMessage")]
    public class UnsubscribeRequestMessage : RequestMessage<UnsubscribeRequestMessage> 
    {
        [MessageBodyMember(Name = "subscriptionMessageXmlType", Order = 1, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        private string _subscriptionMessageXmlType;
        public string SubscriptionMessageXmlType
        {
            get { return _subscriptionMessageXmlType; }
            set { _subscriptionMessageXmlType = value; }
        }

        [MessageBodyMember(Name = "endpointUri", Order = 2, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        private string _endpointUri;
        public string EndpointUri
        {
            get { return _endpointUri; }
            set { _endpointUri = value; }
        }

        [MessageBodyMember(Name = "action", Order = 3, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        private string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
    }
}
