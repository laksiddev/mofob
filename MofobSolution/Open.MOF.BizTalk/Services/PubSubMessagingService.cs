using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class PubSubMessagingService : Open.MOF.Messaging.Services.MessagingService, Open.MOF.Messaging.Services.IMessageService, Open.MOF.Messaging.Services.ISubscriptionService
    {
        protected PubSubMessagingService() : base()
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

        #region ISubscriptionService Members

        public void ProcessSubscribeRequest(Type messageType, string endpointUri, string action)
        {
            BeginProcessSubscribeRequest(messageType, endpointUri, action, null);
        }

        public IAsyncResult BeginProcessSubscribeRequest(Type messageType, string endpointUri, string action, AsyncCallback messageDeliveredCallback)
        {
            throw new NotImplementedException();
        }

        public string EndSubmitSubscribeRequest(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        public void ProcessUnsubscribeRequest(Type messageType, string endpointUri, string action)
        {
            BeginProcessUnsubscribeRequest(messageType, endpointUri, action, null);
        }

        public IAsyncResult BeginProcessUnsubscribeRequest(Type messageType, string endpointUri, string action, AsyncCallback messageDeliveredCallback)
        {
            throw new NotImplementedException();
        }

        public string EndSubmitUnsubscribeRequest(IAsyncResult ar)
        {
            throw new NotImplementedException();
        }

        #endregion

        public override void Dispose()
        {
        }
    }
}
