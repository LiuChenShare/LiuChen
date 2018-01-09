using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class MiscExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="exc"></param>
		public static void Dump(this Exception exc)
        {
            try
            {
                exc.StackTrace.Dump();
                exc.Message.Dump();
            }
            catch (Exception)
            {
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
		public static string ToElapsedMinutes(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalMinutes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="watch"></param>
        /// <returns></returns>
		public static string ToElapsedSeconds(this Stopwatch watch)
        {
            return "{0:0.0}".FormatWith(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds).TotalSeconds);
        }

        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      /// <param name="dv"></param>
        //      /// <param name="columnName"></param>
        //      /// <returns></returns>
        //public static bool HasColumn(this DataView dv, string columnName)
        //      {
        //          dv.RowFilter = "ColumnName='" + columnName + "'";
        //          return dv.Count > 0;
        //      }

        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      /// <param name="dt"></param>
        //      /// <param name="columnName"></param>
        //      /// <returns></returns>
        //public static string GetDataType(this DataTable dt, string columnName)
        //      {
        //          dt.DefaultView.RowFilter = "ColumnName='" + columnName + "'";
        //          return dt.Rows[0]["DataType"].ToString();
        //      }

        //      /// <summary>
        //      /// 
        //      /// </summary>
        //      /// <param name="conn"></param>
        //      /// <param name="sqChenyuanount"></param>
        //      /// <returns></returns>
        //public static int CountExecute(this OleDbConnection conn, string sqChenyuanount)
        //      {
        //          using (OleDbCommand cmd = new OleDbCommand(sqChenyuanount, conn))
        //          {
        //              return (int)cmd.ExecuteScalar();
        //          }
        //      }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static object SafeConvert(this TypeConverter converter, string value)
        {
            try
            {
                if (converter != null && value.HasValue() && converter.CanConvertFrom(typeof(string)))
                {
                    return converter.ConvertFromString(value);
                }
            }
            catch (Exception exc)
            {
                exc.Dump();
            }
            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="converter"></param>
        /// <param name="value"></param>
        /// <param name="compareWith"></param>
        /// <returns></returns>
		public static bool IsEqual(this TypeConverter converter, string value, object compareWith)
        {
            object convertedObject = converter.SafeConvert(value);

            if (convertedObject != null && compareWith != null)
                return convertedObject.Equals(compareWith);

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrDefault<T>(this T? value) where T : struct
        {
            return default(T).Equals(value.GetValueOrDefault());
        }
    }
}
