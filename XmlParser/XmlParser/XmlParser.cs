using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using XmlParser.Logger;

namespace XmlParser
{
    public abstract class XmlParser : IParser
    {
        public ILogger Log { get; set; }
        public XDocument Document { get; set; }

        public List<KeyValuePair<string, string>> ParsedData { get; set; }
        public abstract void ParseDocument(string filePath);

        protected abstract void LogMessage(string message, params object[] args);
        protected abstract void LogWarning(string message, params object[] args);
        protected abstract void LogError(string message, params object[] args);
        
    }
}
