using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Services
{
    public class WcfClientMessagingService : MessagingService
    {
        protected WcfClientMessagingService(string serviceBindingName) : base(serviceBindingName)
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

        public override ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (ServiceInterfaceType.DataService); }
        }

        public override void Dispose()
        {
        }
    }
}
