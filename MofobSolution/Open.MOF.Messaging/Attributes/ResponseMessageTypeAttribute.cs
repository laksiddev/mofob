using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public class ResponseMessageTypeAttribute : Attribute
    {
        public ResponseMessageTypeAttribute(Type responseMessageType)
        {
            if (responseMessageType == null)
                throw new ArgumentException("ResponseMessageType is a required paramter.", "ResponseMessageType");

            _responseMessageType = responseMessageType;
        }

        protected Type _responseMessageType;
        public Type ResponseMessageType
        {
            get { return _responseMessageType; }
            set { _responseMessageType = value; }
        }

        protected bool _isResponseRequired;
        public bool IsResponseRequired
        {
            get { return _isResponseRequired; }
            set { _isResponseRequired = value; }
        }
    }
}
