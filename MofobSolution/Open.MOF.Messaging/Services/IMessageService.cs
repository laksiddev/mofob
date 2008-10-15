using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface IMessageService
    {
        void SubmitMessageRequest(RequestMessage message);

        void SubmitMessageRequest(RequestMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessageRequest(RequestMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        RequestMessage EndSubmitMessageRequest(IAsyncResult ar);
    }
}
