using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Open.MOF.Messaging
{
    public class MessageHandlingSummary
    {
        public MessageHandlingSummary()
        {
        }

        public MessageHandlingSummary(bool wasDelivered, bool responseReceived, bool processedAsync)
            : this(wasDelivered, responseReceived, processedAsync, null)
        {
        }

        public MessageHandlingSummary(bool wasDelivered, bool responseReceived, bool processedAsync, string adapterContext)
        {
            _wasDelivered = wasDelivered;
            _responseReceived = responseReceived;
            _processedAsync = processedAsync;
            _adapterContext = adapterContext;
        }

        private bool _wasDelivered;
        public bool WasDelivered
        {
            get { return _wasDelivered; }
            set { _wasDelivered = value; }
        }

        private bool _responseReceived;
        public bool ResponseReceived
        {
            get { return _responseReceived; }
            set { _responseReceived = value; }
        }

        private bool _processedAsync;
        public bool ProcessedAsync
        {
            get { return _processedAsync; }
            set { _processedAsync = value; }
        }

        private string _adapterContext;
        public string AdapterContext
        {
            get { return _adapterContext; }
            set { _adapterContext = value; }
        }

        public override string ToString()
        {
            return String.Format("WasDelivered={0} : ResponseReceived={1} : ProcessedAsync={2} : context=\n{3}", _wasDelivered.ToString(), _responseReceived.ToString(), _processedAsync.ToString(), ((_adapterContext != null) ? _adapterContext : String.Empty));
        }
    }
}
