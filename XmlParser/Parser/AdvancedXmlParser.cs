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

        public AdvancedXmlParser()
        {
            ParsedData = new List<Tag>();
        }
        /// <summary>
        /// Reads required paths from Config instance and parses XDocument elements from each path
        /// </summary>
        public override void ParseData()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                base.ParseDocument();

                LogMessage("retireving data from Config");
                foreach (var path in ParserConfig.Paths)
                {
                    LogMessage("retireving data from config paths");
                    ParsedData.AddRange(getTagsFromPath(path));
                }
            }
        }

        /// <summary>
        /// Finds ALL XElements, that are found on a given path.
        /// </summary>
        /// <param name="path">Absolute path to certain XElement(s). Delimiter: "/"</param>
        /// <returns></returns>
        private List<Tag> getTagsFromPath(string path)
        {
            List<Tag> tags = new List<Tag>();
            var elements = Document.XPathSelectElements(path);
            if (elements.Count() == 0)
            {
                LogWarning("No elements founds for given path: " + path);
                return tags;
            }
            foreach (var element in elements)
                addSubtags(tags, element);

            return tags;
        }

        /// <summary>
        /// Adds a Tag instance, made of XElement, to tags list.
        /// </summary>
        /// <param name="tags">Tags list</param>
        /// <param name="element">Is used to create a Tag instance</param>
        private void addSubtags(List<Tag> tags, XElement element)
        {
            Tag t = new Tag();
            t.SetTagFromElement(element);

            if (ParserConfig.RequireChildren)
                setTagChildren(t, element);

            tags.Add(t);
        }

        /// <summary>
        /// Add element(tag) children to tha parent element(tag)'s Children property
        /// The class Tag's AddChild() method also calls function SetTagElement which sets all properties 
        /// </summary>
        /// <param name="rootTag">Tag instance, made of element's properties</param>
        /// <param name="element">Is used to find descendants</param>
        private void setTagChildren(Tag rootTag, XElement element)
        {
            foreach (XElement childElement in element.Elements())
            {
                Tag childTag = rootTag.AddChild(childElement);
                setTagChildren(childTag, childElement);
            }
        }

        /// <summary>
        /// Concrete implementation of logging methods
        /// </summary>
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
