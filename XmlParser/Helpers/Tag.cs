using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetronikParser.Helpers
{
    public class Tag 
    {
        public bool IsNode
        {
            get
            {
                return Children.Count > 0;
            }
        }
        public string TagName { get; set; }
        public string TagValue { get; set; }
        public string TagPath { get; set; }
        public Dictionary<string, string> Attributes { get; set; }

        public Dictionary<Tag, List<Tag>> Children { get; set; }
    }
}
