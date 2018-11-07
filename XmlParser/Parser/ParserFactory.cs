using System;
using System.Collections.Generic;
using System.Text;

namespace MetronikParser.Parser
{
    public abstract class ParserFactory
    {
        public enum ParserType
        {
            SIMPLEXMLPARSER,
            ADVANCEDXMLPARSER
        }
        public abstract IParser CreateParser(ParserType type);

    }
}