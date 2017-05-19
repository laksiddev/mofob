using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;

namespace Open.MOF.Messaging.ExceptionHandling
{
    public class ExceptionHandlerData : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData
    {
        private const string __serviceNameProperty = "serviceName";
        private const string __applicationNameProperty = "applicationName";

        public ExceptionHandlerData()
        {
        }

        public ExceptionHandlerData(string name, string serviceName, string applicationName)
            : base(name, typeof(ExceptionHandler))
        {
            ServiceName = serviceName;
            ApplicationName = applicationName;
        }

        [ConfigurationProperty(__serviceNameProperty, IsRequired = false, DefaultValue = "")]
        public string ServiceName
        {
            get { return (string)this[__serviceNameProperty]; }
            set { this[__serviceNameProperty] = value; }
        }

        [ConfigurationProperty(__applicationNameProperty, IsRequired = false, DefaultValue = "")]
        public string ApplicationName
        {
            get { return (string)this[__applicationNameProperty]; }
            set { this[__applicationNameProperty] = value; }
        }

        public override IExceptionHandler BuildExceptionHandler()
        {
            return new ExceptionHandler(ServiceName, ApplicationName);
        }
    }
}
