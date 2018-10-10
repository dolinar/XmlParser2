using System;
using System.Collections.Generic;
using System.Text;

namespace XmlParser.Parser
{
    public class SimpleXmlParserFactory : IParserFactory
    {
        public IParser CreateParser()
        {
            return new SimpleXmlParser();
        }
    }
}