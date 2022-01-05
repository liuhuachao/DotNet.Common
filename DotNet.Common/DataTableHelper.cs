using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
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
        /// <param name="xmlFile">XML文件名称</param>
        public static DataSet XMLToDataSet(string xmlFile)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.ReadXml(xmlFile);
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

        /// <summary>
        /// 根据类型属性创建空表
        /// 表名和类型名称相同，列名和类型可读属性名称相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateEmptyDataTableFromProperties<T>()
        {
            DataTable dt = null;
            Type obj = typeof(T);
            var objClassName = obj.Name;
            dt = new DataTable(objClassName);
            PropertyInfo[] propertys = obj.GetProperties();
            foreach (var pi in propertys)
            {
                if (!pi.CanRead)
                {
                    continue;
                }
                if (!dt.Columns.Contains(pi.Name))
                {
                    dt.Columns.Add(pi.Name, typeof(string));
                }
            }
            return dt;
        }

        /// <summary>
        /// 根据类型字段创建空表
        /// 表名和类型名称相同，列名和类型公共字段名称相同
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static DataTable CreateEmptyDataTableFromFields<T>()
        {
            DataTable dt = null;
            Type obj = typeof(T);
            var objClassName = obj.Name;
            dt = new DataTable(objClassName);
            FieldInfo[] fieldInfos = obj.GetFields();
            foreach (var fi in fieldInfos)
            {
                var columnName = fi.GetRawConstantValue().ToString();
                if (!dt.Columns.Contains(columnName))
                {
                    dt.Columns.Add(columnName, typeof(string));
                }
            }
            return dt;
        }

        /// <summary>
        /// 获取 in where 条件
        /// 针对 Oracle 查询中 in 参数值超过1000的处理方法
        /// </summary>
        /// <param name="dt">数据源 DataTable</param>
        /// <param name="columnName">DataTable 列名</param>
        /// <param name="keyName">数据库表列名</param>
        /// <returns></returns>
        public static string GetInWhere(DataTable dt, string columnName, string keyName)
        {
            var where = string.Empty;
            var inWhere = string.Empty;
            dt = dt.DefaultView.ToTable(true, columnName);
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                var key = Convert.ToString(dt.Rows[i][columnName]);
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                if (i == dt.Rows.Count - 1)
                {
                    inWhere += $"'{key}'";
                }
                else if (i > 0 && i % 999 == 0)
                {
                    inWhere += $"'{key}') OR {keyName} IN (";
                }
                else
                {
                    inWhere += $"'{key}',";
                }
            }
            where += $" {keyName} IN ({inWhere}) ";
            return where;
        }

        /// <summary>
        /// 获取 in where 条件
        /// 针对 Oracle 查询中 in 参数值超过1000的处理方法
        /// </summary>
        /// <param name="list">列值列表</param>
        /// <param name="columnName">列名(带表名)</param>
        /// <returns></returns>
        public static string GetInWhere(List<string> list, string columnName)
        {
            var where = string.Empty;
            var inWhere = string.Empty;
            list = list.Distinct().ToList();
            for (int i = 0; i < list.Count; i++)
            {
                var key = list[i];
                if (string.IsNullOrEmpty(key))
                {
                    continue;
                }
                if (i == list.Count - 1)
                {
                    inWhere += $"'{key}'";
                }
                else if (i > 0 && i % 999 == 0)
                {
                    inWhere += $"'{key}') OR {columnName} IN (";
                }
                else
                {
                    inWhere += $"'{key}',";
                }
            }
            where += $" {columnName} IN ({inWhere}) ";
            return where;
        }

        /// <summary>
        /// IList 转 DataTable
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        public static DataTable IListToDataTable<T>(List<T> list)
        {
            DataTable dt = CreateEmptyDataTableFromProperties<T>();
            foreach (var item in list)
            {
                DataRow dr = dt.NewRow();
                foreach (var column in dt.Columns)
                {                    
                    dr[column.ToString()] = item.GetType().GetProperty(column.ToString()).GetValue(item);                    
                }
                dt.Rows.Add(dr);
            }
            return dt;
        }
    }
}
