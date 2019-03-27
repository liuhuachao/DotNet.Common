using System;
using System.IO;
using System.Xml;
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
        /// <param name="filename">文件路径</param>
        public static void Serialize(object obj, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(obj.GetType());
                serializer.Serialize(fs, obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }

        }

        /// <summary>
        /// 反序列化
        /// </summary>
        /// <param name="type">对象类型</param>
        /// <param name="filename">文件路径</param>
        /// <returns></returns>
        public static object Deserialize(Type type, string filename)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(type);
                return serializer.Deserialize(fs);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (fs != null)
                {
                    fs.Close();
                }
            }
        }

        /// <summary>
        /// 加载XML文件
        /// </summary>
        /// <param name="XMLPath">XML文件路径</param>
        private static XmlDocument Load(string strPath)
        {
            XmlDocument xmldoc = new XmlDocument();
            try
            {
                string filename = AppDomain.CurrentDomain.BaseDirectory.ToString() + strPath;
                if (File.Exists(filename))
                {
                    xmldoc.Load(filename);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            return xmldoc;
        }

        /// <summary>
        /// 读取指定路径和节点的值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// 使用示列:
        /// XMLProsess.Read(path, "/Node")
        public static string Read(string path, string node)
        {
            string value = "";
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = xn.InnerText;
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 读取指定路径和节点的属性值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时返回该属性值，否则返回串联值</param>
        /// 使用示列:
        /// XMLProsess.Read(path, "/Node/Element[@Attribute='Name']", "Attribute")
        public static string Read(string path, string node, string attribute)
        {
            string value = "";
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                value = (attribute.Equals("") ? xn.InnerText : xn.Attributes[attribute].Value);
            }
            catch { }
            return value;
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="attribute">属性名，非空时插入该元素属性值，否则插入元素值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Insert(path, "/Node", "Element", "", "Value")
        /// XMLProsess.Insert(path, "/Node", "Element", "Attribute", "Value")
        /// XMLProsess.Insert(path, "/Node", "", "Attribute", "Value")
        public static void Insert(string path, string node, string element, string attribute, string value)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                if (element.Equals(""))
                {
                    if (!attribute.Equals(""))
                    {
                        XmlElement xe = (XmlElement)xn;
                        xe.SetAttribute(attribute, value);
                    }
                }
                else
                {
                    XmlElement xe = doc.CreateElement(element);
                    if (attribute.Equals(""))
                        xe.InnerText = value;
                    else
                        xe.SetAttribute(attribute, value);
                    xn.AppendChild(xe);
                }
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="element">元素名，非空时插入新元素，否则在该元素中插入属性</param>
        /// <param name="strList">由XML属性名和值组成的二维数组</param>
        public static void Insert(string path, string node, string element, string[][] strList)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = doc.CreateElement(element);
                string strAttribute = "";
                string strValue = "";
                for (int i = 0; i < strList.Length; i++)
                {
                    for (int j = 0; j < strList[i].Length; j++)
                    {
                        if (j == 0)
                            strAttribute = strList[i][j];
                        else
                            strValue = strList[i][j];
                    }
                    if (strAttribute.Equals(""))
                        xe.InnerText = strValue;
                    else
                        xe.SetAttribute(strAttribute, strValue);
                }
                xn.AppendChild(xe);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 修改指定节点的数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Update(path, "/Node","Value")
        public static void Update(string path, string node, string value)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.InnerText = value;
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 修改指定节点的属性值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时修改该节点属性值，否则修改节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Update(path, "/Node", "Attribute", "Value")
        public static void Update(string path, string node, string attribute, string value)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xe.InnerText = value;
                else
                    xe.SetAttribute(attribute, value);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 删除节点值
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Delete(path, "/Node", "")
        public static void Delete(string path, string node)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                xn.ParentNode.RemoveChild(xn);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="node">节点</param>
        /// <param name="attribute">属性名，非空时删除该节点属性值，否则删除节点值</param>
        /// <param name="value">值</param>
        /// 使用示列:
        /// XMLProsess.Delete(path, "/Node", "Attribute")
        public static void Delete(string path, string node, string attribute)
        {
            try
            {
                XmlDocument doc = Load(path);
                XmlNode xn = doc.SelectSingleNode(node);
                XmlElement xe = (XmlElement)xn;
                if (attribute.Equals(""))
                    xn.ParentNode.RemoveChild(xn);
                else
                    xe.RemoveAttribute(attribute);
                doc.Save(AppDomain.CurrentDomain.BaseDirectory.ToString() + path);
            }
            catch { }
        }
    }
}