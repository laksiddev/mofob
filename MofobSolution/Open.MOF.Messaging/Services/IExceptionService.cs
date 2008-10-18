using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.Messaging.Services
{
    public interface IExceptionService
    {
        void SubmitException(System.Exception faultException, Guid execeptionInstanceId, string serviceName, string applicationName);
    }
}
