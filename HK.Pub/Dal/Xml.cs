using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace HK.Pub.Dal
{
    public class Xml
    {
        XmlDocument doc;
        XmlElement root;
        public Xml(string xml)
        {
            doc = new XmlDocument();
            doc.LoadXml(xml);
            root = doc.DocumentElement;
        }
        public XmlNode GetNode(string key)
        {
            return root[key];
        }
        public string GetText(string key)
        {
            XmlNode node=GetNode(key);
            return node==null?"": (node.ChildNodes.Count>1?node.OuterXml: node.InnerText);
        }
        public string this[string key] { get { return GetText(key); } }
    }
}
