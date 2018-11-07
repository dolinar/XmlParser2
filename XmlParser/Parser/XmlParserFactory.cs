using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using MetronikParser.FileReader;

namespace MetronikParser.Parser
{
    public class XmlParserFactory : ParserFactory
    {
        public override IParser CreateParser(ParserType type)
        {
            switch (type)
            {
                case ParserType.SIMPLEXMLPARSER:
                    return new SimpleXmlParser();
                default:
                    throw new InvalidEnumArgumentException("type", (int)type, typeof(ParserType));
            }
        }
    }
}