using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging.Test
{
    [MessageContract(IsWrapped = true, WrapperName = "BogusMessage", WrapperNamespace = "http://mof.open/MessagingTests/ServiceContracts/1/0/")]
    public class BogusMessage
    {
        public string BogusValue;
    }
}
