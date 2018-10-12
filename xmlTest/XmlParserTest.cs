using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlParser;
using XmlParser.Logger;

namespace xmlTest
{
    [TestClass]
    public class XmlParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckForNullDocument()
        {
            XmlParser.XmlParser parser = new SimpleXmlParser();
            parser.Document = null;
            parser.ParseDocument();

        }
        
        [TestMethod]
        public void CheckMessageLogger()
        {
            XmlParser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                
                parser.Log.LogMessage("Testing Logger");
            }

        }

        [TestMethod]
        public void CheckWarningLogger()
        {
            XmlParser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.Get(LoggerType.DEBUG))
            {

                parser.Log.LogWarning("Testing Logger");
            }

        }

        [TestMethod]
        public void CheckErrorLogger()
        {
            XmlParser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.Get(LoggerType.DEBUG))
            {

                parser.Log.LogError("Testing Logger");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SimpleXMLDocument()
        {
            XmlParser.XmlParser parser = new SimpleXmlParser();
            //TextReader t = new StringReader("<xml></xml>");
            var doc = XDocument.Load("<xml></xml>");

            parser.Document = doc;
            parser.ParseDocument();
        }

        private XmlParser.XmlParser LoadParser()
        {
            ReadXml r = new ReadXml();
            r.StringPath = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test.xml";

            XmlParser.XmlParser parser = new SimpleXmlParser();
            parser.Document = r.ReadFile();
            return parser;
        }

        [TestMethod]
        public void TestOutput()
        {
            XmlParser.XmlParser parser = LoadParser();
            //parser.ParsedData = parser.Document.Root.ToString();
            using (parser.Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                //parser.Log.LogMessage(parser.Output + "");
            }
        }

        [TestMethod]
        public void TestDescendantsCount()
        {
            XmlParser.XmlParser parser = LoadParser();
            int descendantsCount = parser.Document.Descendants().Count();

            using (parser.Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                parser.Log.LogMessage("# of descendants: " + descendantsCount);
            }

            Assert.AreEqual(30, descendantsCount);

        }

        [TestMethod]
        public void TestElements()
        {
            XmlParser.XmlParser parser = LoadParser();
            parser.Log = LoggerFactory.Get(LoggerType.DEBUG);
            XDocument parsedDocument = XDocument.Parse(parser.Document.ToString());
            parser.Log.LogMessage(parsedDocument.ToString());
            foreach (XElement e in parsedDocument.Elements())
            {
                parser.Log.LogMessage(e.Value);
            }

        }
    }
}
