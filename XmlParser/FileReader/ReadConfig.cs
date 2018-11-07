using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetronikParser.Logger;

namespace MetronikParser.FileReader
{
    public class ReadConfig : FileObject<string[]>
    {
        public override string[] ReadFile()
        {
            List<string> lines = new List<string>();
            using (Log = LoggerFactory.GetLogger(LoggerType.DEBUG))
            {
                if (StringPath == null)
                    throw new ArgumentNullException();

                if (!File.Exists(StringPath))
                    throw new FileNotFoundException();

                return File.ReadAllLines(StringPath);
            }
        }
    }
}
