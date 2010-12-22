using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestWcfTwoWay
{
    class Program
    {
        static void Main(string[] args)
        {
            TestDataRequestMessage requestMessage = new TestDataRequestMessage("ThisIsTheRequestMessage");

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(requestMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                FrameworkMessage responseMessage = adapter.SubmitMessage(requestMessage);
                Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.Messaging.Test.Messages.TestDataRequestMessage message published with the property {2}={3}", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString(), "Name", requestMessage.Name));
                TestDataResponseMessage testResponseMessage = responseMessage as TestDataResponseMessage;

                Console.WriteLine(testResponseMessage.Value);
                Console.WriteLine(adapter.MessageHandlingSummary.ToString());
            }
        }
    }
}
