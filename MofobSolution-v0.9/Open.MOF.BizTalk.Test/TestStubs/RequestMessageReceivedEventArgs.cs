using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Test.TestStubs
{
    public class RequestMessageReceivedEventArgs : EventArgs
    {
        public RequestMessageReceivedEventArgs(object message, string methodName, string messagePart)
        {
            _message = message;
            _methodName = methodName;
            _messagePart = messagePart;
        }

        private object _message;
        public object Message
        {
            get { return _message; }
        }

        private string _methodName;
        public string MethodName
        {
            get { return _methodName; }
        }

        private string _messagePart;
        public string MessagePart
        {
            get { return _messagePart; }
        }
    }
}
