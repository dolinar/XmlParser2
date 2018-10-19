using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MetronikParser;
using MetronikParser.Parser;

namespace Test
{
    class Program
    {
        //test commit
        static void Main(string[] args)
        {
            int i = 1;

            //string path = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test2.xml";
            //string path2 = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test2.xml";


            switch (i)
            {
                case 1:
                    IParser p = new XmlParserFactory().CreateParser(ParserFactory.ParserType.SIMPLEXMLPARSER);
                    p.ParseData();
                    break;
                default:
                    break;

            }
        }
    }
}
