using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessagingEndpoint
    {
        public MessagingEndpoint()
        {
            _uri = null;
            _action = null;
        }

        public MessagingEndpoint(string uri, string action)
        {
            _uri = uri;
            _action = action;
        }

        protected string _uri;
        public string Uri
        {
            get { return _uri; }
            set { _uri = value; }
        }

        protected string _action;
        public string Action
        {
            get { return _action; }
            set { _action = value; }
        }
    }
}
