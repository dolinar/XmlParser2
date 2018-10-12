using System.Xml;
using XmlParser.Logger;

namespace XmlParser
{
    public interface IParser
    {
        ILogger Log { get; set; }

        void SetDocument();

        void ParseDocument();

        void DoSomething(string path);

    }
}