using System.ComponentModel;

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