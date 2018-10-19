using MetronikParser.Logger;
using MetronikParser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;
using System.Xml.Linq;

namespace MetronikParser.Parser
{
    public class AdvancedXmlParser : XmlParser
    {
        private Tag _tag = null;

        public AdvancedXmlParser()
        {
            ParsedData = new List<Tag>();
        }
        public override void ParseData()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                base.ParseDocument();

                LogMessage("retireving data from Config");
                foreach (var path in ParserConfig.Paths)
                {
                    List<Tag> tags = getTagsFromPath(path);
                    foreach (var tag in tags)
                    {
                        ParsedData.Add(tag);
                    }
                }
            }
        }

        private List<Tag> getTagsFromPath(string path)
        {
            List<Tag> tags = new List<Tag>();
            var elements = Document.XPathSelectElements(path);
            if (elements.Count() == 0)
                throw new NullReferenceException();

            Tag t;
            foreach (var element in elements)
            {
                t = new Tag();
                t.TagName = element.Name.LocalName;
                t.TagValue = element.Value;
                t.Attributes = new Dictionary<string, string>();
                foreach (XAttribute attribute in element.Attributes())
                {
                    t.Attributes.Add(attribute.Name.LocalName, attribute.Value);
                }
                tags.Add(t);
            }

            //path += "/" + element.Name.LocalName;

            return tags;
        }

        public override void LogMessage(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogMessage(String.Format("MESSAGE FROM {0}: {1}", this.GetType().ToString(), message));
        }

        public override void LogWarning(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogWarning(String.Format("WARNING FROM {0}: {1}", this.GetType().ToString(), message));
        }

        public override void LogError(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogError(String.Format("ERROR FROM {0}: {1}", this.GetType().ToString(), message));
        }


    }
}
