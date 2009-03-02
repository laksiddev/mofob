using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "FrameworkMessage")]
    public abstract class FrameworkMessage
    {
        protected static Dictionary<Type, MessageTransactionBehaviorAttribute> _transactionMessageAttributeLookup;

        public FrameworkMessage()
        {
            _messageId = Guid.NewGuid();
            _to = null;
            _from = null;
            _replyTo = null;
            _senderDescription = null;
            _relatedMessageId = null;
        }

        [MessageBodyMember(Name = "messageId", Order = 101, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        protected Guid? _messageId;
        public Guid? MessageId
        {
            get { return _messageId; }
            set { _messageId = value; }
        }

        [MessageBodyMember(Name = "To", Order = 102, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _to;
        public MessagingEndpoint To
        {
            get { return _to; }
            set { _to = value; }
        }

        [MessageBodyMember(Name = "From", Order = 103, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _from;
        public MessagingEndpoint From
        {
            get { return _from; }
            set { _from = value; }
        }

        [MessageBodyMember(Name = "ReplyTo", Order = 104, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        protected MessagingEndpoint _replyTo;
        public MessagingEndpoint ReplyTo
        {
            get { return _replyTo; }
            set { _replyTo = value; }
        }

        [MessageBodyMember(Name = "senderDescription", Order = 105, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        protected string _senderDescription;
        public string SenderDescription
        {
            get { return _senderDescription; }
            set { _senderDescription = value; }
        }

        [MessageBodyMember(Name = "relatedMessageId", Order = 106, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
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
                return FrameworkMessage.GetMessageXmlType(this.GetType());
            }
        }

        public string ToXmlString()
        {
            DataContractFormatAttribute att = new DataContractFormatAttribute();
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(this.GetType(), "action");
            System.ServiceModel.Channels.Message message = messageConverter.ToMessage(this);

            return message.GetReaderAtBodyContents().ReadOuterXml();
        }

        public static T FromXmlString<T>(string xml) where T : FrameworkMessage
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            System.ServiceModel.Channels.Message message = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11WSAddressing10, "action", xmlDocument.DocumentElement);
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(typeof(T), "action");

            return (T)messageConverter.FromMessage(message);
        }

        public static string GetMessageXmlType(System.Type messageType)
        {
            if ((typeof(FrameworkMessage).IsAssignableFrom(messageType)) && (!messageType.IsAbstract))
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

        protected internal static MessageBehavior MessageBehaviorForType(Type messageType)
        {
            if (!typeof(FrameworkMessage).IsAssignableFrom(messageType))
                throw new MessagingException("The message type being processed is not defined correctly.  All messages must extend from MessageBase.");

            System.Reflection.PropertyInfo behaviorProperty = messageType.GetProperty("Behavior", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.FlattenHierarchy);
            if (behaviorProperty != null)
            {
                return (MessageBehavior)behaviorProperty.GetValue(null, null);
            }

            if (_transactionMessageAttributeLookup == null)
                _transactionMessageAttributeLookup = new Dictionary<Type, MessageTransactionBehaviorAttribute>();

            MessageTransactionBehaviorAttribute transactionMessageAttribute;
            if (_transactionMessageAttributeLookup.ContainsKey(messageType))
            {
                transactionMessageAttribute = _transactionMessageAttributeLookup[messageType];
            }
            else
            {
                MessageTransactionBehaviorAttribute[] attributes = (MessageTransactionBehaviorAttribute[])messageType.GetCustomAttributes(typeof(MessageTransactionBehaviorAttribute), false);
                if ((attributes != null) && (attributes.Length > 0))
                {
                    transactionMessageAttribute = attributes[0];
                }
                else
                {
                    transactionMessageAttribute = new MessageTransactionBehaviorAttribute(true, false);
                }
                _transactionMessageAttributeLookup.Add(messageType, transactionMessageAttribute);
            }

            return transactionMessageAttribute.Behavior;

        }
    }
}
