using System.Xml;
using XmlParser.Logger;

namespace XmlParser
{
    public interface IParser
    {
        ILogger Log { get; set; }

        void ParseDocument(string filePath);
    }
}