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
        private string _path = null;

        public AdvancedXmlParser()
        {
            ParsedData = new Dictionary<string, List<Tag>>();
        }
        /// <summary>
        /// Reads required paths from Config instance and parses XDocument elements from each path
        /// </summary>
        public override void ParseData()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                base.ParseDocument();

                List<Tag> tags;
                LogMessage("retireving data from Config");
                foreach (var path in ParserConfig.Paths)
                {
                    _path = path;
                    LogMessage("retireving path from config paths");
                    tags = getTagsListFromPath();
                    checkIfTagsExistAndInsert(tags);
                }
            }
        }


        /// <summary>
        /// Check if there are elements present in given list and act accordingly.
        /// </summary>
        /// <param name="tags">List of tags.</param>
        private void checkIfTagsExistAndInsert(List<Tag> tags)
        {
            if (tags.Count != 0)
                ParsedData[tags.First().TagName] = tags;
            else
                LogError("No tags found for provided path: " + _path);
        }

        /// <summary>
        /// Finds ALL XElements, that are found on a given path.
        /// </summary>
        /// <returns></returns>
        private List<Tag> getTagsListFromPath()
        {
            List<Tag> tags = new List<Tag>();
            var elements = Document.XPathSelectElements(_path);
            if (elements.Count() == 0)
            {
                LogWarning("No elements founds for given path: " + _path);
                return tags;
            }
            foreach (var element in elements)
                addTagFromElement(tags, element);

            return tags;
        }

        /// <summary>
        /// Adds a Tag instance, made of XElement, to tags list.
        /// </summary>
        /// <param name="tags">Tags list</param>
        /// <param name="element">Is used to create a Tag instance</param>
        private void addTagFromElement(List<Tag> tags, XElement element)
        {
            Tag t = new Tag();
            t.SetTagFromElement(element);

            t.SetTagChildren(t, element);

            tags.Add(t);
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
