using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.ServiceLocation;

namespace Open.MOF.Messaging.Callback
{
    public class WcfPubSubCallbackAdapter : IPubSubCallbackAdapter
    {
        private const string _constCallbackAction = "http://mof.open/Messaging/ServiceContracts/1/0/IMessagingCallback/ProcessMessage";

        private Dictionary<Guid, EventHandler<MessageReceivedEventArgs>> _callbackHandlers;
        public WcfPubSubCallbackAdapter()
        {
            _callbackHandlers = new Dictionary<Guid, EventHandler<MessageReceivedEventArgs>>();
        }

        #region IPubSubCallbackAdapter Members

        public event EventHandler<UnknownMessageEventArgs> UnknownMessageReceived;

        public void RegisterCallbackHandler(FrameworkMessage requestMessage, EventHandler<MessageReceivedEventArgs> messageResponseCallback)
        {
            if (!requestMessage.MessageId.HasValue)
                return;

            ICallbackHost callbackHost = ServiceLocator.Current.GetInstance<ICallbackHost>();
            if (!_callbackHandlers.ContainsKey(requestMessage.MessageId.Value))
            {
                _callbackHandlers.Add(requestMessage.MessageId.Value, messageResponseCallback);

                if (!callbackHost.IsServiceRunning)
                {
                    callbackHost.InitializeCallbackHost(new HandleCallbackDelegate(HandleCallback));
                }
            }

            requestMessage.ReplyTo = new MessagingEndpoint(callbackHost.EndpointUri, _constCallbackAction);
        }

        public void UnRegisterCallbackHandler(Guid requestMessageId)
        {
            if (_callbackHandlers.ContainsKey(requestMessageId))
                _callbackHandlers.Remove(requestMessageId);
        }

        public void HandleCallback(object sender, FrameworkMessage callbackMessage)
        {
            if ((callbackMessage.RelatedMessageId.HasValue) && (_callbackHandlers.ContainsKey(callbackMessage.RelatedMessageId.Value)))
            {
                EventHandler<MessageReceivedEventArgs> callbackDelegate = _callbackHandlers[callbackMessage.RelatedMessageId.Value];
                callbackDelegate(sender, new MessageReceivedEventArgs(callbackMessage));
            }
            else
            {
                if (UnknownMessageReceived != null)
                {
                    UnknownMessageReceived(null, new UnknownMessageEventArgs(callbackMessage));
                }
            }
        }      

        #endregion
    }
}
