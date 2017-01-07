using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "ResponseMessage")]
    public abstract class ResponseMessage<T> : GenericMessage<T> where T : MessageBase
    {
    }
}
