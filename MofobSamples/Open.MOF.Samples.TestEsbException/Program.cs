using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using Open.MOF.Messaging.Test.Messages;

namespace Open.MOF.Samples.TestEsbException
{
    class Program
    {
        static void Main(string[] args)
        {
            ApplicationException testException = new ApplicationException("This is an expception to test the exception handling framework.");
            FaultMessage faultMessage = new Open.MOF.Messaging.FaultMessage(Guid.NewGuid(), "EsbExceptionSerivce", "Test stubs", testException);

            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(faultMessage))
            {
                if (adapter == null)
                {
                    Console.WriteLine("Could not find a compatible adapter through which the message could be sent.");
                    return;
                }

                FrameworkMessage responseMessage = adapter.SubmitMessage(faultMessage);
                Open.MOF.Messaging.EventLogUtility.LogInformationMessage(String.Format("{0} {1} : Open.MOF.Messaging.FaultMessage message published.", DateTime.Now.ToShortDateString(), DateTime.Now.ToLongTimeString()));
                if (responseMessage is MessageSubmittedResponse)
                {
                    MessageSubmittedResponse testResponseMessage = responseMessage as MessageSubmittedResponse;

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
