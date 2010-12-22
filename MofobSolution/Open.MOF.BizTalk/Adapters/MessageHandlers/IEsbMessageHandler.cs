using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Adapters.MessageHandlers
{
    internal interface IEsbMessageHandler : IDisposable
    {
        bool AsyncSupported { get; }

        string EndpointConctactName { get; }

        string HandlerContext { get; }

        IAsyncResult BeginSubmitMessage(MessagingState messagingState, AsyncCallback messageDeliveredCallback);

        MessagingState EndSubmitMessage(IAsyncResult ar);

        MessagingState PerformSubmitMessage(FrameworkMessage requestMessage);

        bool CanSupportMessage(FrameworkMessage message);
    }
}
