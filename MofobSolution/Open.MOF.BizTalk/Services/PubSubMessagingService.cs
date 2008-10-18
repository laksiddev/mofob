using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class PubSubMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        protected PubSubMessagingService(string serviceBindingName) : base(serviceBindingName)
        {
        }

        public override IAsyncResult BeginSubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            throw new NotImplementedException();
        }

        public override MessageBase EndSubmitMessage(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return ((Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService) | (Open.MOF.Messaging.Services.ServiceInterfaceType.SubscriptionService)); }
        }

        public override void Dispose()
        {
        }
    }
}
