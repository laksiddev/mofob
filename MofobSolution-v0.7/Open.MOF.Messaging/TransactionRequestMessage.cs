using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "TransactionRequestMessage")]
    public abstract class TransactionRequestMessage<T> : RequestMessage<T> where T : MessageBase
    {
    }
}
