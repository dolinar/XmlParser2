using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XmlParser.Parser;

namespace Test
{
    class Program
    {
        //test commit
        static void Main(string[] args)
        {
            int i = 1;

            string path = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test.xml";

            ParserBuilder pb = new ParserBuilder();
            IParserFactory pf = null;

            switch (i)
            {
                case 1:
                    pf = new SimpleXmlParserFactory();
                    pb.ParseDocument(pf, path);
                    break;
                default:
                    pf = new SimpleXmlParserFactory();
                    break;

            }
        }
    }
}
