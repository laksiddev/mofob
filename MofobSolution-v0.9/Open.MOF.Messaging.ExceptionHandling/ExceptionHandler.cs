using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

using Open.MOF.Messaging;
using Open.MOF.Messaging.Adapters;
using System.Collections.Specialized;

namespace Open.MOF.Messaging.ExceptionHandling
{
    [ConfigurationElementType(typeof(ExceptionHandlerData))]
    public class ExceptionHandler : IExceptionHandler
    {
        // I believe they use the property name, not the stored value name.
        private const string __serviceNameProperty = "ServiceName"; // "serviceName";
        private const string __applicationNameProperty = "ApplicationName"; //"applicationName";

        private string _serviceName;
        private string _applicationName;

        public ExceptionHandler(NameValueCollection values)
        {
            _serviceName = (string)values[__serviceNameProperty] ?? "Unconfigured Service";
            _applicationName = (string)values[__applicationNameProperty] ?? "Unconfigured Application";
        }

        public ExceptionHandler(string serviceName, string applicationName)
        {
            _serviceName = serviceName;
            _applicationName = applicationName;
        }

        public Exception HandleException(Exception exception, Guid exceptionInstanceId)
        {
            FaultMessage fault = new FaultMessage(exceptionInstanceId, _serviceName, _applicationName, exception);
            using (IMessagingAdapter adapter = MessagingAdapter.CreateInstance(fault))
            {
                adapter.BeginSubmitMessage(fault, null, null);
            }

            return exception;
        }
    }

}
