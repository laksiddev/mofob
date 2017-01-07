using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class PubSubResponseMessageTypeAttribute : ResponseMessageTypeAttribute
    {
        public PubSubResponseMessageTypeAttribute(Type responseMessageType)
            : base(responseMessageType, false, false)
        {
        }
    }
}
