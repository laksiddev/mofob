using System;
using System.Collections.Generic;
using System.Text;

namespace Open.MOF.BizTalk.Services
{
    public class FaultMessageConverter : System.ComponentModel.TypeConverter
    {
        public override bool CanConvertFrom(System.ComponentModel.ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(Open.MOF.Messaging.FaultMessage))
                return true;

            return base.CanConvertFrom(context, sourceType);
        }

        public override object ConvertFrom(System.ComponentModel.ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (value.GetType() == typeof(Open.MOF.Messaging.FaultMessage))
            {
                Open.MOF.Messaging.FaultMessage localFaultMessage = (Open.MOF.Messaging.FaultMessage)value;

                Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessage proxyFaultMessage = new Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessage();

                // Fault Message Header
                proxyFaultMessage.Header = new Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessageHeader();
                proxyFaultMessage.Header.FaultGenerator = "ESBExceptionService";
                proxyFaultMessage.Header.FaultCode = "0";
                proxyFaultMessage.Header.ErrorType = localFaultMessage.ExceptionDetail.ExceptionType;
                proxyFaultMessage.Header.FailureCategory = "Exception";
                proxyFaultMessage.Header.DateTime = DateTime.Now.ToUniversalTime().ToString("yyyy-MM-dd HH:mm:ss");
                proxyFaultMessage.Header.Application = localFaultMessage.ApplicationName;
                proxyFaultMessage.Header.Description = localFaultMessage.ExceptionDetail.Message;
                proxyFaultMessage.Header.FaultDescription = localFaultMessage.ExceptionDetail.Message;
                proxyFaultMessage.Header.MachineName = System.Net.Dns.GetHostName();
                proxyFaultMessage.Header.ServiceName = localFaultMessage.ServiceName;
                proxyFaultMessage.Header.ServiceInstanceID = localFaultMessage.ExceptionInstanceId.ToString();
                proxyFaultMessage.Header.MessageID = localFaultMessage.ExceptionInstanceId.ToString();
                proxyFaultMessage.Header.ActivityIdentity = "";
                proxyFaultMessage.Header.Scope = "";

                // Fault Message Exception Object
                proxyFaultMessage.ExceptionObject = new Open.MOF.BizTalk.Services.Proxy.EsbExceptionInstance.FaultMessageExceptionObject();
                proxyFaultMessage.ExceptionObject.Type = localFaultMessage.ExceptionDetail.ExceptionType;
                proxyFaultMessage.ExceptionObject.Source = localFaultMessage.ExceptionDetail.Source;
                proxyFaultMessage.ExceptionObject.TargetSite = localFaultMessage.ExceptionDetail.TargetSite;
                proxyFaultMessage.ExceptionObject.StackTrace = localFaultMessage.ExceptionDetail.StackTrace;
                proxyFaultMessage.ExceptionObject.Message = localFaultMessage.ExceptionDetail.Message;
                proxyFaultMessage.ExceptionObject.InnerExceptionMessage = ((localFaultMessage.ExceptionDetail.InnerDetail != null) ? localFaultMessage.ExceptionDetail.InnerDetail.Message : String.Empty);

                proxyFaultMessage.Messages = null;

                return proxyFaultMessage;
            }

            return base.ConvertFrom(context, culture, value);
        }
    }
}
