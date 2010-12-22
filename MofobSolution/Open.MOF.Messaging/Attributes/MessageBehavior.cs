using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public enum MessageBehavior
    {
        DataReporting,
        TransactionsSupported,
        TransactionsRequired,
        FaultReporting //,
        //SubscriptionControl
    }
}
