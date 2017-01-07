using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Open.MOF.Messaging.Callback.WcfService
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class WcfMessagingCallbackService : IMessagingCallback
    {
        public EventHandler<MessageReceivedEventArgs> MessageReceived;
        #region IMessagingCallback Members
      
        [ServiceKnownType(typeof(FrameworkMessage))]
        public void ProcessResponse(object callbackMessage)
        {
            FrameworkMessage message = null;
            if (callbackMessage is FrameworkMessage)
            {
                message = (FrameworkMessage)callbackMessage;
            }
            else if (callbackMessage is string)
            {
                message = FrameworkMessage.FromXmlString((string)callbackMessage);
            }
            else
            {
                EventLogUtility.LogWarningMessage(String.Format("An unknown message type was received by the callback service and could not be processed: {0}", callbackMessage.GetType().FullName));
            }

            if (message != null)
                OnMessageReceived(message);
        }

        #endregion

        private void OnMessageReceived(FrameworkMessage receivedMessage)
        {
            if (MessageReceived != null)
                MessageReceived(this, new MessageReceivedEventArgs(receivedMessage));
        }
    }
}
