using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Reflection;

namespace DotNet.Common
{
    /// <summary>
    /// DataTable 操作类
    /// </summary>
    public class DataTableHelper
    {
        /// <summary>
        /// DataTable 转 XML
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static string DataTableToXML(DataTable dt)
        {
            if (dt != null)
            {
                if (dt.Rows.Count > 0)
                {
                    System.IO.StringWriter writer = new System.IO.StringWriter();
                    dt.WriteXml(writer);
                    return writer.ToString();
                }
            }
            return String.Empty;
        }

        /// <summary>
        /// XML 转 DataSet
        /// </summary>
        /// <param name="path">XML文件相对路径</param>
        public DataSet XMLToDataSet(string path)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(FileHelper.GetAbsolutePath(path));
                if (ds.Tables.Count > 0)
                {
                    return ds;
                }
                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }

        /// <summary>
        /// DataRow 转 实体类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T DataRowToModel<T>(DataRow dr)
        {
            T model = Activator.CreateInstance<T>();
            foreach (PropertyInfo pi in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
            {
                if (!IsNullOrDBNull(dr[pi.Name]))
                {
                    pi.SetValue(model, HackType(dr[pi.Name], pi.PropertyType), null);
                }
            }
            return model;
        }
        public static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null) return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }
        public static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }

        /// <summary>
        /// DataTable转为对象LIST
        /// </summary>
        /// <param name="dt"></param>
        /// <returns>数据行是对象的类，类的属性与数据字段一致</returns>
        public static IList DataTableToIList<T>(DataTable dt)
        {
            IList list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                T obj = Activator.CreateInstance<T>();
                PropertyInfo[] propertys = obj.GetType().GetProperties();
                foreach (PropertyInfo pi in propertys)
                {
                    if (dt.Columns.Contains(pi.Name))
                    {
                        if (!pi.CanWrite) continue;
                        object value = dr[pi.Name];
                        if (value != DBNull.Value)
                        {
                            pi.SetValue(obj, value, null);
                        }
                    }
                }
                list.Add(obj);
            }
            return list;
        }
    }
}
