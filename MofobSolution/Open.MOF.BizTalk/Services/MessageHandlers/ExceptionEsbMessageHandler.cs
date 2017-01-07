using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    internal class ExceptionEsbMessageHandler : IEsbMessageHandler
    {
        private string _channelEndpointName;
        private ChannelFactory<Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.ExceptionHandlingChannel> _channelFactory = null;

        public ExceptionEsbMessageHandler(string channelEndpointName)
        {
            _channelEndpointName = channelEndpointName;
        }

        #region IESBMessageHandler Members

        public MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)message;
            FaultMessageConverter converter = new FaultMessageConverter();
            Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessage)converter.ConvertFrom(localFaultMessage);
            Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.SubmitFaultRequest faultRequest = new Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.SubmitFaultRequest(proxyFaultMessage);

            if (_channelFactory == null)
            {
                _channelFactory = new ChannelFactory<Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.ExceptionHandlingChannel>(_channelEndpointName);
                _channelFactory.Open();
            }

            Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.ExceptionHandlingChannel channel = _channelFactory.CreateChannel();
            channel.Open();
            channel.SubmitFault(faultRequest);
            channel.Close();

            bool wasMessageDelivered = true;
            MessageSubmittedResponse responseMessage = new MessageSubmittedResponse();
            responseMessage.RelatedMessageId = message.MessageId;

            return new MessagingResult(message, wasMessageDelivered, responseMessage);
        }

        public bool CanSupportMessage(FrameworkMessage message)
        {
            bool doesMessageHaveAddress = ((message.To != null) && (message.To.IsValid()));
            bool isMessageRoutable = (doesMessageHaveAddress);

            return (isMessageRoutable);
        }

        #endregion

        public void Dispose()
        {
            if (_channelFactory != null)
            {
                _channelFactory.Close();
                _channelFactory = null;
            }
        }
    }
}
