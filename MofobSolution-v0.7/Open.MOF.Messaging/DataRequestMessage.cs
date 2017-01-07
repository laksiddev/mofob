using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "DataRequestMessage")]
    public abstract class DataRequestMessage<T> : RequestMessage<T> where T : MessageBase
    {
    }
}
