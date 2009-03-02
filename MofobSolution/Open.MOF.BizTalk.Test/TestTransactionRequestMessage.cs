using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test
{
    [MessageTransactionBehavior(true, true)]
    public class TestTransactionRequestMessage : FrameworkMessage
    {
    }
}
