using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging.Callback
{
    public delegate void HandleCallbackDelegate(object sender, FrameworkMessage callbackMessage);
    public interface ICallbackHost : IDisposable
    {
        bool IsServiceRunning { get; }

        string EndpointUri { get; }

        void InitializeCallbackHost(HandleCallbackDelegate callbackDelegate);
    }
}
