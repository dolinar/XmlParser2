﻿using System.Collections.Generic;
using System.Xml;
using MetronikParser.Logger;
using MetronikParser.Helpers;

namespace MetronikParser.Parser
{
    public interface IParser
    {
        ILogger Log { get; set; }

        Config ParserConfig { get; set; }

        List<Tag> ParsedData { get; set; }

        void ParseDocument();

        void ParseData();
    }
}