using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestEsbOneWayAsync
{
    class Program
    {
        private static System.Threading.AutoResetEvent _waitHandle;
        private static IAsyncResult _asyncResult;

        static void Main(string[] args)
        {
            TestTransactionSubmitMessage requestMessage = new TestTransactionSubmitMessage("ThisIsTheTestTransactionSubmitMessage");

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(requestMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                _asyncResult = adapter.BeginSubmitMessage(requestMessage, null, new AsyncCallback(MessageDeliveredCallback));

                _waitHandle = new System.Threading.AutoResetEvent(false);
                _waitHandle.WaitOne();

                FrameworkMessage responseMessage = adapter.EndSubmitMessage(_asyncResult);
                Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.Messaging.Test.Messages.TestTransactionSubmitMessage message published with the property {2}={3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), "Name", requestMessage.Name));
                if (responseMessage is MessageSubmittedResponse)
                {
                    MessageSubmittedResponse testResponseMessage = responseMessage as MessageSubmittedResponse;

                    //Console.WriteLine(testResponseMessage.);
                    Console.WriteLine(adapter.MessageHandlingSummary.ToString());
                }
                else
                {
                    Console.WriteLine(responseMessage.GetType().ToString());
                }
            }
        }

        protected static void MessageDeliveredCallback(IAsyncResult ar)
        {
            _asyncResult = ar;
            _waitHandle.Set();
        }
    }
}
