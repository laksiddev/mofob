using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Open.MOF.BizTalk.Test.TestStubs.Queued
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class EsbServiceImpl : 
            Open.MOF.BizTalk.Test.TestStubs.Queued.ItineraryOneWayService.IProcessRequest,
            Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.IExceptionHandling
    {
        public EventHandler<RequestMessageReceivedEventArgs> RequestMessageReceived;

        #region ProcessRequestQueued Members

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        void Open.MOF.BizTalk.Test.TestStubs.Queued.ItineraryOneWayService.IProcessRequest.SubmitRequest(Open.MOF.BizTalk.Test.TestStubs.Queued.ItineraryOneWayService.SubmitRequestRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "ProcessRequestQueued.SubmitRequest", request.part.ToString()));
        }

        #endregion

        #region ExceptionHandlingQueued Members

        [OperationBehavior(TransactionScopeRequired = true, TransactionAutoComplete = true)]
        void Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.IExceptionHandling.SubmitFault(Open.MOF.BizTalk.Test.TestStubs.Queued.EsbExceptionService.SubmitFaultRequest request)
        {
            System.Threading.Thread.Sleep(500); // Delay the response for more reliable Async processing

            //Open.MOF.Messaging.EventLogUtility.LogInformationMessage("Open.MOF.BizTalk.Test.Queued.TestStubs.EsbExceptionService.SubmitFault() method called.");
            if (RequestMessageReceived != null)
                RequestMessageReceived(this, new RequestMessageReceivedEventArgs(request, "ExceptionHandlingQueued.SubmitFault", request.FaultMessage.ToString()));
        }

        #endregion
    }
}
