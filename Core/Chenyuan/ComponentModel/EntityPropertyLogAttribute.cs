using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 实体属性日志特性类定义
	/// </summary>
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = true)]
	public class EntityPropertyLogAttribute : Attribute
	{
		/// <summary>
		/// 
		/// </summary>
		public EntityPropertyLogAttribute()
		{

		}

		/// <summary>
		/// 属性或元素类型
		/// </summary>
		public Type PropertyType
		{
			get;
			set;
		}

		/// <summary>
		/// 参考ID属性名。针对有关联的属性对象
		/// </summary>
		public string ReferenceIdPropertyName
		{
			get;
			set;
		}

		/// <summary>
		/// 是否是可枚举的对象
		/// </summary>
		public bool Enumerable
		{
			get;
			set;
		}
	}
}
