using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MetronikParser;
using MetronikParser.Helpers;
using MetronikParser.Logger;
using MetronikParser.Parser;

namespace Test
{
    class Program
    {
        //test commit
        static void Main(string[] args)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Dictionary<string, List<Tag>> data = init();
            Console.WriteLine("Elapsed milliseconds: " + sw.ElapsedMilliseconds);
            displayData(data);
            sw.Stop();
            Console.WriteLine("Total time elapsed: "+ sw.Elapsed);
            Console.ReadKey();
        }

        private static Dictionary<string, List<Tag>> init()
        {
            ReadXml r = new ReadXml
            {
                StringPath = @"C:\Users\dor\source\repos\xmlTest\XmlParser\test2.xml"
            };

            XmlParser parser = new AdvancedXmlParser();
            try
            {
                parser.Document = r.ReadFile();
                parser.ParserConfig = new Config
                {
                    Paths = new List<string>(new string[] { "rows/head", "rows/row" }),
                };
                    
                parser.ParseData(LoggerType.DEBUG);
                return parser.ParsedData;
            } catch (FileNotFoundException fnfException)
            {
                Console.WriteLine(fnfException.Message + " " + fnfException.FileName);
                Console.WriteLine("Click any key to exit the program");
                Console.ReadKey();
                Environment.Exit(1);
            } catch (XmlException xmlException)
            {
                Console.WriteLine(xmlException.Message);
                Console.WriteLine("Click any key to exit the program");
                Console.ReadKey();
                Environment.Exit(1);
            } catch (ArgumentNullException anException)
            {
                Console.WriteLine(anException.Message);
                Console.WriteLine("Click any key to exit the program");
                Console.ReadKey();
                Environment.Exit(1);
            } catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Click any key to exit the program");
                Console.ReadKey();
                Environment.Exit(1);
            }
            return null;
        }

        private static void displayData(Dictionary<string, List<Tag>> data)
        {
            foreach (KeyValuePair<string, List<Tag>> pair in data)
            {
                Console.WriteLine("root tag '" + pair.Key + "s'");
                foreach (Tag t in pair.Value)
                {
                    Console.WriteLine("\tTag name '" + t.TagName +  "'");
                    foreach (KeyValuePair<string, string> attPair in t.Attributes)
                    {
                        Console.WriteLine("\t\tAttribute '" + attPair.Key + "' with value '" + attPair.Value + "'");
                    }
                    if (t.TagValue == null)
                    {
                        foreach (KeyValuePair<string, List<Tag>> pair2 in t.Children)
                        {
                            Console.WriteLine("\t\t\tchild tag '" + pair2.Key + "'");
                            foreach (Tag t2 in pair2.Value)
                            {
                                foreach (KeyValuePair<string, string> attPair2 in t.Attributes)
                                {
                                    Console.WriteLine("\t\t\t\tAttribute '" + attPair2.Key + "' with value '" + attPair2.Value + "'");
                                }

                                Console.WriteLine("\t\t\t\tTag value: '" + t2.TagValue + "'");
                                Console.WriteLine();
                            }

                        }
                    }
                    else
                    {
                        Console.WriteLine("\t\tTag value: '" + t.TagValue + "'");
                    }
                }
            }
        }
    }
}
