using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.ServiceModel;
using System.ServiceModel.Configuration;
using Microsoft.Practices.ServiceLocation;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class TwoWayEsbSoapMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.ProcessSoapChannel>
    {
        public TwoWayEsbSoapMessageHandler()
            : base()
        {
        }

        public TwoWayEsbSoapMessageHandler(string channelEndpointName)
            : base(channelEndpointName)
        {
        }

        #region IESBMessageHandler Members

        public override string HandlerContext
        {
            get
            {
                string handlerType = this.GetType().AssemblyQualifiedName;
                return String.Format("<Handler type=\"{0}\"><Channel endpoint=\"{1}\" /></Handler>", handlerType, _channelEndpointName);
            }
        }

        public override bool CanSupportMessage(SimpleMessage message)
        {
            List<Type> responseTypes = ((message is FrameworkMessage) ? ((FrameworkMessage)message).ResponseTypes : new List<Type>());

            bool messageHasSendToAddress = false;
            if (message is FrameworkMessage)
            {
                messageHasSendToAddress = ((((FrameworkMessage)message).To != null) && (((FrameworkMessage)message).To.IsValid()));
            }
            // Open question: Should a two way interface be used if no response type has been defined?
            //bool messageSupportsTwoWay = ((responseTypes != null) && (responseTypes.Count > 0));
            // Until all use cases are determined, if a message does not require two-way, it should not be processed as two-way
            bool messageSupportsTwoWay = message.RequiresTwoWay;
            bool isMessageSupported = (!messageHasSendToAddress && messageSupportsTwoWay);    // (messageHasItinerary && messageSupportsTwoWay);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.ProcessSoapChannel channel,
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(messagingState.RequestMessage);

            return channel.BeginSubmitRequestResponse(itineraryRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.ProcessSoapChannel channel,
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseResponse itineraryResponse
                = channel.EndSubmitRequestResponse(ar);

            SimpleMessage responseMessage = null;
            string messageXml = null;
            if ((itineraryResponse != null) && (itineraryResponse.Root != null))
            {
                messageXml = itineraryResponse.Root.InnerText;
            }

            if (responseMessage == null)
            {
                responseMessage = new Open.MOF.Messaging.TwoWayResponseMessage();
                responseMessage.LoadContent(messageXml);
            }

            return responseMessage;
        }

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.ProcessSoapChannel channel,
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseRequest itineraryRequest =
                MapMessageToEsbRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseResponse itineraryResponse =
                channel.SubmitRequestResponse(itineraryRequest);

            SimpleMessage responseMessage = null;
            string messageXml = null;
            if ((itineraryResponse != null) && (itineraryResponse.Root != null))
            {
                messageXml = itineraryResponse.Root.InnerText;
            }

            if (responseMessage == null)
            {
                responseMessage = new Open.MOF.Messaging.TwoWayResponseMessage();
                responseMessage.LoadContent(messageXml);
            }

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseRequest MapMessageToEsbRequest(SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.Itinerary itinerary = null; 
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(requestMessage.ToXmlString());

            Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseRequest itineraryRequest = new Open.MOF.BizTalk.Adapters.Proxy.EsbTwoWaySoapInstance.SubmitRequestResponseRequest(itinerary, doc.DocumentElement);

            return itineraryRequest;
        }
    }
}
