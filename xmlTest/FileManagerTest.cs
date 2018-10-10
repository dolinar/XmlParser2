using System;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using XmlParser;

namespace xmlTest
{
    [TestClass]
    public class FileManagerTest
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CheckForNullPath()
        {
            ReadXml r = new ReadXml();
            r.StringPath = null;

            r.ReadFile();
        }

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void CheckIfFileExists()
        {
            ReadXml r = new ReadXml();
            r.StringPath = @"bla.xml";

            XDocument doc = r.ReadFile();
        }

        [TestMethod]
        public void TryReadingXmlFile()
        {
            ReadXml r = new ReadXml();
            r.StringPath = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test.xml";

            XmlParser.XmlParser parser = new SimpleXmlParser();
            parser.Document = r.ReadFile();
        }

        
        //not needed for now, absolute path is expected
        //[TestMethod]
        //public void TryReadingXmlFromRelativePath()
        //{
        //    ReadXml r = new ReadXml();
        //    r.StringPath = @"..\test.xml";
        //    XmlParser.XmlParser parser = new SimpleXmlParser();
        //    parser.Document = r.ReadFile();
        //}
    }
}
