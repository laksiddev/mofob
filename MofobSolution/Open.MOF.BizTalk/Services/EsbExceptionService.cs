using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.BizTalk.Services.Proxy;

namespace Open.MOF.BizTalk.Services
{
    public class EsbExceptionService : Open.MOF.Messaging.Services.ExceptionService
    {
        protected ChannelFactory<ExceptionHandlingOneWayChannel> _exceptionOneWayChannelFactory = null;
        protected ChannelFactory<ExceptionHandlingChannel> _exceptionChannelFactory = null;

        protected EsbExceptionService(string bindingName) : base(bindingName)
        {
        }

        protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            if (!(message is Open.MOF.Messaging.FaultMessage))
            {
                throw new ArgumentException("An invalid message type was provided.", "message");
            }
            if (String.IsNullOrEmpty(_channelEndpointName))
                throw new MessagingConfigurationException("ESB Exception Binding Name not properly configured in application settings.");

            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)message;
            FaultMessageConverter converter = new FaultMessageConverter();
            Open.MOF.BizTalk.Services.Proxy.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Services.Proxy.FaultMessage)converter.ConvertFrom(localFaultMessage);

            ChannelEndpointElement channelEndpoint = WcfUtilities.FindEndpointByName(_channelEndpointName);
            bool isOneWayContract = (channelEndpoint.Contract.IndexOf("OneWay", StringComparison.CurrentCultureIgnoreCase) != -1);

            bool wasMessageDelivered = false;
            FaultResponse responseMessage = null;
            SubmitFaultRequest faultRequest = new SubmitFaultRequest(proxyFaultMessage);
            if (isOneWayContract)
            {
                if (_exceptionOneWayChannelFactory == null)
                {
                    _exceptionOneWayChannelFactory = new ChannelFactory<ExceptionHandlingOneWayChannel>(_channelEndpointName);
                    _exceptionOneWayChannelFactory.Open();
                }

                ExceptionHandlingOneWayChannel channel = _exceptionOneWayChannelFactory.CreateChannel();
                channel.Open();
                channel.SubmitFault(faultRequest);
                channel.Close();

                wasMessageDelivered = true;
                responseMessage = new FaultResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }
            else
            {
                if (_exceptionChannelFactory == null)
                {
                    _exceptionChannelFactory = new ChannelFactory<ExceptionHandlingChannel>(_channelEndpointName);
                    _exceptionChannelFactory.Open();
                }

                ExceptionHandlingChannel channel = _exceptionChannelFactory.CreateChannel();
                channel.Open();
                SubmitFaultResponse proxyFaultResponse = channel.SubmitFault(faultRequest);
                channel.Close();

                wasMessageDelivered = true;
                responseMessage = new FaultResponse();
                responseMessage.RelatedMessageId = message.MessageId;
            }

            return new MessagingResult(message, wasMessageDelivered, responseMessage);
        }

        protected override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService); }
        }

        protected override bool CanSupportMessage(FrameworkMessage message)
        {
            return (message is Open.MOF.Messaging.FaultMessage);
        }

        public override void Dispose()
        {
            if (_exceptionOneWayChannelFactory != null)
            {
                _exceptionOneWayChannelFactory.Close();
                ((IDisposable)_exceptionOneWayChannelFactory).Dispose();
                _exceptionOneWayChannelFactory = null;
            }
            if (_exceptionChannelFactory != null)
            {
                _exceptionChannelFactory.Close();
                ((IDisposable)_exceptionChannelFactory).Dispose();
                _exceptionChannelFactory = null;
            }
        }
    }
}
