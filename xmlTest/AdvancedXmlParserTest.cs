using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            parser.ParseData(LoggerType.DEBUG);
            Assert.AreEqual(3, parser.ParsedData.Count);
        }

        [TestMethod]
        public void TestWrongConfigPath()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config();
            parser.ParserConfig.Paths = new List<string>(new string[] { "root/ime/doesNotExist" });
            parser.ParseData(LoggerType.DEBUG);
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
            parser.ParseData(LoggerType.DEBUG);
            Tag t = parser.ParsedData["ime"].First();
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
            parser.ParseData(LoggerType.DEBUG);
            Tag t = parser.ParsedData["ime"].ElementAt(0);
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
            parser.ParseData(LoggerType.DEBUG);
            Tag t = parser.ParsedData["priimek"].ElementAt(0);
            Assert.AreEqual(t.TagValue, "Dolinar");
        }

        [TestMethod]
        public void TestDescendants()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root/sola" }),
            };
            parser.ParseData(LoggerType.DEBUG);
            Tag t = parser.ParsedData["sola"].ElementAt(0);
            Assert.AreEqual(t.Children["ime_sole"].ElementAt(0).TagValue, "FRI");
        }

        [TestMethod]
        public void TestChildrenOfChildren()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config
            {
                Paths = new List<string>(new string[] { "root" }),
            };
            parser.ParseData(LoggerType.DEBUG);

            Dictionary<string, List<Tag>> children = parser.ParsedData["root"].ElementAt(0).Children["sola"].ElementAt(0).Children;

            Tag tag = children["ime_sole"].ElementAt(0);
            Assert.AreEqual(tag.TagValue, "FRI");
        }

        [TestMethod]
        public void TestBigXml()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            ReadXml r = new ReadXml
            {
                StringPath = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test2.xml"
            };

            XmlParser parser = new AdvancedXmlParser
            {
                Document = r.ReadFile(),

                ParserConfig = new Config
                {
                    Paths = new List<string>(new string[] { "rows/head", "rows", "rows/head/property", "bla", "rows/row" }),
                }
            };

            parser.ParseData(LoggerType.DEBUG);
            sw.Stop();
            using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                parser.Log.LogMessage("Time total: " + sw.Elapsed);
            }
            Assert.IsTrue(sw.Elapsed.TotalSeconds < 1, String.Format("Took more than a second {0} for 1000 line XML, not good enough", sw.Elapsed));
        }

        //[TestMethod]
        //public void testGetListByKey()
        //{
        //    XmlParser parser = LoadParser();
        //    parser.ParserConfig = new Config
        //    {
        //        Paths = new List<string>(new string[] { "root" }),
        //        RequireChildren = true
        //    };
        //    parser.ParseData();
        //    Tag rootTag = parser.ParsedData["root"].First();
        //    List<Tag> found = rootTag.FindListOfTagsByKey("sola", rootTag.Children);
        //    foreach (var item in found)
        //    {
        //        using (parser.Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
        //        {
        //            parser.Log.LogMessage(item.TagName + " --- " + item.TagValue);
        //        }
        //    }
        //}
    }
}
