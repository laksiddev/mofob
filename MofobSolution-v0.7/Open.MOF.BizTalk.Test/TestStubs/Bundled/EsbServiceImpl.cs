using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Open.MOF.BizTalk.Test.TestStubs.Bundled
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EsbServiceImpl :
            Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.IProcessRequestResponse,
            Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.IProcessRequest
    {
        public EventHandler<RequestMessageReceivedEventArgs> RequestMessageReceived;

        #region IProcessRequestResponse Members

        Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.SubmitRequestResponseResponse Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.IProcessRequestResponse.SubmitRequestResponse(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.SubmitRequestResponseRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "Bundled.ProcessRequestResponse.SubmitRequestResponse", request.part.ToString()));

            Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage requestMessage = Open.MOF.Messaging.FrameworkMessage.FromXmlString(request.part.ToString()) as Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage;
            Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage responseMessage = new Open.MOF.Messaging.Test.Messages.TestTransactionResponseMessage(request.part.ToString(), request.Itinerary.ToString());
            if (requestMessage != null)
                responseMessage.RelatedMessageId = requestMessage.MessageId;

            return new Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryTwoWayService.SubmitRequestResponseResponse(responseMessage.ToXmlString());
        }

        #endregion

        #region Bundled.IProcessRequest Members

        Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestResponse Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.IProcessRequest.SubmitRequest(Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "Bundled.ProcessRequest.SubmitRequest", request.part.ToString() + ":" + request.Itinerary.ToString()));

            return new Open.MOF.BizTalk.Test.TestStubs.Bundled.ItineraryOneWayService.SubmitRequestResponse();
        }

        #endregion

    }
}
