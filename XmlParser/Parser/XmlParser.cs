using System;
using System.Collections.Generic;
using System.Xml.Linq;
using MetronikParser.Logger;
using MetronikParser.Helpers;

namespace MetronikParser.Parser
{
    public abstract class XmlParser : IParser
    {
        public ILogger Log { get; set; }

        public XDocument Document { get; set; }

        public Config ParserConfig { get; set; }

        public Dictionary<string, List<Tag>> ParsedData { get; protected set; }

        public abstract void ParseData(LoggerType type);

        public void ParseDocument(LoggerType type)
        {
            using (Log = LoggerFactory.GetLogger(type))
            {
                if (Document == null)
                    throw new ArgumentNullException();

                LogMessage("Starting parsing document");
                // no need to catch exception, XDocument.Load() method already checks for illegal chars / wrong tag closings etc.
                Document = XDocument.Parse(Document.ToString());
            }
        }

        public abstract void LogMessage(string message, params object[] args);
        public abstract void LogWarning(string message, params object[] args);
        public abstract void LogError(string message, params object[] args);
    }
}
