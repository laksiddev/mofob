using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
    public class MessageTransactionBehaviorAttribute : Attribute
    {
        public MessageTransactionBehaviorAttribute()
        {
            _supportsTransactions = true;
            _requiresTransactions = false;
        }

        protected bool _supportsTransactions;
        public bool SupportsTransactions
        {
            get { return _supportsTransactions; }
            set 
            { 
                _supportsTransactions = value;
                if (_supportsTransactions == false)
                    _requiresTransactions = false;
            }
        }

        protected bool _requiresTransactions;
        public bool RequiresTransactions
        {
            get { return _requiresTransactions; }
            set 
            { 
                _requiresTransactions = value;
                if (_requiresTransactions == true)
                    _supportsTransactions = true;
            }
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
