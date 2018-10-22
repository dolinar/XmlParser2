using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MetronikParser.Parser;
using MetronikParser.Logger;
using MetronikParser.Helpers;

namespace xmlTest
{
    [TestClass]
    public class SimpleXmlParserTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckForNullDocument()
        {
            MetronikParser.Parser.XmlParser parser = new SimpleXmlParser();
            parser.Document = null;
            parser.ParseDocument();

        }
        
        [TestMethod]
        public void CheckMessageLogger()
        {
            MetronikParser.Parser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                
                parser.Log.LogMessage("Testing Logger");
            }

        }

        [TestMethod]
        public void CheckWarningLogger()
        {
            MetronikParser.Parser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {

                parser.Log.LogWarning("Testing Logger");
            }

        }

        [TestMethod]
        public void CheckErrorLogger()
        {
            MetronikParser.Parser.XmlParser parser = new SimpleXmlParser();
            var doc = new XmlDocument();
            doc.LoadXml("<xml></xml>");

            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {

                parser.Log.LogError("Testing Logger");
            }

        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void SimpleXMLDocument()
        {
            XmlParser parser = new SimpleXmlParser();
            //TextReader t = new StringReader("<xml></xml>");
            var doc = XDocument.Load("<xml></xml>");

            parser.Document = doc;
            parser.ParseDocument();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TestDuplicateAttribute()
        {
            XmlParser parser = new SimpleXmlParser();
            //TextReader t = new StringReader("<xml></xml>");
            var doc = XDocument.Load(@"<xml att=""123"" att=""1234""></xml>");

            parser.Document = doc;
            parser.ParseDocument();
        }

        private XmlParser LoadParser()
        {
            XmlParser parser = new SimpleXmlParser();
            parser.Document = XDocument.Parse(
                            @"<root>
                                <ime testAtt=""testAtt"">Rok</ime>
                                <priimek>Dolinar</priimek>
                                <kraj>Javorje 34</kraj>
                                <sola>
                                  <ime_sole>FRI</ime_sole>
                                  <lokacija>Vecna pot</lokacija>
                                </sola>
                              </root>");
            return parser;
        }

        [TestMethod]
        public void TestRootTag()
        {
            XmlParser parser = LoadParser();
            parser.ParseData();
            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                parser.Log.LogMessage("root tag: " + parser.Document.Root.Name.LocalName);
            }
            Assert.AreEqual("root", parser.Document.Root.Name.LocalName);
        }

        [TestMethod]
        public void TestDescendantsCount()
        {
            XmlParser parser = LoadParser();
            int descendantsCount = parser.Document.Descendants().Count();

            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                parser.Log.LogMessage("# of descendants: " + descendantsCount);
            }

            Assert.AreEqual(7, descendantsCount);
        }

        [TestMethod]
        public void TestGetParsedData()
        {
            XmlParser parser = LoadParser();
            parser.ParseData();
            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                foreach (var e in parser.ParsedData)
                {
                    if (e.Attributes.Count > 0)
                    {
                        parser.Log.LogMessage(e.TagName + " : " + e.TagValue + " : " + e.Attributes.First().Key + " : " + e.Attributes.First().Value);
                    } else
                    {
                        parser.Log.LogMessage(e.TagName + " : " + e.TagValue);
                    }
                }
            }
            bool notEmpty = parser.ParsedData.Capacity > 0;
            Assert.AreEqual(true, notEmpty);
        }

        [TestMethod]
        public void TestTagValue()
        {
            XmlParser parser = LoadParser();
            parser.ParseData();
            Tag t = parser.ParsedData.First();
            Assert.AreEqual("Rok", t.TagValue);
        }

        [TestMethod]
        public void TestTagName()
        {
            XmlParser parser = LoadParser();
            parser.ParseData();
            Tag t = parser.ParsedData.First();
            Assert.AreEqual("ime", t.TagName);
        }

        [TestMethod]
        public void TestTagAttribute()
        {
            XmlParser parser = LoadParser();
            parser.ParseData();
            Tag t = parser.ParsedData.First();
            Assert.AreEqual("testAtt", t.Attributes.First().Value);
        }
    }
}
