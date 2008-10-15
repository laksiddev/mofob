using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class EsbMessagingService : Open.MOF.Messaging.Services.MessagingService, Open.MOF.Messaging.Services.IMessageService
    {
        protected EsbMessagingService() : base()
        {
        }

        #region IMessageService Members
        
        public void SubmitMessageRequest(RequestMessage message)
        {
            SubmitMessageRequest(message, null);
        }

        public void SubmitMessageRequest(RequestMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            BeginSubmitMessageRequest(message, messageResponseCallback, null);
        }

        public IAsyncResult BeginSubmitMessageRequest(RequestMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            throw new NotImplementedException();
        }

        public RequestMessage EndSubmitMessageRequest(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
