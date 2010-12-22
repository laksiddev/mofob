using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Adapters
{
    public interface IMessagingAdapter : IDisposable
    {
        MessageHandlingSummary MessageHandlingSummary { get; }

        FrameworkMessage SubmitMessage(FrameworkMessage message);

        FrameworkMessage SubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        FrameworkMessage EndSubmitMessage(IAsyncResult ar);

        bool CanSupportInterface(AdapterInterfaceType interfaceType);
    }
}
