using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace DotNet.Common
{
    /// <summary>
    /// XML 操作类
    /// </summary>
    public class XMLHelper
    {
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="obj">对象</param>
        public static string Serialize<T>(T obj)
        {
            using (StringWriter sw = new StringWriter())
            {
                Type t = obj.GetType();
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(sw, obj);
                sw.Close();
                return sw.ToString();
            }
        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="strXML">xml 字符串</param>
        /// <returns></returns>
        public static T Deserialize<T>(string strXML) where T : class
        {
            try
            {
                using (StringReader sr = new StringReader(strXML))
                {
                    XmlSerializer serializer = new XmlSerializer(typeof(T));
                    return serializer.Deserialize(sr) as T;
                }
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="xmlfile"></param>
        /// <param name="rootElement"></param>
        /// <param name="subElement"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public static void Create(string xmlfile,string rootElement,string subElement,string element,string attribute,string value)
        {
            if (!File.Exists(xmlfile))
            {
                XDocument xd = new XDocument(
                    new XElement(rootElement, 
                        new XElement(subElement, 
                            new XElement(element, new XAttribute(attribute, value))
                        )
                    )
                );
                xd.Save(xmlfile);
            }
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="path"></param>
        /// <param name="node"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            XDocument xdoc = XDocument.Load(path);
            XElement xeElement = xdoc.Root.Element(node);
            XElement xeNew = new XElement(element);
            xeNew.SetAttributeValue(attribute, value);
            xeElement.Add(xeNew);
        }

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="xmlFile"></param>
        public static void Read(string xmlFile)
        {
            XDocument xdoc = XDocument.Load(xmlFile); 
            IEnumerable<XElement> xeSet = xdoc.Root.Elements();
            foreach (XElement xeRoot in xeSet)
            {
                foreach (XElement xe in xeRoot.Elements())
                {
                    Console.WriteLine($"Name:{xe.Name},Attribute:{xe.Attribute("id")}");
                }
            }
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="path"></param>
        /// <param name="element"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public static void Update(string path, string element, string attribute, string value)
        {
            try
            {
                XDocument xdoc = XDocument.Load(path);
                XElement xe = xdoc.Root.Elements(element).FirstOrDefault();
                xe.SetAttributeValue(attribute, value);  
                xdoc.Save(path);
            }
            catch { }
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="xmlfile"></param>
        /// <param name="parentElement"></param>
        /// <param name="finalElement"></param>
        /// <param name="attribute"></param>
        /// <param name="value"></param>
        public static void Delete(string xmlfile,string parentElement,string finalElement,string attribute,string value)
        {
            if (File.Exists(xmlfile))
            {
                XDocument xd = XDocument.Load(xmlfile);

                //删除节点 全部没有了
                xd.Descendants(parentElement)
                    .Where(p => p.Element(finalElement).Attribute(attribute).Value == value).First()
                    .Remove();
                //删除节点内容 只剩下节点名
                xd.Descendants(parentElement)
                    .Where(p => p.Element(finalElement).Attribute(attribute).Value == value).First()
                    .RemoveAll();
                //删除节点属性 内容还在
                xd.Descendants(parentElement)
                    .Where(p => p.Element(finalElement).Attribute(attribute).Value == value).First()
                    .RemoveAttributes();
                //删除节点内容 只剩下节点名和属性
                xd.Descendants(parentElement)
                    .Where(p => p.Element(finalElement).Attribute(attribute).Value == value).First()
                    .RemoveNodes();

                xd.Save(xmlfile);
            }
        }
    }
}