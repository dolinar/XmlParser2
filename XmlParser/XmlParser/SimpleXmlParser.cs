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
        public enum NodeTypes
        {
            HasChildren,
            IsNode,
            IsAttribute
        }

        /// <summary>
        /// Class extending from XmlParser abstract class, 
        /// </summary>
        /// <remarks>
        /// Class generates XDocument with the help of ReadXml class, parses data and sets the output property
        /// </remarks>
        public override void ParseDocument(string filePath)
        {
            // initialize debug logging
            Log = LoggerFactory.Get(LoggerType.DEBUG);

            // generate XDocument with ReadXml class. 
            // path is provided in the Program.cs and is passed to this method via ParserBuilder and abstract factory.
            FileObject<XDocument> readXml = new ReadXml { StringPath = filePath };
            Document = readXml.ReadFile();
            LogMessage("Reading XML file");


            // make sure the generated XML document is not null
            if (Document == null)
                throw new ArgumentNullException();
            LogMessage("XML file read, document is not null.");

            Document = XDocument.Parse(Document.ToString());
            LogMessage("Document parsed");

            ParsedData = GetValuesAndPaths();

            foreach (var e in ParsedData)
            {
                Console.WriteLine("(" + e.Key + ")      (" + e.Value + ")");
            }
            //var e = Document.XPathSelectElements("root/report/metadata/machine");
            //foreach (var element in e)
            //{
            //    LogMessage(element.Name.LocalName + "      " + element.Value);
            //}
            Console.ReadKey();
        }

        // TODO: testing git.
        private List<KeyValuePair<string, string>> GetValuesAndPaths()
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
                } else
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

        // get absolute path of leaf node
        private string GetPath(XElement element)
        {
            XElement parent = element.Parent;
            if (parent == null)
            {
                return element.Name.LocalName;
            }
            return GetPath(parent) + "/" + element.Name.LocalName;
        }


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


        protected override void LogMessage(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogMessage(String.Format("MESSAGE: {0}" + Environment.NewLine, message));
        }

        protected override void LogWarning(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogWarning(String.Format("WARNING: {0}\n", message));
        }
        protected override void LogError(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogError(String.Format("ERROR: {0}\n", message));
        }


    }
}
