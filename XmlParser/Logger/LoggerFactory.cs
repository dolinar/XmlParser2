using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XmlParser.Logger
{
    public enum LoggerType
    {
        DEBUG
    }

    public class LoggerFactory
    {
        public static ILogger Get(LoggerType type)
        {
            switch (type)
            {
                case LoggerType.DEBUG:
                    return new DebugLogger();
                default:
                    return new DebugLogger();
            }
        }
    }
}
