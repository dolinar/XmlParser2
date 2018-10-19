using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using MetronikParser.Helpers;
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
        [ExpectedException(typeof(NullReferenceException))]
        public void TestWrongConfigPath()
        {
            XmlParser parser = LoadParser();
            parser.ParserConfig = new Config();
            parser.ParserConfig.Paths = new List<string>(new string[] { "root/ime/doesNotExist" });
            parser.ParseData();
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
            Assert.AreEqual(t.TagValue, "Dolinar");
        }

    }
}
