using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using MetronikParser.Logger;
using MetronikParser.Helpers;

namespace MetronikParser.Parser
{
    public class SimpleXmlParser : XmlParser
    {

        /// <summary>
        /// Get all leaf tags, with no need of providing Config instance.
        /// </summary>
        public override void ParseData(LoggerType type)
        {
            using (Log = LoggerFactory.GetLogger(type))
            {
                base.ParseDocument(type);
                ParsedData = getTags();
            }
        }

        private Dictionary<string, List<Tag>> getTags()
        {
            Dictionary<string, List<Tag>> tags = new Dictionary<string, List<Tag>>();

            List<XElement> listOfNodes = GetListOfNodes(Document.Root);
            LogMessage("Finished finding leaf nodes.");

            foreach (XElement node in listOfNodes)
            {
                Tag tag = new Tag();
                tag.SetTagFromElement(node);

                string name = tag.TagName;
                if (tags.ContainsKey(name))
                    tags[name].Add(tag);
                else
                    tags.Add(name, new List<Tag> { tag });
            }

            LogMessage($"Finished finding node paths. Tags count: {tags.Count} Nodes count: {listOfNodes.Count}");
            return tags;
        }

        /// <summary>
        /// Get list of all leaf nodes (which hold values we are looking for) of a given root element.
        /// </summary>
        /// <param name="elements">Root element of type XElement</param>
        /// <returns>List of all leaf nodes.</returns>
        private List<XElement> GetListOfNodes(XElement element)
        {
            List<XElement> listOfNodes = new List<XElement>();
            NodeTypes nodeType;
            foreach (XElement childElement in element.Elements())
            {
                nodeType = childElement.Descendants().Count() > 0 ? NodeTypes.HasChildren : NodeTypes.IsLeaf;

                switch (nodeType)
                {
                    case NodeTypes.IsLeaf:
                        listOfNodes.Add(childElement);
                        break;
                    case NodeTypes.HasChildren:
                        listOfNodes.AddRange(GetListOfNodes(childElement));
                        break;
                }
            }
            return listOfNodes;
        }

        /// <summary>
        /// Concrete implementation of logging methods.
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
            Log.LogError(String.Format("WARNING FROM {0}: {1}", this.GetType().ToString(), message));
        }
    }
    /// <summary>
    /// Each XML node is one of two types - it has descendants (HasChildren) or it does not (IsAttribute) 
    /// </summary>
    public enum NodeTypes
    {
        HasChildren,
        IsLeaf
    }
}
