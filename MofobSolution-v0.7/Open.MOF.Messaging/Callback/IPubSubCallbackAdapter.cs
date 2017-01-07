using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.Messaging.Callback
{
    public interface IPubSubCallbackAdapter
    {
        event EventHandler<UnknownMessageEventArgs> UnknownMessageReceived; 
        
        void RegisterCallbackHandler(FrameworkMessage requestMessage, EventHandler<MessageReceivedEventArgs> messageResponseCallback);

        void UnRegisterCallbackHandler(Guid requestMessageId);

        void HandleCallback(object sender, FrameworkMessage message);
    }
}
