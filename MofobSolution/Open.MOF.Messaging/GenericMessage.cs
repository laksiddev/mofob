using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;
using System.Xml;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "Generic")]
    public abstract class GenericMessage<T> : MessageBase where T : MessageBase
    {
        public string ToXmlString()
        {
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(typeof(T), "action");
            System.ServiceModel.Channels.Message message = messageConverter.ToMessage(this);

            return message.GetReaderAtBodyContents().ReadOuterXml();
        }

        public static T FromXmlString(string xml)
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(xml);

            System.ServiceModel.Channels.Message message = System.ServiceModel.Channels.Message.CreateMessage(System.ServiceModel.Channels.MessageVersion.Soap11WSAddressing10, "action", xmlDocument.DocumentElement);
            System.ServiceModel.Description.TypedMessageConverter messageConverter = System.ServiceModel.Description.TypedMessageConverter.Create(typeof(T), "action");

            return (T)messageConverter.FromMessage(message);
        }
    }
}
