using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    internal class ResponseTypeDescription
    {
        public ResponseTypeDescription()
        {
        }

        public ResponseTypeDescription(Type responseType, bool isResponseRequired, bool isResponseTwoWay)
        {
            ResponseType = responseType;
            IsResponseRequired = isResponseRequired;
            IsResponseTwoWay = isResponseTwoWay;
        }

        public Type ResponseType { get; set; }
        public bool IsResponseRequired { get; set; }
        public bool IsResponseTwoWay { get; set; }
    }
}
