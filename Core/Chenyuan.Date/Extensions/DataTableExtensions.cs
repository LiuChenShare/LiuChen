using Chenyuan.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date.Extensions
{
    /// <summary>
	/// DataTable数据对象转换
	/// </summary>
	public static class DataTableExtensions
    {
        /// <summary>
        /// Fill DataTable to List.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<T> FillEntity<T>(this DataTable dt) where T : new()
        {
            List<T> list = new List<T>();
            foreach (DataRow dr in dt.Rows)
            {
                var obj = dr.FillEntity<T>();
                list.Add(obj);
            }
            return list;
        }
        /// <summary>
        /// Fill DataTable to List.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<dynamic> FillEntity(this DataTable dt, Type type)
        {
            List<dynamic> list = new List<dynamic>();
            foreach (DataRow dr in dt.Rows)
            {
                var obj = dr.FillEntity(type);
                list.Add(obj);
            }
            return list;
        }
        /// <summary>
        /// Fill DataTable to List.
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static List<dynamic> FillEntity(this DataTable dt)
        {
            List<dynamic> list = new List<dynamic>();
            foreach (DataRow dr in dt.Rows)
            {
                var obj = dr.FillEntity();
                list.Add(obj);
            }
            return list;
        }
        /// <summary>
        /// Fill DataRow to object.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="dr"></param>
        /// <param name="parentName"></param>
        /// <returns></returns>
        public static dynamic FillEntity(this DataRow dr, Type type, string parentName = null)
        {
            var obj = type.Assembly.CreateInstance(type.FullName);
            var properties = type.GetProperties();
            var columns = dr.Table.Columns;
            bool searchSub = false;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ColumnName.IndexOf(".") != -1)
                {
                    searchSub = true;
                    break;
                }
            }
            foreach (var p in properties)
            {
                string columnname = string.IsNullOrEmpty(parentName) ? p.Name : $"{parentName}.{p.Name}";

                if (searchSub && !p.PropertyType.IsValueType && p.PropertyType.IsClass && p.PropertyType.Namespace.StartsWith("Zupo"))
                {
                    var curValue = dr.FillEntity(p.PropertyType, columnname);
                    p.SetValue(obj, curValue);
                }
                else
                {
                    if (p.Attributes.IsAttributeDefined<ColumnAttribute>(true))
                    {
                        columnname = p.GetAttribute<ColumnAttribute>(true).ColumnName;
                    }
                    if (columns.Contains(columnname))
                    {
                        Type underlyType = Nullable.GetUnderlyingType(p.PropertyType);
                        if (dr[columnname].GetType() == typeof(DBNull))
                        {
                            p.SetValue(obj, null);
                        }
                        else if (underlyType != null && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            if (underlyType.IsEnum)
                            {
                                p.SetValue(obj, Enum.Parse(underlyType, dr[columnname].ToString()));
                            }
                            else
                            {
                                p.SetValue(obj, Convert.ChangeType(dr[columnname], underlyType));
                            }
                        }
                        else
                        {
                            if (p.PropertyType.IsEnum)
                            {
                                p.SetValue(obj, Enum.Parse(p.PropertyType, dr[columnname].ToString()));
                            }
                            else
                            {
                                p.SetValue(obj, Convert.ChangeType(dr[columnname], p.PropertyType));
                            }
                        }
                    }
                }

            }
            return obj;
        }

        /// <summary>
        /// Fill DataRow to object.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static T FillEntity<T>(this DataRow dr, string parentName = null) where T : new()
        {
            var obj = new T();
            var properties = typeof(T).GetProperties();
            var columns = dr.Table.Columns;
            bool searchSub = false;
            for (int i = 0; i < columns.Count; i++)
            {
                if (columns[i].ColumnName.IndexOf(".") != -1)
                {
                    searchSub = true;
                    break;
                }
            }
            foreach (var p in properties)
            {
                string columnname = string.IsNullOrEmpty(parentName) ? p.Name : $"{parentName}.{p.Name}";
                if (searchSub && !p.PropertyType.IsValueType && p.PropertyType.IsClass && p.PropertyType.Namespace.StartsWith("Zupo"))
                {
                    var curValue = dr.FillEntity(p.PropertyType, columnname);
                    p.SetValue(obj, curValue);
                }
                else if (searchSub && p.PropertyType.Name == "IList`1")
                {
                    var curValue = dr.FillEntity(p.PropertyType.GetGenericArguments()[0], columnname);
                    (p.GetValue(obj) as IList).Add(curValue);
                }
                else
                {
                    if (p.Attributes.IsAttributeDefined<ColumnAttribute>(true))
                    {
                        columnname = p.GetAttribute<ColumnAttribute>(true).ColumnName;
                    }
                    if (columns.Contains(columnname))
                    {
                        Type underlyType = Nullable.GetUnderlyingType(p.PropertyType);
                        if (dr[columnname].GetType() == typeof(DBNull))
                        {
                            p.SetValue(obj, null);
                        }
                        else if (underlyType != null && p.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            if (underlyType.IsEnum)
                            {
                                p.SetValue(obj, Enum.Parse(underlyType, dr[columnname].ToString()));
                            }
                            else
                            {
                                p.SetValue(obj, Convert.ChangeType(dr[columnname], underlyType));
                            }
                        }
                        else
                        {
                            if (p.PropertyType.IsEnum)
                            {
                                p.SetValue(obj, Enum.Parse(p.PropertyType, dr[columnname].ToString()));
                            }
                            else
                            {
                                if (p.PropertyType == typeof(Boolean) && dr[columnname].ToString() == "")
                                {
                                    dr[columnname] = false;
                                }
                                p.SetValue(obj, Convert.ChangeType(dr[columnname], p.PropertyType));
                            }
                        }
                    }
                }
            }
            return obj;
        }

        /// <summary>
        /// Fill DataRow to dynamic.
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static dynamic FillEntity(this DataRow dr)
        {
            dynamic obj = new ExpandoObject();
            var dic = (IDictionary<string, object>)obj;
            var columns = dr.Table.Columns;
            foreach (DataColumn c in columns)
            {
                dic[c.ColumnName] = dr[c.ColumnName];
            }

            return obj;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dt"></param>
        /// <param name="tablename"></param>
        /// <param name="connectionString"></param>
        public static void BatchInsert<T>(this DataTable dt, string tablename, string connectionString)
        {
            var dtColum = dt.Columns;
            //声明SqlBulkCopy ,using释放非托管资源  

            using (SqlBulkCopy sqlBC = new SqlBulkCopy(connectionString))
            {
                //一次批量的插入的数据量  
                sqlBC.BatchSize = 10000;
                //超时之前操作完成所允许的秒数，如果超时则事务不会提交 ，数据将回滚，所有已复制的行都会从目标表中移除  
                sqlBC.BulkCopyTimeout = 300;
                //設定 NotifyAfter 属性，以便在每插入10000 条数据时，呼叫相应事件。   
                sqlBC.NotifyAfter = 10000;
                // sqlBC.SqlRowsCopied += new SqlRowsCopiedEventHandler(OnSqlRowsCopied);  
                //设置要批量写入的表  
                sqlBC.DestinationTableName = tablename;
                //自定义的datatable和数据库的字段进行对应
                var properties = typeof(T).GetProperties();
                for (int i = 0; i < dtColum.Count; i++)
                {
                    if (properties.Exists(x => x.Name == dtColum[i].ColumnName))
                    {
                        sqlBC.ColumnMappings.Add(dtColum[i].ColumnName.ToString(), dtColum[i].ColumnName.ToString());
                    }
                }
                //批量写入  
                sqlBC.WriteToServer(dt);
                sqlBC.Close();
            }

        }
    }
}
