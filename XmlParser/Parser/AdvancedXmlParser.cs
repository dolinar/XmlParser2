using MetronikParser.Logger;
using MetronikParser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.XPath;
using System.Xml.Linq;

namespace MetronikParser.Parser
{
    public class AdvancedXmlParser : XmlParser
    {     

        public AdvancedXmlParser()
        {
            ParsedData = new Dictionary<string, List<Tag>>();
        }
        /// <summary>
        /// Reads paths from Config instance and parses XDocument elements from each path
        /// </summary>
        public override void ParseData(LoggerType type)
        {
            using (Log = LoggerFactory.GetLogger(type))
            {
                base.ParseDocument(type);

                List<Tag> tags;
                LogMessage("retireving data from Config");               
                foreach (var path in ParserConfig.Paths)
                {                   
                    LogMessage("retireving path from config paths");
                    tags = getTagsListFromPath(path);
                    checkIfTagsExistAndInsert(tags,path);
                }
            }
        }


        /// <summary>
        /// Check if there are elements present in the given Tag list, if they are, add them to main data structure.
        /// </summary>
        /// <param name="tags">List of tags.</param>
        private void checkIfTagsExistAndInsert(List<Tag> tags,string path)
        {          
            string tagName = tags?.FirstOrDefault()?.TagName;

            if(!String.IsNullOrEmpty(tagName))
                ParsedData[tagName] = tags;

            LogError($"No tags({tagName}) found for provided path: {path}.");          
        }

        /// <summary>
        /// Finds all XElements objects that are found for a given path, 
        /// makes instance of Tag for each element and adds it to List
        /// </summary>
        /// <returns>List of Tags that were found for a given path</returns>
        private List<Tag> getTagsListFromPath(string path)
        {
            List<Tag> tags = new List<Tag>();
            var elements = Document.XPathSelectElements(path);
            if (elements.Count() == 0)
            {
                LogWarning($"No elements founds for given path: {path}");
                return tags;
            }
            foreach (var element in elements)
                addTagToListFromElement(tags, element);

            return tags;
        }

        /// <summary>
        /// Populates Tag instance with values from XElement and adds it to provided tags list.
        /// </summary>
        /// <param name="tags">List for newly created tag to append to.</param>
        /// <param name="element">Is used to create a Tag instance</param>
        private void addTagToListFromElement(List<Tag> tags, XElement element)
        {
            Tag t = new Tag();
            t.SetTagFromElement(element);

            t.SetChildren(t, element); // recursive

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
