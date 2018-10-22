using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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
        public Dictionary<string, string> Attributes { get; }
        public List<Tag> Children { get; }

        public Tag()
        {
            Children = new List<Tag>();
            Attributes = new Dictionary<string, string>();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="element">All needed values for Tag are stored in here</param>
        /// <param name="path">Only needed for setting the Tag TagPath property.</param>
        /// <returns></returns>
        public void SetTagFromElement(XElement element)
        {
            TagName = element.Name.LocalName;
            TagValue = element.Value;
            TagPath = getPath(element);

            foreach (XAttribute attribute in element.Attributes())
                Attributes.Add(attribute.Name.LocalName, attribute.Value);
        }

        public Tag AddChild(XElement element)
        {
            Tag currentTag = new Tag();
            SetTagFromElement(element);
            Children.Add(currentTag);
            return currentTag;
        }

        /// <summary>
        /// Get absolute path from root node to the given XElement
        /// </summary>
        /// <param name="element">Leaf node of type XElement</param>
        /// <returns>Absolute path from the root element to the leaf node</returns>
        private string getPath(XElement element)
        {
            XElement parent = element.Parent;
            if (parent == null)
                return element.Name.LocalName;

            return getPath(parent) + "/" + element.Name.LocalName;
        }
    }
}
