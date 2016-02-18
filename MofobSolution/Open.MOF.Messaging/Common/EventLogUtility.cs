using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace Open.MOF.Messaging
{
    public class EventLogUtility
    {
        private const string _constEventLogSource = "Message Oriented Framework";
        private const int _constEventLogId = 1001;

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
            System.Diagnostics.EventLog.WriteEntry(_constEventLogSource, message, EventLogEntryType.Information, _constEventLogId);
        }

        public static void LogWarningMessage(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(_constEventLogSource, message, EventLogEntryType.Warning, _constEventLogId);
        }

        public static void LogErrorMessage(string message)
        {
            System.Diagnostics.EventLog.WriteEntry(_constEventLogSource, message, EventLogEntryType.Error, _constEventLogId);
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

            if (ex.InnerException != null)
            {
                sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
                sbExceptionMessage.Append(ex.InnerException.Message);
                sbExceptionMessage.Append("\r\n");
                sbExceptionMessage.Append(ex.InnerException.Source);
                sbExceptionMessage.Append("\r\n");
                sbExceptionMessage.Append(ex.InnerException.StackTrace);

                if (ex.InnerException.InnerException != null)
                {
                    sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
                    sbExceptionMessage.Append(ex.InnerException.InnerException.Message);
                    sbExceptionMessage.Append("\r\n");
                    sbExceptionMessage.Append(ex.InnerException.InnerException.Source);
                    sbExceptionMessage.Append("\r\n");
                    sbExceptionMessage.Append(ex.InnerException.InnerException.StackTrace);
                }
            }

            return sbExceptionMessage.ToString();
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

            if (exceptionDetail.InnerDetail != null)
            {
                sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
                sbExceptionMessage.Append(exceptionDetail.InnerDetail.Message);
                sbExceptionMessage.Append("\r\n");
                sbExceptionMessage.Append(exceptionDetail.InnerDetail.Source);
                sbExceptionMessage.Append("\r\n");
                sbExceptionMessage.Append(exceptionDetail.InnerDetail.StackTrace);

                if (exceptionDetail.InnerDetail.InnerDetail != null)
                {
                    sbExceptionMessage.Append("\r\n\r\nAdditional Exception details:\r\n\r\n");
                    sbExceptionMessage.Append(exceptionDetail.InnerDetail.InnerDetail.Message);
                    sbExceptionMessage.Append("\r\n");
                    sbExceptionMessage.Append(exceptionDetail.InnerDetail.InnerDetail.Source);
                    sbExceptionMessage.Append("\r\n");
                    sbExceptionMessage.Append(exceptionDetail.InnerDetail.InnerDetail.StackTrace);
                }
            }

            return sbExceptionMessage.ToString();
        }
    }
}
