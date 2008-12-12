using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class EsbMessagingService : Open.MOF.Messaging.Services.MessagingService
    {
        public EsbMessagingService(string serviceBindingName) : base(serviceBindingName)
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
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.TransactionService); }
        }
        
        public override void Dispose()
        {
        }
    }
}
