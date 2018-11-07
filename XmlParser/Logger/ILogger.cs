using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronikParser.Logger
{
    /// <summary>
    /// Enum LogSeverity enumerates severity types. 
    /// MESSAGE = 1, WARNING = 2, ERROR = 3
    /// </summary>
    public enum LogSeverity
    {
        MESSAGE = 1,
        WARNING = 2,
        ERROR   = 3
    }

    /// <summary>
    /// Logging interface - if another type of logger is needed (DB logger, EventLogger), extend this interface
    /// and implement it. Add appropriate case to switch function in LoggerFactory.
    /// </summary>
    public interface ILogger : IDisposable
    {
        /// <summary>
        /// Basic methods for logging, seperated by their severity type
        /// </summary>
        /// <param name="Message">Log message</param>
        /// <param name="args">Not required</param>
        void LogMessage(string Message, params object[] args);
        void LogWarning(string Message, params object[] args);
        void LogError(string Message, params object[] args);

        /// <summary>
        /// Provides a mechanism for releasing unmanaged logging resources.
        /// </summary>
        void Flash();
        string GetCurrentMessageLog();
    }
}
