using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface ISubscriptionService
    {
        void ProcessSubscribeRequest(System.Type messageType, string endpointUri, string action);

        IAsyncResult BeginProcessSubscribeRequest(System.Type messageType, string endpointUri, string action, AsyncCallback messageDeliveredCallback);

        string EndSubmitSubscribeRequest(IAsyncResult ar);

        void ProcessUnsubscribeRequest(System.Type messageType, string endpointUri, string action);

        IAsyncResult BeginProcessUnsubscribeRequest(System.Type messageType, string endpointUri, string action, AsyncCallback messageDeliveredCallback);

        string EndSubmitUnsubscribeRequest(IAsyncResult ar);
    }
}
