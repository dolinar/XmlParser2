using MetronikParser.Logger;
using MetronikParser.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.XPath;

namespace MetronikParser.Parser
{
    public class AdvancedXmlParser : XmlParser
    {
        private Tag _previousTag = null;

        public override void ParseData()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                base.ParseDocument();

            }
        }

        private Tag getTagFromPath(string path)
        {
            var element = Document.XPathSelectElement(path);
            _previousTag = new Tag();
            _previousTag.TagName = element.Name.LocalName;
            _previousTag.TagValue = element.Value;
            _previousTag.TagPath = path;

            //path += "/" + element.Name.LocalName;

            return _previousTag;
        }

        public override void LogMessage(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogMessage(String.Format("MESSAGE FROM {0}: {1}", this.GetType().ToString(), message));
        }

        public override void LogWarning(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogWarning(String.Format("WARNING FROM {0}: {1}", this.GetType().ToString(), message));
        }

        public override void LogError(string message, params object[] args)
        {
            if (Log == null)
                return;
            Log.LogError(String.Format("ERROR FROM {0}: {1}", this.GetType().ToString(), message));
        }


    }
}
