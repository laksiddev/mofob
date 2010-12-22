using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public abstract class ResponseMessageTypeAttribute : Attribute
    {
        public ResponseMessageTypeAttribute(Type responseMessageType)
        {
            if (responseMessageType == null)
                throw new ArgumentException("ResponseMessageType is a required paramter.", "ResponseMessageType");

            _responseMessageType = responseMessageType;
            _isResponseRequired = true;
            _isResponseTwoWay = false;
        }

        protected ResponseMessageTypeAttribute(Type responseMessageType, bool isResponseRequired, bool isResponseTwoWay)
        {
            if (responseMessageType == null)
                throw new ArgumentException("ResponseMessageType is a required paramter.", "ResponseMessageType");

            _responseMessageType = responseMessageType;
            _isResponseRequired = isResponseRequired;
            _isResponseTwoWay = isResponseTwoWay;
        }

        protected Type _responseMessageType;
        public Type ResponseMessageType
        {
            get { return _responseMessageType; }
            set { _responseMessageType = value; }
        }

        protected bool _isResponseRequired;
        protected internal bool IsResponseRequired
        {
            get { return _isResponseRequired; }
            set { _isResponseRequired = value; }
        }

        protected bool _isResponseTwoWay;
        protected internal bool IsResponseTwoWay
        {
            get { return _isResponseTwoWay; }
            set { _isResponseTwoWay = value; }
        }
    }
}
