using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Adapters
{
    public interface IMessagingAdapter : IDisposable
    {
        MessageHandlingSummary MessageHandlingSummary { get; }

        SimpleMessage SubmitMessage(SimpleMessage message);

        SimpleMessage SubmitMessage(SimpleMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessage(SimpleMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        SimpleMessage EndSubmitMessage(IAsyncResult ar);

        bool CanSupportInterface(AdapterInterfaceType interfaceType);
    }
}
