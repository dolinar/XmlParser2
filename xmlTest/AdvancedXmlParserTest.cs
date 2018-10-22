using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MetronikParser;
using MetronikParser.Helpers;
using MetronikParser.Logger;
using MetronikParser.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace xmlTest
{
    [TestClass]
    public class AdvancedXmlParserTest
    { 
        private XmlParser LoadParser()
        {
            XmlParser parser = new AdvancedXmlParser();
            parser.Document = XDocument.Parse(
                            @"<root>
                                <ime testAtt=""testAtt"">Rok</ime>
                                <ime testAtt2=""testAtt2"">Rok2</ime>
                                <priimek>Dolinar</priimek>
                                <priimek>Dolinar2</priimek>
                                <kraj>Javorje 34</kraj>
                                <sola>
                                  <ime_sole>FRI</ime_sole>
                                  <lokacija>Vecna pot</lokacija>
                                </sola>
                              </root>");
            return parser;
        }

        [TestMethod]
        public void TestConfig()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/ime", "root/priimek", "root/sola/ime_sole" })
            };
            parser.ParseData();
            Assert.AreEqual(5, parser.ParsedData.Count);
        }

        [TestMethod]
        public void TestWrongConfigPath()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config();
            parser.ParserConfig.Paths = new List<string>(new string[] { "root/ime/doesNotExist" });
            parser.ParseData();
            Assert.AreEqual(0, parser.ParsedData.Count);
        }

        [TestMethod]
        public void TestPathWithAttributeProvided()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/ime[@testAtt='testAtt']" })
            };
            parser.ParseData();
            Tag t = parser.ParsedData.ElementAt(0);
            Assert.AreEqual(t.TagValue, "Rok");
        }

        [TestMethod]
        public void TestPathWithDuplicateTagProvided()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/ime[@testAtt2='testAtt2']" })
            };
            parser.ParseData();
            Tag t = parser.ParsedData.ElementAt(0);
            Assert.AreEqual(t.TagValue, "Rok2");
        }

        [TestMethod]
        public void TestPathWithDuplicateTagNoAttributesProvided()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/priimek" })
            };
            parser.ParseData();
            Tag t = parser.ParsedData.ElementAt(0);
            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                foreach (var tag in parser.ParsedData)
                {
                    parser.Log.LogMessage(tag.TagName + " --- " + tag.TagValue + " --- " + tag.TagPath);
                }
            }
            Assert.AreEqual(t.TagValue, "Dolinar");
        }

        [TestMethod]
        public void TestDescendants()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/sola" }),
                RequireChildren = true
            };
            parser.ParseData();
            Tag t = parser.ParsedData.ElementAt(0);
            //Assert.AreEqual(t.TagValue, "Dolinar");
            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                foreach (var e in t.Children)
                {
                    parser.Log.LogMessage(e.TagName + " --- " + e.TagValue + " --- " + e.TagPath);
                }
            }
        }

        //[TestMethod]
        //public void TestDescendants2()
        //{
        //    XmlParser parser = LoadParser();
        //    parser.ParserConfig = new Config
        //    {
        //        Paths = new List<string>(new string[] { "root" }),
        //        RequireChildren = true
        //    };
        //    parser.ParseData();
        //    Tag t = parser.ParsedData.ElementAt(0);
        //    //Assert.AreEqual(t.TagValue, "Dolinar");
        //    using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
        //    {
        //        while (t.TagName != "ime_sole")
        //        {
        //            foreach (var e in t.Children)
        //            {

        //            }
        //        }
        //    }
        //}

        [TestMethod]
        public void TestBigXml()
        {
            ReadXml r = new ReadXml
            {
                StringPath = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test2.xml"
            };

            XmlParser parser = new AdvancedXmlParser
            {
                Document = r.ReadFile(),

                ParserConfig = new Config
                {
                    Paths = new List<string>(new string[] { "row" }),
                    RequireChildren = true
                }
            };

            parser.ParseData();

            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                foreach (var e in parser.ParsedData)
                {
                    parser.Log.LogMessage(e.TagName + " --- " + e.TagValue + " --- " + e.TagPath + "\n");
                }
            }
        }
    }
}
