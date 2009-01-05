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
            _messageId = Guid.NewGuid();
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

        public string MessageXmlType
        {
            get
            {
                return MessageBase.GetMessageXmlType(this.GetType());
            }
        }

        public string ToXmlString()
        {
            DataContractFormatAttribute att = new DataContractFormatAttribute();
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(this.GetType(), "action");
            System.ServiceModel.Channels.Message message = messageConverter.ToMessage(this);

            return message.GetReaderAtBodyContents().ReadOuterXml();
        }

        public static string GetMessageXmlType(System.Type messageType)
        {
            if ((typeof(MessageBase).IsAssignableFrom(messageType)) && (!messageType.IsAbstract))
            {
                System.ServiceModel.MessageContractAttribute[] attributes = 
                    (System.ServiceModel.MessageContractAttribute[])messageType.GetCustomAttributes(typeof(System.ServiceModel.MessageContractAttribute), false);

                if (attributes.Length == 1)
                {
                    string messageNamespace = attributes[0].WrapperNamespace;
                    return messageNamespace + "#" + messageType.Name;
                }
            }

            return String.Empty;
        }
    }
}
