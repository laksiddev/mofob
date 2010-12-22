using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestEsbPubSub
{
    class Program
    {
        private static System.Threading.AutoResetEvent _waitHandle;
        
        static void Main(string[] args)
        {
            TestPubSubRequestMessage requestMessage = new TestPubSubRequestMessage("ThisIsTheTestPubSubRequestMessage");
            //requestMessage.To = new MessagingEndpoint(Properties.Settings.Default.ProcessMessageServiceUrl, Properties.Settings.Default.ProcessMessageServiceAction);

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(requestMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                FrameworkMessage responseMessage = adapter.SubmitMessage(requestMessage, new EventHandler<MessageReceivedEventArgs>(ResponseMessageReceivedHandler));
                Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.Messaging.Test.Messages.TestPubSubRequestMessage message published with the property {2}={3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), "Name", requestMessage.Name));
                if (responseMessage is MessageSubmittedResponse)
                {
                    MessageSubmittedResponse testResponseMessage = responseMessage as MessageSubmittedResponse;

                    //Console.WriteLine(testResponseMessage.Value);
                    //Console.WriteLine(testResponseMessage.Context);
                    Console.WriteLine(adapter.MessageHandlingSummary.ToString());
                }
                else
                {
                    Console.WriteLine(responseMessage.GetType().ToString());
                }
            }

            _waitHandle = new System.Threading.AutoResetEvent(false);
            _waitHandle.WaitOne();

            Console.WriteLine("Done");
        }

        private static void ResponseMessageReceivedHandler(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message is TestPubSubResponseMessage)
            {
                TestPubSubResponseMessage testResponseMessage = (TestPubSubResponseMessage)args.Message;
                Console.WriteLine(testResponseMessage.Value);
                Console.WriteLine(testResponseMessage.Context);
            }
            else
            {
                Console.WriteLine(args.Message.GetType().ToString());
            }

            _waitHandle.Set();
        }
    }
}
