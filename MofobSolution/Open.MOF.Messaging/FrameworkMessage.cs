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

        public string MessageDescriptor
        {
            get
            {
                return FrameworkMessage.GetMessageDescriptor(this.GetType());
            }
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
            return (T)FrameworkMessage.FromXmlString(xml, typeof(T));
        }

        public static FrameworkMessage FromXmlString(string xml)
        {
            FrameworkMessage result = null;
            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);

                string messageNamespace = String.Empty;
                XmlNode node = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
                if (node != null)
                    messageNamespace = node.Value;

                string messageName = doc.DocumentElement.Name;
                string messageXmlType = messageNamespace + "#" + messageName;
                
                System.Type messageClrType = Open.MOF.Messaging.WcfUtility.FrameworkMessageTypeLookup(messageXmlType);
                
                if (messageClrType != null)
                    result = FrameworkMessage.FromXmlString(xml, messageClrType);
            }
            catch (Exception) { }

            return result;
        }

        protected static FrameworkMessage FromXmlString(string xml, System.Type messageType) 
        {
            if (!typeof(FrameworkMessage).IsAssignableFrom(messageType))
                throw new ArgumentException("It is not possible to convert the xml string to the specified type since it does not represent a FrameworkMessage type.", "MessageType");
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            System.ServiceModel.Channels.Message message = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11WSAddressing10, "action", xmlDocument.DocumentElement);
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(messageType, "action");

            return (FrameworkMessage)messageConverter.FromMessage(message);
        }

        public static string GetMessageDescriptor(System.Type messageType)
        {
            // First try to find a message descriptor for the message type
            if ((typeof(FrameworkMessage).IsAssignableFrom(messageType)) && (!messageType.IsAbstract))
            {
                MessageDescriptorAttribute[] descriptorAttributes =
                    (MessageDescriptorAttribute[])messageType.GetCustomAttributes(typeof(MessageDescriptorAttribute), false);

                if (descriptorAttributes.Length == 1)
                {
                    return descriptorAttributes[0].MessageDescriptor;
                }

                // Otherwise return Message XML Type
                return GetMessageXmlType(messageType);
            }

            return String.Empty;
        }

        public static string GetMessageXmlType(System.Type messageType)
        {
            if ((typeof(FrameworkMessage).IsAssignableFrom(messageType)) && (!messageType.IsAbstract))
            {
                System.ServiceModel.MessageContractAttribute[] contractAttributes = 
                    (System.ServiceModel.MessageContractAttribute[])messageType.GetCustomAttributes(typeof(System.ServiceModel.MessageContractAttribute), false);

                if (contractAttributes.Length == 1)
                {
                    string messageNamespace = contractAttributes[0].WrapperNamespace;
                    return messageNamespace + "#" + contractAttributes[0].WrapperName;
                }
            }

            return String.Empty;
        }

        public MessageBehavior GetMessageBehavior()
        {
            return FrameworkMessage.GetMessageBehavior(this);
        }

        public static MessageBehavior GetMessageBehavior(FrameworkMessage message)
        {
            return GetMessageBehavior(message.GetType());
        }

        public static MessageBehavior GetMessageBehavior<T>() where T : FrameworkMessage
        {
            return GetMessageBehavior(typeof(T));
        }

        public static MessageBehavior GetMessageBehavior(Type messageType)
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
                    transactionMessageAttribute = new MessageTransactionBehaviorAttribute();
                    transactionMessageAttribute.SupportsTransactions = true;
                    transactionMessageAttribute.RequiresTransactions = false;
                }
                _transactionMessageAttributeLookup.Add(messageType, transactionMessageAttribute);
            }

            return transactionMessageAttribute.Behavior;

        }

        public bool RequiresTwoWay
        {
            get
            {
                List<ResponseTypeDescription> messageResponseDescriptions = this.ResponseTypesForRequestType;
                bool messageRequiresTwoWay = false;     // At least one response type requires two way 
                if ((messageResponseDescriptions != null) && (messageResponseDescriptions.Count > 0))
                {
                    foreach (ResponseTypeDescription messageResponseDescription in messageResponseDescriptions)
                    {
                        if ((messageResponseDescription.ResponseType != null) &&
                            (messageResponseDescription.IsResponseRequired) &&
                            (messageResponseDescription.IsResponseTwoWay))
                        {
                            messageRequiresTwoWay = true;
                            break;
                        }
                    }
                }

                return messageRequiresTwoWay;
            }
        }

        public List<Type> ResponseTypes
        {
            get
            {
                List<Type> responseTypes = new List<Type>();
                foreach (ResponseTypeDescription messageResponseDescription in this.ResponseTypesForRequestType)
                {
                    if (messageResponseDescription.ResponseType != null)
                    {
                        responseTypes.Add(messageResponseDescription.ResponseType);
                    }
                }

                return responseTypes;
            }
        }

        internal List<ResponseTypeDescription> ResponseTypesForRequestType
        {
            get
            {
                ResponseMessageTypeAttribute[] attributes = (ResponseMessageTypeAttribute[])this.GetType().GetCustomAttributes(typeof(ResponseMessageTypeAttribute), true);
                if (attributes != null)
                {
                    List<ResponseTypeDescription> result = new List<ResponseTypeDescription>();
                    for (int i = 0; i < attributes.Length; i++)
                    {
                        result.Add(new ResponseTypeDescription(attributes[i].ResponseMessageType, attributes[i].IsResponseRequired, attributes[i].IsResponseTwoWay));
                    }

                    return result;
                }

                return null;
            }
        }
    }
}
