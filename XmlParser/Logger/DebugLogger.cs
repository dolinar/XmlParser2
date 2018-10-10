using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace XmlParser.Logger
{
    class DebugLogger : ILogger
    {
        StringBuilder logMessage = new StringBuilder();
        StringBuilder logWarning = new StringBuilder();
        StringBuilder logError = new StringBuilder();
        public void Dispose()
        {
            Flash();
        }

        public void Flash()
        {
            logMessage.Clear();
            logWarning.Clear();
            logError.Clear();
        }



        public void LogMessage(string message, params object[] args)
        {
            string formatedMessage = string.Format(message, args);
            logMessage.AppendLine(message);
            Debug.WriteLine(formatedMessage);
        }

        public void LogWarning(string message, params object[] args)
        {
            string formatedMessage = string.Format(message, args);
            logWarning.AppendLine(message);
            Debug.WriteLine(formatedMessage);
        }

        public void LogError(string message, params object[] args)
        {
            string formatedMessage = string.Format(message, args);
            logMessage.AppendLine(message);
            Debug.WriteLine(formatedMessage);
        }


        public string GetCurrentMessageLog()
        {
            return logMessage.ToString();
        }

        public string GetCurrentWarningLog()
        {
            return logWarning.ToString();
        }

        public string GetCurrentErrorLog()
        {
            return logError.ToString();
        }
    }
}
