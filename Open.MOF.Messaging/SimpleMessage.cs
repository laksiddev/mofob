using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "SimpleMessage")]
    public abstract class SimpleMessage 
    {
        protected static Dictionary<Type, MessageTransactionBehaviorAttribute> _transactionMessageAttributeLookup;

        private XmlDocument _messageContent;

        public SimpleMessage()
        {
            _messageContent = null;
        }

        public virtual void LoadContent(string xmlContent)
        {
            _messageContent = new XmlDocument();
            _messageContent.LoadXml(xmlContent);
        }

        public virtual void LoadContent(XmlDocument xmlDocument)
        {
            _messageContent = xmlDocument;
        }

        public string MessageDescriptor
        {
            get
            {
                return SimpleMessage.GetMessageDescriptor(this);
            }
        }

        public string MessageXmlType
        {
            get
            {
                return SimpleMessage.GetMessageXmlType(this);
            }
        }

        public virtual string ToXmlString()
        {
            if (_messageContent == null)
                throw new ArgumentNullException("No content has been defined for this message.");

            return _messageContent.OuterXml;
        }

        public virtual XmlDocument ToXmlDocument()
        {
            if (_messageContent == null)
                throw new ArgumentNullException("No content has been defined for this message.");

            return _messageContent;
        }

        //public static T FromXmlString<T>(string xml) where T : GenericMessage
        //{
        //    return (T)GenericMessage.FromXmlString(xml, typeof(T));
        //}

        //public static FrameworkMessage FromXmlString(string xml)
        //{
        //    FrameworkMessage result = null;
        //    try
        //    {
        //        XmlDocument doc = new XmlDocument();
        //        doc.LoadXml(xml);

        //        string messageNamespace = String.Empty;
        //        XmlNode node = doc.DocumentElement.Attributes.GetNamedItem("xmlns");
        //        if (node != null)
        //            messageNamespace = node.Value;

        //        string messageName = doc.DocumentElement.Name;
        //        string messageXmlType = messageNamespace + "#" + messageName;
                
        //        System.Type messageClrType = Open.MOF.Messaging.WcfUtility.FrameworkMessageTypeLookup(messageXmlType);
                
        //        if (messageClrType != null)
        //            result = FrameworkMessage.FromXmlString(xml, messageClrType);
        //    }
        //    catch (Exception) { }

        //    return result;
        //}

        //protected static FrameworkMessage FromXmlString(string xml, System.Type messageType) 
        //{
        //    if (!typeof(FrameworkMessage).IsAssignableFrom(messageType))
        //        throw new ArgumentException("It is not possible to convert the xml string to the specified type since it does not represent a FrameworkMessage type.", "MessageType");
        //    XmlDocument xmlDocument = new XmlDocument();
        //    xmlDocument.LoadXml(xml);

        //    System.ServiceModel.Channels.Message message = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11WSAddressing10, "action", xmlDocument.DocumentElement);
        //    System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(messageType, "action");

        //    return (FrameworkMessage)messageConverter.FromMessage(message);
        //}

        public static string GetMessageDescriptor(SimpleMessage message)
        {
            System.Type messageType = message.GetType();

            // First try to find a message descriptor for the message type
            if ((typeof(FrameworkMessage).IsAssignableFrom(messageType)) && (!messageType.IsAbstract))
            {
                MessageDescriptorAttribute[] descriptorAttributes =
                    (MessageDescriptorAttribute[])messageType.GetCustomAttributes(typeof(MessageDescriptorAttribute), false);

                if (descriptorAttributes.Length == 1)
                {
                    return descriptorAttributes[0].MessageDescriptor;
                }
            }

            // Otherwise return Message XML Type
            return GetMessageXmlType(message);
        }

        public static string GetMessageXmlType(SimpleMessage message)
        {
            System.Type messageType = message.GetType();

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
            else if (!messageType.IsAbstract)
            {
                // todo provide generic implementation using namespace and message name
            }

            return String.Empty;
        }

        //public virtual MessageBehavior GetMessageBehavior()
        //{
        //    return MessageBehavior.TransactionsSupported;
        //}

        public MessageBehavior GetMessageBehavior()
        {
            return SimpleMessage.GetMessageBehavior(this);
        }

        public static MessageBehavior GetMessageBehavior(SimpleMessage message)
        {
            return GetMessageBehavior(message.GetType());
        }

        public static MessageBehavior GetMessageBehavior<T>() where T : FrameworkMessage
        {
            return GetMessageBehavior(typeof(T));
        }

        public static MessageBehavior GetMessageBehavior(Type messageType)
        {
            if (typeof(FrameworkMessage).IsAssignableFrom(messageType))
            {
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
            else if (typeof(SimpleMessage).IsAssignableFrom(messageType))
            {
                return MessageBehavior.TransactionsSupported;
            }
            else
            {
                throw new MessagingException("The message type being processed is not defined correctly.  All messages must extend from MessageBase.");
            }
        }

        public abstract bool RequiresTwoWay { get; }
    }
}
