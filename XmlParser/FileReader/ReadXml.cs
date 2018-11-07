using System;
using System.IO;
using System.Xml.Linq;
using MetronikParser.Logger;
using MetronikParser.FileReader;

namespace MetronikParser
{
    public class ReadXml : FileObject<XDocument>
    {

        public override XDocument ReadFile()
        {
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                if (StringPath == null)
                    throw new ArgumentNullException();

                if (!File.Exists(StringPath))
                    throw new FileNotFoundException("File not found.", StringPath);

                return XDocument.Load(StringPath);
                
            }

        }
    }
}
