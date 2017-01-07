using System;
using System.Collections.Generic;
using System.Text;
using System.Configuration;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Open.MOF.Messaging.ExceptionHandling
{
    [Assembler(typeof(ExceptionHandlerAssembler))]
    public class ExceptionHandlerData : Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.ExceptionHandlerData
    {
        private const string serviceNameProperty = "serviceName";
        private const string applicationNameProperty = "applicationName";

        public ExceptionHandlerData()
        {
        }

        public ExceptionHandlerData(string name, string serviceName, string applicationName)
            : base(name, typeof(ExceptionHandler))
        {
            ServiceName = serviceName;
            ApplicationName = applicationName;
        }

        [ConfigurationProperty(serviceNameProperty, IsRequired = false, DefaultValue = "")]
        public string ServiceName
        {
            get { return (string)this[serviceNameProperty]; }
            set { this[serviceNameProperty] = value; }
        }

        [ConfigurationProperty(applicationNameProperty, IsRequired = false, DefaultValue = "")]
        public string ApplicationName
        {
            get { return (string)this[applicationNameProperty]; }
            set { this[applicationNameProperty] = value; }
        }
    }
}
