using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Text;

namespace Open.MOF.Messaging
{
    public class EventLogUtility
    {
        private const string __eventLogAppSettingKey = "ApplicationEventLogSource";
        private const string __defaultEventLogSource = "Message Oriented Framework";
        private const int __constEventLogId = 1001;

        private static string _eventLogSource = null;
        public static string EventLogSource
        {
            get
            {
                if (String.IsNullOrEmpty(_eventLogSource))
                {
                    // First use general AppSettings value that is available to all apps
                    _eventLogSource = ConfigurationManager.AppSettings[__eventLogAppSettingKey];

                    // Then use a default value
                    if (String.IsNullOrEmpty(_eventLogSource))
                        _eventLogSource = __defaultEventLogSource;
                }

                return _eventLogSource;
            }
        }

        public static void LogException(System.Exception ex)
        {
            LogErrorMessage(FormatExceptionMessage(ex));
        }

        public static void LogException(ExceptionDetail exceptionDetail)
        {
            LogErrorMessage(FormatExceptionMessage(exceptionDetail));
        }

        public static void LogInformationMessage(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Information, __constEventLogId);
        }

        public static void LogWarningMessage(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Warning, __constEventLogId);
        }

        public static void LogErrorMessage(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(EventLogSource, message, EventLogEntryType.Error, __constEventLogId);
        }

        public static string FormatExceptionMessage(System.Exception ex)
        {
            StringBuilder sbExceptionMessage = new StringBuilder();

            sbExceptionMessage.Append("An Exception has been raised:\r\n\r\n");
            sbExceptionMessage.Append("");
            sbExceptionMessage.Append(ex.Message);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(ex.Source);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(ex.StackTrace);

            FormatInnerExceptionMessage(ex.InnerException, sbExceptionMessage);

            return sbExceptionMessage.ToString();
        }

        private static void FormatInnerExceptionMessage(System.Exception ex, StringBuilder sbExceptionMessage)
        {
            if (ex == null)
                return;

            sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
            sbExceptionMessage.Append(ex.Message);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(ex.Source);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(ex.StackTrace);

            FormatInnerExceptionMessage(ex.InnerException, sbExceptionMessage);
        }

        private static string FormatExceptionMessage(ExceptionDetail exceptionDetail)
        {
            StringBuilder sbExceptionMessage = new StringBuilder();

            sbExceptionMessage.Append("An Exception has been raised:\r\n\r\n");
            sbExceptionMessage.Append("");
            sbExceptionMessage.Append(exceptionDetail.Message);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(exceptionDetail.Source);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(exceptionDetail.StackTrace);

            FormatInnerExceptionMessage(exceptionDetail.InnerDetail, sbExceptionMessage);

            return sbExceptionMessage.ToString();
        }

        private static void FormatInnerExceptionMessage(ExceptionDetail exceptionDetail, StringBuilder sbExceptionMessage)
        {
            if (exceptionDetail == null)
                return;

            sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
            sbExceptionMessage.Append(exceptionDetail.InnerDetail.Message);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(exceptionDetail.InnerDetail.Source);
            sbExceptionMessage.Append("\r\n");
            sbExceptionMessage.Append(exceptionDetail.InnerDetail.StackTrace);

            FormatInnerExceptionMessage(exceptionDetail.InnerDetail, sbExceptionMessage);
        }
    }
}
