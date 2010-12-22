using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class ExceptionQueuedEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.ExceptionHandlingChannel>
    {
         public ExceptionQueuedEsbMessageHandler()
            : base()
        {
        }
  
        public ExceptionQueuedEsbMessageHandler(string channelEndpointName)
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

        public override bool CanSupportMessage(FrameworkMessage message)
        {
            bool isFaultMessage = (message is Open.MOF.Messaging.FaultMessage);
            bool isMessageSupported = (isFaultMessage);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest =
                MapMessageToFaultRequest(messagingState.RequestMessage);

            return channel.BeginSubmitFault(faultRequest, messageDeliveredCallback, messagingState);
        }

        protected override FrameworkMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            IAsyncResult ar)
        {
            channel.EndSubmitFault(ar);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        protected override FrameworkMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            FrameworkMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest =
                MapMessageToFaultRequest(requestMessage);

            channel.SubmitFault(faultRequest);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.SubmitFaultRequest MapMessageToFaultRequest(FrameworkMessage requestMessage)
        {
            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)requestMessage;
            QueuedFaultMessageConverter converter = new QueuedFaultMessageConverter();
            Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.FaultMessage)converter.ConvertFrom(localFaultMessage);
            Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest = new Open.MOF.BizTalk.Adapters.Proxy.Queued.EsbExceptionServiceInstance.SubmitFaultRequest(proxyFaultMessage);

            return faultRequest;
        }
    }
}
