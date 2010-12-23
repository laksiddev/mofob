using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal class ExceptionEsbMessageHandler : EsbMessageHandler<Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.ExceptionHandlingChannel>
    {
        public ExceptionEsbMessageHandler()
            : base()
        {
        }

        public ExceptionEsbMessageHandler(string channelEndpointName)
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
            bool isFaultMessage = (message is Open.MOF.Messaging.FaultMessage);
            bool isMessageSupported = (isFaultMessage);

            return (isMessageSupported);
        }

        #endregion

        protected override IAsyncResult InvokeChannelBeginAync(Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            MessagingState messagingState, AsyncCallback messageDeliveredCallback)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest = 
                MapMessageToFaultRequest(messagingState.RequestMessage);

            return channel.BeginSubmitFault(faultRequest, messageDeliveredCallback, messagingState);
        }

        protected override SimpleMessage InvokeChannelEndAsync(Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            IAsyncResult ar)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultResponse response = channel.EndSubmitFault(ar);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        protected override SimpleMessage InvokeChannelSync(Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.ExceptionHandlingChannel channel, 
            SimpleMessage requestMessage)
        {
            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest = 
                MapMessageToFaultRequest(requestMessage);

            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultResponse response = 
                channel.SubmitFault(faultRequest);

            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();

            return responseMessage;
        }

        private Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultRequest MapMessageToFaultRequest(SimpleMessage requestMessage)
        {
            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)requestMessage;
            FaultMessageConverter converter = new FaultMessageConverter();
            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.FaultMessage)converter.ConvertFrom(localFaultMessage);
            Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultRequest faultRequest = new Open.MOF.BizTalk.Adapters.Proxy.EsbExceptionServiceInstance.SubmitFaultRequest(proxyFaultMessage);

            return faultRequest;
        }
    }
}
