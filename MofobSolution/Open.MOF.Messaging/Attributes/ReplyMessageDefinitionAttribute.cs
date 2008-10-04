using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    public class ReplyMessageDefinitionAttribute
    {
        public ReplyMessageDefinitionAttribute(Type replyMessageType)
        {
            if (replyMessageType == null)
                throw new ArgumentException("ReplyMessageType is a required paramter.", "ReplyMessageType");

            _replyMessageType = replyMessageType;
        }

        protected Type _replyMessageType;
        public Type ReplyMessageType
        {
            get { return _replyMessageType; }
            set { _replyMessageType = value; }
        }

        protected bool _isReplyRequired;
        public bool IsReplyRequired
        {
            get { return _isReplyRequired; }
            set { _isReplyRequired = value; }
        }
    }
}
