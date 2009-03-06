using System;
using System.Collections.Generic;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Configuration;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class PubSubMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        private IEsbMessageHandler _handler = null;

        protected PubSubMessagingService(string channelEndpointName) : base(channelEndpointName)
        {
        }

        protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            Initialize();

            if (!CanSupportMessage(message))
                throw new MessagingException("ESB Framework is attempting to deliver a message using an invalid endpoint.");

            return _handler.PerformSubmitMessage(message);
        }

        protected void Initialize()
        {
            if (_handler == null)
            {
                if (String.IsNullOrEmpty(_channelEndpointName))
                    throw new MessagingConfigurationException("ESB Channel Endpoint Name was not found in the application settings.");

                ChannelEndpointElement channel = WcfUtilities.FindEndpointByName(_channelEndpointName);
                if (channel == null)
                    throw new MessagingConfigurationException("ESB Channel Endpoint for the defined name not properly configured in application settings.");

                _handler = EsbMessageHandlerFactory.CreateHander(channel);
            }
        }

        protected override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService); }
        }

        protected override bool CanSupportMessage(FrameworkMessage message)
        {
            Initialize();

            return _handler.CanSupportMessage(message);
        }

        public override void Dispose()
        {
            if (_handler != null)
            {
                _handler.Dispose();
                _handler = null;
            }
        }
    }
}
