using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class TwoWayResponseMessageTypeAttribute : ResponseMessageTypeAttribute
    {
        public TwoWayResponseMessageTypeAttribute(Type responseMessageType)
            : base(responseMessageType, true, true)
        {
        }
    }
}
