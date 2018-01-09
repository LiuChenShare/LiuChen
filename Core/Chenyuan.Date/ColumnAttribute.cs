using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Date
{
    /// <summary>
	/// 列属性
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ColumnAttribute : Attribute
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        public ColumnAttribute(string name)
        {
            this.ColumnName = name;
        }
        /// <summary>
        /// 列名
        /// </summary>
        public string ColumnName { get; set; }
    }
}
