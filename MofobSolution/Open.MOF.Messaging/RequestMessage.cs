using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "RequestMessage")]
    public abstract class RequestMessage<T> : GenericMessage<T> where T : MessageBase
    {
    }
}
