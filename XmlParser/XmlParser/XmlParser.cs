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
        
        public string FilePath { get; set; }

        public void SetDocument()
        {
            using (Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                // generate XDocument with ReadXml class. 
                // path is provided in the Program.cs and is passed to this method via ParserBuilder and abstract factory.
                FileObject<XDocument> readXml = new ReadXml { StringPath = FilePath };
                LogMessage("Reading XML file");
                Document = readXml.ReadFile();
            }
        }

        public void ParseDocument()
        {
            using (Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                // make sure the generated XML document is not null
                if (Document == null)
                    throw new ArgumentNullException();

                LogMessage("Starting parsing document");

                // no need to catch exception, XDocument.Load() method already checks for illegal chars / wrong tag closing etc.
                Document = XDocument.Parse(Document.Root.ToString());

            }
        }

        public abstract void DoSomething(string filePath);

        protected abstract void LogMessage(string message, params object[] args);
        protected abstract void LogWarning(string message, params object[] args);
        protected abstract void LogError(string message, params object[] args);
        
    }
}
