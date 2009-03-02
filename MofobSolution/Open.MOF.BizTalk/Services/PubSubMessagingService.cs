using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class PubSubMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        protected PubSubMessagingService(string bindingName) : base(bindingName)
        {
        }

        protected override MessagingResult PerformSubmitMessage(FrameworkMessage message)
        {
            throw new NotImplementedException();
        }

        protected override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService); }
        }

        protected override bool CanSupportMessage(FrameworkMessage message)
        {
            return ((message is SubscribeRequestMessage) || (message is UnsubscribeRequestMessage));
        }

        public override void Dispose()
        {
        }
    }
}
