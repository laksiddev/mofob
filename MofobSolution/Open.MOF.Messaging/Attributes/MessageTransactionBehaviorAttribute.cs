using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTransactionBehaviorAttribute : Attribute
    {
        public MessageTransactionBehaviorAttribute(bool supportsTransactions, bool requiresTransactions)
        {
            _supportsTransactions = supportsTransactions;
            _requiresTransactions = requiresTransactions;
        }

        protected bool _supportsTransactions;
        public bool SupportsTransactions
        {
            get { return _supportsTransactions; }
            set { _supportsTransactions = value; }
        }

        protected bool _requiresTransactions;
        public bool RequiresTransactions
        {
            get { return _requiresTransactions; }
            set { _requiresTransactions = value; }
        }

        public MessageBehavior Behavior
        {
            get
            {
                if (_requiresTransactions)
                {
                    return MessageBehavior.TransactionsRequired;
                }
                else if (_supportsTransactions)
                {
                    return MessageBehavior.TransactionsSupported;
                }
                else
                {
                    return MessageBehavior.DataReporting;
                }
            }
        }
    }
}
