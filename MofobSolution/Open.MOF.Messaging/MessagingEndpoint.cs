using System;
using System.Collections.Generic;
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

        public bool IsValid()
        {
            return IsValid(false);
        }

        protected bool IsValid(bool verifyEndpointConnectivity)
        {
            if (String.IsNullOrEmpty(_uri))
                return false;

            if (String.IsNullOrEmpty(_action))
                return false;

            if (verifyEndpointConnectivity)
            {
            }

            return true;
        }
    }
}
