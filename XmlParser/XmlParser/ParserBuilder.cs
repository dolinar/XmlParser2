using System;
using System.Collections.Generic;
using System.Text;

namespace XmlParser.Parser
{
    public class ParserBuilder
    {
        public void ParseDocument(IParserFactory parserFactory, string filePath)
        {
            IParser parser = parserFactory.CreateParser();
            parser.DoSomething(filePath);
        }
    }
}