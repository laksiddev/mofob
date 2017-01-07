using System;
using System.Collections.Generic;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test
{
    [MessageTransactionBehavior(false, false)]
    public class TestDataRequestMessage : FrameworkMessage
    {
    }
}
