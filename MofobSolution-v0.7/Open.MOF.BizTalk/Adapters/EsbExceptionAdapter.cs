using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Configuration;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;
using Open.MOF.BizTalk.Adapters.MessageHandlers;

namespace Open.MOF.BizTalk.Adapters
{
    public class EsbExceptionAdapter : Open.MOF.Messaging.Adapters.ExceptionAdapter
    {
        protected EsbExceptionAdapter(string channelEndpointName) : base(channelEndpointName)
        {
        }

        protected override bool NativeAsyncSupported
        {
            get { return false; }
        }

        public override string AdapterContext
        {
            get
            {
                IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);

                string adapterType = this.GetType().AssemblyQualifiedName;
                string handlerContext = ((handler != null) ? handler.HandlerContext : "<Handler type\"null\" />");
                return String.Format("<AdapterContext><Adapter type=\"{0}\" endpointName=\"{1}\">{2}</Adapter></AdapterContext>", adapterType, _channelEndpointName, handlerContext);
            }
        }

        protected override MessagingState PerformSubmitMessage(SimpleMessage message)
        {
            IEsbMessageHandler handler = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName);

            if (!handler.CanSupportMessage(message))
                throw new MessagingException("ESB Framework is attempting to deliver a message using an invalid endpoint.");

            MessagingState messagingState = handler.PerformSubmitMessage(message);
            messagingState.HandlingSummary.AdapterContext = AdapterContext;

            return messagingState;
        }

        //protected void Initialize()
        //{
        //    if (_handler == null)
        //    {
        //        if (String.IsNullOrEmpty(_channelEndpointName))
        //            throw new MessagingConfigurationException("ESB Channel Endpoint Name was not found in the application settings.");

        //        ChannelEndpointElement channel = WcfUtilities.FindEndpointByName(_channelEndpointName);
        //        if (channel == null)
        //            throw new MessagingConfigurationException("ESB Channel Endpoint for the defined name was not properly configured in application settings.");

        //        _handler = MessageHandlers.EsbMessageHandlerFactory.CreateHander(channel);
        //        if (_handler == null)
        //            throw new MessagingConfigurationException("ESB Channel Endpoint is not of a supported interface type.");
        //    }
        //}
       
        //protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        //{
        //    if (!(message is Open.MOF.Messaging.FaultMessage))
        //    {
        //        throw new ArgumentException("An invalid message type was provided.", "message");
        //    }
        //    if (String.IsNullOrEmpty(_channelEndpointName))
        //        throw new MessagingConfigurationException("ESB Exception Binding Name not properly configured in application settings.");

        //    Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)message;
        //    FaultMessageConverter converter = new FaultMessageConverter();
        //    Open.MOF.BizTalk.Services.Proxy.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Services.Proxy.FaultMessage)converter.ConvertFrom(localFaultMessage);

        //    ChannelEndpointElement channelEndpoint = WcfUtilities.FindEndpointByName(_channelEndpointName);
        //    bool isOneWayContract = (channelEndpoint.Contract.IndexOf("OneWay", StringComparison.CurrentCultureIgnoreCase) != -1);

        //    bool wasMessageDelivered = false;
        //    FaultResponse responseMessage = null;
        //    SubmitFaultRequest faultRequest = new SubmitFaultRequest(proxyFaultMessage);
        //    if (isOneWayContract)
        //    {
        //        if (_exceptionOneWayChannelFactory == null)
        //        {
        //            _exceptionOneWayChannelFactory = new ChannelFactory<ExceptionHandlingOneWayChannel>(_channelEndpointName);
        //            _exceptionOneWayChannelFactory.Open();
        //        }

        //        ExceptionHandlingOneWayChannel channel = _exceptionOneWayChannelFactory.CreateChannel();
        //        channel.Open();
        //        channel.SubmitFault(faultRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new FaultResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }
        //    else
        //    {
        //        if (_exceptionChannelFactory == null)
        //        {
        //            _exceptionChannelFactory = new ChannelFactory<ExceptionHandlingChannel>(_channelEndpointName);
        //            _exceptionChannelFactory.Open();
        //        }

        //        ExceptionHandlingChannel channel = _exceptionChannelFactory.CreateChannel();
        //        channel.Open();
        //        SubmitFaultResponse proxyFaultResponse = channel.SubmitFault(faultRequest);
        //        channel.Close();

        //        wasMessageDelivered = true;
        //        responseMessage = new FaultResponse();
        //        responseMessage.RelatedMessageId = message.MessageId;
        //    }

        //    return new MessagingResult(message, wasMessageDelivered, responseMessage);
        //}

        protected override Open.MOF.Messaging.Adapters.AdapterInterfaceType SuportedAdapterInterfaces
        {
            get { return (Open.MOF.Messaging.Adapters.AdapterInterfaceType.ExceptionService); }
        }

        protected override bool CanSupportMessage(SimpleMessage message)
        {
            MessageBehavior behavior = message.GetMessageBehavior();
            bool messageHasValidBehavior = (behavior == MessageBehavior.FaultReporting);

            bool handlerCanSupportMessage = EsbMessageHandlerFactory.GetHandlerInstance(_channelEndpointName).CanSupportMessage(message);
            bool isMessageSupported = ((messageHasValidBehavior) && (handlerCanSupportMessage));

            return (isMessageSupported);
        }

        public override void Dispose()
        {
            //if (_handler != null)
            //{
            //    _handler.Dispose();
            //    _handler = null;
            //}
        }
    }
}
