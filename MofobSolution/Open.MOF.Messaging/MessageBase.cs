using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "MessageBase", WrapperNamespace = "http://mofob.open/Messaging/ServiceContracts/1/0/")]
    public abstract class MessageBase
    {
        public MessageBase()
        {
            _messageId = null;
            _to = null;
            _from = null;
            _replyTo = null;
            _senderDescription = null;
            _relatedMessageId = null;
        }

        [MessageBodyMember(Name = "messageId", Order = 101, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected Guid? _messageId;
        public Guid? MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        [MessageBodyMember(Name = "To", Order = 102, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _to;
        public MessagingEndpoint To
        {
            get { return _to; }
            set { _to = value; }
        }

        [MessageBodyMember(Name = "From", Order = 103, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _from;
        public MessagingEndpoint From
        {
            get { return _from; }
            set { _from = value; }
        }

        [MessageBodyMember(Name = "ReplyTo", Order = 104, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _replyTo;
        public MessagingEndpoint ReplyTo
        {
            get { return _replyTo; }
            set { _replyTo = value; }
        }

        [MessageBodyMember(Name = "senderDescription", Order = 105, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected string _senderDescription;
        public string SenderDescription
        {
            get { return _senderDescription; }
            set { _senderDescription = value; }
        }

        [MessageBodyMember(Name = "relatedMessageId", Order = 106, Namespace = "http://mofob.open/Messaging/DataContracts/1/0/")]
        protected Guid? _relatedMessageId;
        public Guid? RelatedMessageId
        {
            get { return _relatedMessageId; }
            set { _relatedMessageId = value; }
        }
    }
}
