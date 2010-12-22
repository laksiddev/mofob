using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestEsbTwoWay
{
    class Program
    {
        static void Main(string[] args)
        {
            TestTransactionRequestMessage requestMessage = new TestTransactionRequestMessage("ThisIsTheTestTransactionRequestMessage");

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(requestMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                FrameworkMessage responseMessage = adapter.SubmitMessage(requestMessage);
                Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.Messaging.Test.Messages.TestTransactionRequestMessage message published with the property {2}={3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), "Name", requestMessage.Name));
                if (responseMessage is TestTransactionResponseMessage)
                {
                    TestTransactionResponseMessage testResponseMessage = responseMessage as TestTransactionResponseMessage;

                    Console.WriteLine(testResponseMessage.Value);
                    Console.WriteLine(testResponseMessage.Context);
                    Console.WriteLine(adapter.MessageHandlingSummary.ToString());
                }
                else if (responseMessage is MessageSubmittedResponse)
                {
                    MessageSubmittedResponse testResponseMessage = responseMessage as MessageSubmittedResponse;

                    Console.WriteLine("SHOULD NOT BE HERE");
                    //Console.WriteLine(testResponseMessage.);
                    Console.WriteLine(adapter.MessageHandlingSummary.ToString());
                }
                else
                {
                    Console.WriteLine(responseMessage.GetType().ToString());
                }
            }
        }
    }
}
