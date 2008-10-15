using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Services
{
    public class WcfClientMessagingService : MessagingService, IMessageService
    {
        protected WcfClientMessagingService() : base()
        {
        }

        #region IMessageService Members

        public void SubmitMessageRequest(MessageBase message)
        {
            SubmitMessageRequest(message, null);
        }

        public void SubmitMessageRequest(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            BeginSubmitMessageRequest(message, messageResponseCallback, null);
        }

        public IAsyncResult BeginSubmitMessageRequest(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            throw new NotImplementedException();
        }

        public MessageBase EndSubmitMessageRequest(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
