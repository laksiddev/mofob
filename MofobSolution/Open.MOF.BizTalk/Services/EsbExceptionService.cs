using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

using Open.MOF.Messaging;

namespace Open.MOF.BizTalk.Services
{
    public class EsbExceptionService : Open.MOF.Messaging.Services.ExceptionService
    {
        protected EsbExceptionService(string serviceBindingName) : base(serviceBindingName)
        {
        }

        public override IAsyncResult BeginSubmitMessage(MessageBase message, EventHandler<MessageReceivedEventArgs> messageResponseCallback, AsyncCallback messageDeliveredCallback)
        {
            if (!(message is Open.MOF.Messaging.FaultMessage))
            {
                throw new ArgumentException("An invalid message type was provided.", "message");
            }
            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)message;
            

            IAsyncResult ar = new AsyncResult<Open.MOF.Messaging.FaultMessage>(messageDeliveredCallback, localFaultMessage);
            if (messageDeliveredCallback != null)
            {
                ThreadPool.QueueUserWorkItem(PerformSubmitException, ar);
            }
            else
            {
                PerformSubmitException(ar);
            }

            return ar;
        }

        protected void PerformSubmitException(object state)
        {
            if (!(state is IAsyncResult))
                throw new ArgumentException("Invalid information passed to function.", "state");
            if (!(((IAsyncResult)state).AsyncState is Open.MOF.Messaging.FaultMessage))
                throw new ArgumentException("Invalid information passed to function.", "IAsyncResult.AsyncState");
 
            IAsyncResult ar = (IAsyncResult)state;
            Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)ar.AsyncState;
            FaultMessageConverter converter = new FaultMessageConverter();
            Open.MOF.BizTalk.Services.Proxy.FaultMessage proxyFaultMessage = (Open.MOF.BizTalk.Services.Proxy.FaultMessage)converter.ConvertFrom(localFaultMessage);

            if (String.IsNullOrEmpty(_serviceBindingName))
                throw new ApplicationException("ESB Exception Binding Name not properly configured in application settings.");

            Open.MOF.BizTalk.Services.Proxy.ExceptionHandlingClient exceptionClient = null;
            try
            {
                exceptionClient = new Open.MOF.BizTalk.Services.Proxy.ExceptionHandlingClient(_serviceBindingName);
                if (exceptionClient.State != System.ServiceModel.CommunicationState.Opened)
                {
                    exceptionClient.Open();
                }

                exceptionClient.SubmitFault(proxyFaultMessage);
            }
            catch (Exception ex)
            {
                EventLogUtility.LogException(ex);
                SetMessagingAsException(ar, ex);
            }
            finally
            {
                if ((exceptionClient != null)
                    && (exceptionClient.State == System.ServiceModel.CommunicationState.Opened))
                {
                    exceptionClient.Close();
                }
            }

            if (exceptionClient == null)
            {
                Open.MOF.BizTalk.Services.Proxy.ExceptionHandlingOneWayClient exceptionOneWayClient = null;
                try
                {
                    exceptionOneWayClient = new Open.MOF.BizTalk.Services.Proxy.ExceptionHandlingOneWayClient(_serviceBindingName);
                    if (exceptionOneWayClient.State != System.ServiceModel.CommunicationState.Opened)
                    {
                        exceptionOneWayClient.Open();
                    }

                    exceptionOneWayClient.SubmitFault(proxyFaultMessage);
                }
                catch (Exception ex)
                {
                    EventLogUtility.LogException(ex);
                    SetMessagingAsException(ar, ex);
                }
                finally
                {
                    if ((exceptionOneWayClient != null)
                        && (exceptionOneWayClient.State == System.ServiceModel.CommunicationState.Opened))
                    {
                        exceptionOneWayClient.Close();
                    }
                }
            }

            if (!ar.IsCompleted)    // this would only show completed after an exception
                SetMessagingAsComplete(ar);
        }

        public override MessageBase EndSubmitMessage(IAsyncResult ar)
        {
            return ((AsyncResult<FaultMessage>)ar).EndInvoke();
        }

        protected void OnMessagingComplete(IAsyncResult ar)
        {
            // Empty implementation
            // For cases where notification of MessagingComplete is not needed

            try
            {
                Open.MOF.Messaging.FaultMessage faultMessage = (Open.MOF.Messaging.FaultMessage)EndSubmitMessage(ar);
                Console.WriteLine("Fault send complete.\n");
                EventLogUtility.LogException(faultMessage.ExceptionDetail);
            }
            catch (Exception ex)
            {
                EventLogUtility.LogException(ex);
            }
        }

        protected void SetMessagingAsComplete(object state)
        {
            // this method is for asynchronous purposes only
            if (state is IAsyncResult)
                SetMessagingAsComplete((IAsyncResult)state);
        }

        protected void SetMessagingAsComplete(IAsyncResult ar)
        {
            FaultMessage faultMessage = null;
            if ((ar.AsyncState != null) && (ar.AsyncState is FaultMessage))
            {
                faultMessage = (Open.MOF.Messaging.FaultMessage)ar.AsyncState;
            }

            ((AsyncResult<Open.MOF.Messaging.FaultMessage>)ar).SetAsCompleted(faultMessage, false);
        }

        protected void SetMessagingAsException(IAsyncResult ar, Exception ex)
        {
            ((AsyncResult<Open.MOF.Messaging.FaultMessage>)ar).SetAsCompleted(ex, false);
        }

        //public static IAsyncResult EmptyResult(AsyncCallback messagingCompleteCallback)
        //{
        //    AsyncResult<FaultMessage> ar = new AsyncResult<FaultMessage>(messagingCompleteCallback, null);
        //    ThreadPool.QueueUserWorkItem(SetMessagingAsComplete, (object)ar);
        //    return ar;
        //}

        public override Open.MOF.Messaging.Services.ServiceInterfaceType SuportedServiceInterfaces
        {
            get { return (Open.MOF.Messaging.Services.ServiceInterfaceType.ExceptionService); }
        }
        
        public override void Dispose()
        {
        }
    }
}
