using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Logger;

namespace XmlParser.FileReader
{
    class ReadConfig : FileObject<string[]>
    {
        public override string[] ReadFile()
        {
            List<string> lines = new List<string>();
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

                return File.ReadAllLines(StringPath);
            }
        }
    }
}
