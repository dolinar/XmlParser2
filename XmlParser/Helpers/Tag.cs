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
        public Dictionary<string, List<Tag>> Children { get; }

        public Tag()
        {
            Children = new Dictionary<string, List<Tag>>();
            Attributes = new Dictionary<string, string>();
        }

        /// <summary>
        /// Set values of properties for this instance.
        /// </summary>
        /// <param name="element">All needed values for Tag are stored in here</param>
        /// <param name="path">Only needed for setting the Tag TagPath property.</param>
        /// <returns></returns>
        public void SetTagFromElement(XElement element)
        {
            TagName = element.Name.LocalName;
            if (element.Elements().Count() == 0)
                TagValue = element.Value;
            else
                TagValue = "notLeaf";

            TagPath  = getPath(element);

            foreach (XAttribute attribute in element.Attributes())
                Attributes.Add(attribute.Name.LocalName, attribute.Value);
        }

        /// <summary>
        /// Create an instance of a Tag from a given element and add it to children
        /// </summary>
        /// <param name="element">Source of data for Tag instance</param>
        /// <returns></returns>
        public Tag AddChild(XElement element)
        {
            Tag currentTag = new Tag();
            currentTag.SetTagFromElement(element);
            if (Children.ContainsKey(currentTag.TagName))
                Children[currentTag.TagName].Add(currentTag);
            else
                Children.Add(currentTag.TagName, new List<Tag> { currentTag });
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

        public List<Tag> FindListOfTagsByKey(string neededKey, Dictionary<string, List<Tag>> nestedDict)
        {
            foreach (KeyValuePair<string, List<Tag>> entry in nestedDict)
            {
                if (neededKey.Equals(entry.Key))
                    return entry.Value;

                object nextLevel = nestedDict[entry.Key];
                if (nextLevel == null)
                    continue;
                FindListOfTagsByKey(neededKey, (Dictionary<string, List<Tag>>)nextLevel);
            }
            return null;
        }
    }
}
