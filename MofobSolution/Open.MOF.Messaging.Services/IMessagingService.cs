using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface IMessagingService : IDisposable
    {
        FrameworkMessage SubmitMessage(FrameworkMessage message);

        FrameworkMessage SubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        IAsyncResult BeginSubmitMessage(FrameworkMessage message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback);

        FrameworkMessage EndSubmitMessage(IAsyncResult ar);

        bool CanSupportInterface(ServiceInterfaceType interfaceType);
    }
}
