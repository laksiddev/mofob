using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public abstract class ExceptionService : MessagingService
    {
        protected ExceptionService(string serviceBindingName)  : base(serviceBindingName) 
        {
        }

        public void SubmitException(Exception faultException, Guid execeptionInstanceId, string serviceName, string applicationName)
        {
            FaultMessage faultMessage = new FaultMessage(execeptionInstanceId, serviceName, applicationName, faultException);
            base.SubmitMessage(faultMessage);
        }
    }
}
