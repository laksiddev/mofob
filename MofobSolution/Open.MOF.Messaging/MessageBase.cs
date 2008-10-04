using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public abstract class MessageBase
    {
        protected Guid _messageId;
        public Guid MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        protected MessagingEndpoint _to;
        public MessagingEndpoint To
        {
            get { return _to; }
            set { _to = value; }
        }

        protected MessagingEndpoint _from;
        public MessagingEndpoint From
        {
            get { return _from; }
            set { _from = value; }
        }

        protected MessagingEndpoint _replyTo;
        public MessagingEndpoint ReplyTo
        {
            get { return _replyTo; }
            set { _replyTo = value; }
        }

        protected string _senderDescription;
        public string SenderDescription
        {
            get { return _senderDescription; }
            set { _senderDescription = value; }
        }

        protected Guid _relatedMessageId;
        public Guid RelatedMessageId
        {
            get { return _relatedMessageId; }
            set { _relatedMessageId = value; }
        }
    }
}
