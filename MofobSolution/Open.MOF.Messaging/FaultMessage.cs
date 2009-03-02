using System;
using System.Collections.Generic;
using System.ServiceModel;
using System.Text;

namespace Open.MOF.Messaging
{
    [MessageContract(IsWrapped = true, WrapperName = "FaultMessage", WrapperNamespace = "http://mof.open/Messaging/ServiceContracts/1/0/")]
    public class FaultMessage : FrameworkMessage
    {
        public FaultMessage() : base()
        {
            _execeptionInstanceId = null;
            _serviceName = null;
            _applicationName = null;
            _exceptionDetail = null;
        }

        public FaultMessage(Guid execeptionInstanceId, string serviceName, string applicationName, Exception ex) 
        {
            _execeptionInstanceId = (Guid?)execeptionInstanceId;
            _serviceName = serviceName;
            _applicationName = applicationName;
            SetExceptionDetail(ex);
        }
        
        public FaultMessage(Guid execeptionInstanceId, string serviceName, string applicationName, ExceptionDetail exceptionDetail)
        {
            _execeptionInstanceId = (Guid?)execeptionInstanceId;
            _serviceName = serviceName;
            _applicationName = applicationName;
            _exceptionDetail = exceptionDetail;
        }

        [MessageBodyMember(Name = "execeptionInstanceId", Order = 1, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        private Guid? _execeptionInstanceId;
        public Guid? ExceptionInstanceId
        {
            get { return _execeptionInstanceId; }
            set { _execeptionInstanceId = value; }
        }

        [MessageBodyMember(Name = "serviceName", Order = 2, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        private string _serviceName;
        public string ServiceName
        {
            get { return _serviceName; }
            set { _serviceName = value; }
        }

        [MessageBodyMember(Name = "applicationName", Order = 3, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        private string _applicationName;
        public string ApplicationName
        {
            get { return _applicationName; }
            set { _applicationName = value; }
        }

        [MessageBodyMember(Name = "ExceptionDetail", Order = 4, Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
        private ExceptionDetail _exceptionDetail;
        public ExceptionDetail ExceptionDetail
        {
            get { return _exceptionDetail; }
            set { _exceptionDetail = value; }
        }

        public void SetExceptionDetail(Exception ex)
        {
            System.ComponentModel.TypeConverter converter = System.ComponentModel.TypeDescriptor.GetConverter(typeof(ExceptionDetail));
            _exceptionDetail = (ExceptionDetail)converter.ConvertFrom(ex);
        }

        public static MessageBehavior Behavior
        {
            get
            {
                return MessageBehavior.FaultReporting;
            }
        }
    }
}
