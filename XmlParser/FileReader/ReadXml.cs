using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;
using XmlParser.Logger;

namespace XmlParser
{
    public class ReadXml : FileObject<XDocument>
    {

        public override XDocument ReadFile()
        {
            using (Log = LoggerFactory.Get(LoggerType.DEBUG))
            {
                if (StringPath == null)
                {
                    throw new ArgumentNullException();
                }

                if (!File.Exists(StringPath))
                {
                    throw new FileNotFoundException();
                }

                try
                {
                    return XDocument.Load(StringPath);
                }
                catch (Exception)
                {
                    // logging
                    throw;
                }
            }

        }
    }
}
