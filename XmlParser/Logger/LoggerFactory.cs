using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronikParser.Logger
{
    public enum LoggerType
    {
        DEBUG,
        CONSOLE
    }

    public class LoggerFactory
    {
        /// <summary>
        /// get logger by provided enum type
        /// </summary>
        /// <param name="type">element of LoggerType enum</param>
        /// <returns>instance of a logger</returns>
        public static ILogger GetLogger(LoggerType type)
        {
            switch (type)
            {
                case LoggerType.DEBUG:
                    return new DebugLogger();
                case LoggerType.CONSOLE:
                    return new ConsoleLogger();
                default:
                    throw new InvalidEnumArgumentException("type", (int)type, typeof(LoggerType));
            }
        }

        /// <summary>
        /// get logger by provided string
        /// </summary>
        /// <param name="loggerName">string logger name, ie 'debug'</param>
        /// <returns>instance of a logger</returns>
        public static ILogger GetLogger(string loggerName)
        {
            switch (loggerName.ToLower())
            {
                case "debug":
                    return new DebugLogger();
                default:
                    throw new ArgumentException();
            }
        }
    }
}
