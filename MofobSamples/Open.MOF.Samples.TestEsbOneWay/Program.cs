using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestEsbOneWay
{
    class Program
    {
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

                FrameworkMessage responseMessage = adapter.SubmitMessage(requestMessage);
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
    }
}
