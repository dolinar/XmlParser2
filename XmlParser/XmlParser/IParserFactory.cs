using System;
using System.Collections.Generic;
using System.Text;

namespace XmlParser.Parser
{
    public interface IParserFactory
    {
        IParser CreateParser();
    }
}