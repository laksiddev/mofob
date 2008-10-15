using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface IMessageService
    {
        void SubmitMessageRequest(MessageBase message);

        void SubmitMessageRequest(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessageRequest(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        MessageBase EndSubmitMessageRequest(IAsyncResult ar);
    }
}
