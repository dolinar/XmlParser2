using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;
using XmlParser.Logger;

namespace XmlParser
{
    public class SimpleXmlParser : XmlParser
    {

        /// <summary>
        /// Class extending from XmlParser abstract class, 
        /// </summary>
        /// <remarks>
        /// Class generates XDocument with the help of ReadXml class, parses data and sets the output property
        /// </remarks>
        public override void DoSomething(string path)
        {
            using (Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                FilePath = path;
                base.SetDocument();
                base.ParseDocument();

                //LogMessage("XML file read, document is not null.");
                LogMessage("Starting parsing Document");
                Document = XDocument.Parse(Document.ToString());

                LogMessage("Starting getting Values");
                // Name of tag + path to the tag
                // for duplicate tags, tag's attribute is used instead of the name
                // just hope there is no attribute duplicates.
                ParsedData = GetNamesAndPaths();


                string tagName = "ime_sole";
                // GetValueFromPath vrne vrednost, ki se nahaja na podani poti
                // ime_sole  ---  root/sola/ime_sole  ---  FRI
                foreach (var e in ParsedData)
                {
                    if (tagName == e.Key)
                    {
                        Console.WriteLine(e.Key + "  ---  " + e.Value + "  ---  " + GetValueFromPath(e.Value));
                    }
                }

                Console.ReadKey();
                
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">Path that gets us to the wanted value. Path uses / delimeter.</param>
        /// <returns>Tag value</returns>
        private string GetValueFromPath(string path)
        {
            var element = Document.XPathSelectElement(path);
            return element.Value;

        }

        // TODO: List<KeyValuePair> is not good enough.
        private List<KeyValuePair<string, string>> GetNamesAndPaths()
        {
            List<KeyValuePair<string, string>> values = new List<KeyValuePair<string, string>>();

            List<XElement> list = GetListOfNodes(Document.Root);
            LogMessage("Finished finding leaf nodes.");

            foreach (XElement e in list)
            {
                int numOfAttributes = e.Attributes().Count();   
                if (numOfAttributes == 0)
                {
                    values.Add(new KeyValuePair<string, string>(e.Name.LocalName, GetPath(e)));
                }
                else
                {
                    if (numOfAttributes > 1)
                    {
                        LogWarning(String.Format("Number of attributes at tag {0} greater than one.", e.Name.LocalName));
                    }
                    string attribute = "";
                    foreach (XAttribute att in e.Attributes())
                    {
                        attribute = att.Value;
                    }
                    values.Add(new KeyValuePair<string, string>(attribute, GetPath(e)));
                }


            }
            LogMessage("Finished finding node paths.");
            return values;
        }

        /// <summary>
        /// Get absolute path from root node to the given XElement
        /// </summary>
        /// <param name="element">Leaf node of type XElement</param>
        /// <returns>Absolute path from the root element to the leaf node</returns>
        private string GetPath(XElement element)
        {
            XElement parent = element.Parent;
            if (parent == null)
            {
                return element.Name.LocalName;
            }
            return GetPath(parent) + "/" + element.Name.LocalName;
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
        protected override void LogMessage(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogMessage(String.Format("MESSAGE: {0}", message));
        }

        protected override void LogWarning(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogWarning(String.Format("WARNING: {0}", message));
        }
        protected override void LogError(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogError(String.Format("ERROR: {0}", message));
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
