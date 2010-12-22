using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Open.MOF.BizTalk.Test.TestStubs
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EsbServiceImpl :
            Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.IProcessRequestResponse,
            Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.IProcessRequest,
            Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.IExceptionHandling 
    {
        public EventHandler<RequestMessageReceivedEventArgs> RequestMessageReceived;

        #region IProcessRequestResponse Members

        Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseResponse Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.IProcessRequestResponse.SubmitRequestResponse(Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "ProcessRequestResponse.SubmitRequestResponse", request.part.ToString()));
            
            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage requestMessage = Open.MOF.Messaging.FrameworkMessage.FromXmlString(request.part.ToString()) as Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage;
            Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage response = new Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage(request.part.ToString(), request.ItineraryDescription.Name + ((request.ItineraryDescription.Version != null) ? ":" + request.ItineraryDescription.Version : ""));
            if (requestMessage != null) 
                response.RelatedMessageId = requestMessage.MessageId;

            return new Open.MOF.BizTalk.Test.TestStubs.ItineraryTwoWayService.SubmitRequestResponseResponse(response.ToXmlString());
        }

        #endregion

        #region IProcessRequest Members

        Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestResponse Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.IProcessRequest.SubmitRequest(Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "ProcessRequest.SubmitRequest", request.part.ToString()));

            if ((request != null) && (request.part != null))
            {
                string messagePart = request.part.ToString();
                Open.MOF.Messaging.FrameworkMessage message = Open.MOF.Messaging.FrameworkMessage.FromXmlString(messagePart);
                if (message is Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage)
                {
                    Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage pubsubMessage = (Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage)message;
                    if ((pubsubMessage.ReplyTo != null) && (pubsubMessage.ReplyTo.IsValid()))
                    {
                        Open.MOF.Messaging.Test.Messages.TestPubSubResponseMessage responseMessage = new Open.MOF.Messaging.Test.Messages.TestPubSubResponseMessage(request.part.ToString(), request.ItineraryDescription.Name + ((request.ItineraryDescription.Version != null) ? ":" + request.ItineraryDescription.Version : ""));
                        responseMessage.RelatedMessageId = pubsubMessage.MessageId;
                        SubmitResponseMessage(responseMessage);
                    }
                }
            }

            return new Open.MOF.BizTalk.Test.TestStubs.ItineraryOneWayService.SubmitRequestResponse();
        }

        #endregion

        #region IExceptionHandling Members

        Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFaultResponse Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.IExceptionHandling.SubmitFault(Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFaultRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "ExceptionHandling.SubmitFault", request.FaultMessage.ToString()));

            return new Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFaultResponse();
        }

        #endregion

        private Open.MOF.BizTalk.Test.WcfMessagingCallbackService.IMessagingCallback.MessagingCallbackClient _client;
        private void SubmitResponseMessage(Open.MOF.Messaging.FrameworkMessage responseMessage)
        {
            _client = new Open.MOF.BizTalk.Test.WcfMessagingCallbackService.IMessagingCallback.MessagingCallbackClient("WSHttpBinding_IMessagingCallback");
            _client.BeginProcessResponse(new Open.MOF.BizTalk.Test.WcfMessagingCallbackService.IMessagingCallback.ProcessResponseRequest(responseMessage.ToXmlString()), new AsyncCallback(SubmitResponseCallback), null);
        }

        private void SubmitResponseCallback(IAsyncResult ar)
        {
            _client.Close();
        }
    }
}
