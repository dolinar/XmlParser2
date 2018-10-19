using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronikParser.Logger
{
    class ConsoleLogger : ILogger
    {
        StringBuilder _log = new StringBuilder();
        public void Dispose()
        {
            Flash();
        }

        public void Flash()
        {
            _log.Clear();
        }

        public void LogMessage(string message, params object[] args)
        {
            log(LogSeverity.MESSAGE, message, args);
        }

        public void LogWarning(string message, params object[] args)
        {
            log(LogSeverity.WARNING, message, args);
        }

        public void LogError(string message, params object[] args)
        {
            log(LogSeverity.ERROR, message, args);
        }

        private void log(LogSeverity severity, string message, params object[] args)
        {
            string severityString = GetSeverityString(severity);
            string formatedMessage = string.Format(message, args);
            _log.AppendLine($"{severityString}({DateTime.Now}):{formatedMessage}");
            Console.Write(formatedMessage);
        }

        private static string GetSeverityString(LogSeverity severity)
        {
            string severityString;
            switch (severity)
            {
                case LogSeverity.MESSAGE:
                    severityString = "MESSAGE";
                    break;
                case LogSeverity.WARNING:
                    severityString = "WARNING";
                    break;
                case LogSeverity.ERROR:
                    severityString = "ERROR";
                    break;
                default:
                    throw new ArgumentException($"unknown severity {severity.ToString()}");
            }
            return severityString;
        }

        public string GetCurrentMessageLog()
        {
            return _log.ToString();
        }
    }
}
