using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Adapters
{
    public abstract class ExceptionAdapter : MessagingAdapter
    {
        protected ExceptionAdapter(string bindingName)  : base(bindingName) 
        {
        }

        public void SubmitException(Exception faultException, Guid execeptionInstanceId, string serviceName, string applicationName)
        {
            FaultMessage faultMessage = new FaultMessage(execeptionInstanceId, serviceName, applicationName, faultException);
            base.SubmitMessage(faultMessage);
        }
    }
}
