using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface IMessagingService
    {
        void SubmitMessage(MessageBase message);

        void SubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        MessageBase EndSubmitMessage(IAsyncResult ar);
    }
}
