using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;

namespace Open.MOF.Messaging.ExceptionHandling
{
    [ConfigurationElementType(typeof(ExceptionHandlerData))]
    public class ExceptionHandler : IExceptionHandler
    {
        Dictionary<string, object> _values;

        public ExceptionHandler(Dictionary<string, object> values)
        {
            _values = values;
        }

        public Exception HandleException(Exception exception, Guid exceptionInstanceId)
        {
            string serviceName = (string)_values["ServiceName"];
            string applicationName = (string)_values["ApplicationName"];

            FaultMessage fault = new FaultMessage(exceptionInstanceId, serviceName, applicationName, exception);
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(fault))
            {
                adapter.BeginSubmitMessage(fault, null, null);
            }

            return exception;
        }
    }

}
