using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    internal interface IEsbMessageHandler : IDisposable
    {
        MessagingResult PerformSubmitMessage(FrameworkMessage message);

        bool CanSupportMessage(FrameworkMessage message);
    }
}
