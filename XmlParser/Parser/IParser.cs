﻿using MetronikParser.Logger;
using MetronikParser.Helpers;

namespace MetronikParser.Parser
{
    public interface IParser
    {
        ILogger Log { get; set; }

        Config ParserConfig { get; set; }

        void ParseDocument(LoggerType type);

        void ParseData(LoggerType type);
    }
}