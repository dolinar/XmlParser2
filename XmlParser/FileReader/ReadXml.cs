using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Xml;
using System.Xml.Linq;

namespace XmlParser
{
    public class ReadXml : FileObject<XDocument>
    {

        public override XDocument ReadFile()
        {
            if (StringPath == null)
            {
                throw new ArgumentNullException();
            }

            if (!File.Exists(StringPath))
            {
                throw new FileNotFoundException();
            }

            return XDocument.Load(StringPath);
        }
    }
}
