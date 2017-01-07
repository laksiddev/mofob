using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;

namespace Open.MOF.Messaging.ExceptionHandling
{
    public class ExceptionHandlerAssembler : IAssembler<IExceptionHandler, Open.MOF.Messaging.ExceptionHandling.ExceptionHandlerData>
    {
        public IExceptionHandler Assemble(IBuilderContext context, ExceptionHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            Open.MOF.Messaging.ExceptionHandling.ExceptionHandlerData castedObjectConfiguration = 
                (Open.MOF.Messaging.ExceptionHandling.ExceptionHandlerData)objectConfiguration;

            Dictionary<string, object> values = new Dictionary<string, object>();
            values.Add("ServiceName", castedObjectConfiguration.ServiceName);
            values.Add("ApplicationName", castedObjectConfiguration.ApplicationName);

            ExceptionHandler createdObject = new ExceptionHandler(values);

            return createdObject;
        }
    }
}
