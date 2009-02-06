using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Open.MOF.Messaging
{
    [System.ComponentModel.TypeConverter(typeof(ExceptionDetailConverter))]
    [DataContract(Name = "ExceptionDetails", Namespace = "http://mof.open/Messaging/DataContracts/1/0/")]
    public class ExceptionDetail
    {
        public ExceptionDetail()
        {
            _message = null;
            _exceptionType = null;
            _source = null;
            _targetSite = null;
            _stackTrace = null;
            _innerDetail = null;
        }

        public ExceptionDetail(string message, string exceptionType, string source, string targetSite, string stackTrace, ExceptionDetail innerDetail)
        {
            _message = message;
            _exceptionType = exceptionType;
            _source = source;
            _targetSite = targetSite;
            _stackTrace = stackTrace;
            _innerDetail = innerDetail;
        }

        [DataMember(Name="message", Order = 1)]
        private string _message;
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

        [DataMember(Name = "exceptionType", Order = 2)]
        private string _exceptionType;
        public string ExceptionType
        {
            get { return _exceptionType; }
            set { _exceptionType = value; }
        }

        [DataMember(Name = "source", Order = 3)]
        private string _source;
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        [DataMember(Name = "targetSite", Order = 4)]
        private string _targetSite;
        public string TargetSite
        {
            get { return _targetSite; }
            set { _targetSite = value; }
        }

        [DataMember(Name = "stackTrace", Order = 5)]
        private string _stackTrace;
        public string StackTrace
        {
            get { return _stackTrace; }
            set { _stackTrace = value; }
        }

        [DataMember(Name = "InnerDetail", Order = 6)]
        private ExceptionDetail _innerDetail;
        public ExceptionDetail InnerDetail
        {
            get { return _innerDetail; }
            set { _innerDetail = value; }
        }
    }
}
