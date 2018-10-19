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
        /// Class extending from XmlParser abstract class
        /// </summary>
        /// <remarks>
        /// 
        /// </remarks>

        List<Tag> _data = null;

        public SimpleXmlParser()
        {
            _data = new List<Tag>();
        }

        public override void ParseData()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                base.ParseDocument();
                List<Tag> tags = GetTags();
                foreach (var tag in tags)
                {
                    _data.Add(tag);
                }
                ParsedData = _data;
            }
        }

        private List<Tag> GetTags()
        {
            List<Tag> tags = new List<Tag>();

            List<XElement> list = GetListOfNodes(Document.Root);
            LogMessage("Finished finding leaf nodes.");

            foreach (XElement e in list)
            {
                Tag t = new Tag();
                t.TagName = e.Name.LocalName;
                t.TagValue = e.Value;
                t.Attributes = new Dictionary<string, string>();
                foreach (XAttribute attribute in e.Attributes())
                {
                    t.Attributes.Add(attribute.Name.LocalName, attribute.Value);
                }

                tags.Add(t);
            }

            LogMessage("Finished finding node paths. Tags count: " + tags.Count + " Nodes count: " + list.Count);
            return tags;
        }

        /// <summary>
        /// Get list of all leaf nodes (which hold values we are looking for) of a given root element.
        /// </summary>
        /// <param name="elements">Root element of type XElement</param>
        /// <returns>List of all leaf nodes.</returns>
        private List<XElement> GetListOfNodes(XElement elements)
        {
            List<XElement> list = new List<XElement>();
            NodeTypes nodeType;
            foreach (XElement element in elements.Elements())
            {
                nodeType = element.Descendants().Count() > 0 ? NodeTypes.HasChildren : NodeTypes.IsAttribute;

                switch (nodeType)
                {
                    case NodeTypes.IsAttribute:
                        list.Add(element);
                        break;
                    case NodeTypes.HasChildren:
                        list.AddRange(GetListOfNodes(element));
                        break;
                }
            }
            return list;
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
        IsAttribute
    }
}
