using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 实体类日志特性定义
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
	public class EntityLogAttribute : Attribute
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		/// <param name="autoLog">是否自动记录日志</param>
		/// <remarks>
		///     当自动记录日志时，系统会自动执行日志更新操作，否则，需要手动执行日志数据的更新
		/// </remarks>
		public EntityLogAttribute(bool autoLog = true)
		{
			this.AutoLog = autoLog;
		}

		/// <summary>
		/// 是否自动记录日志
		/// </summary>
		/// <remarks>
		///     当自动记录日志时，系统会自动执行日志更新操作，否则，需要手动执行日志数据的更新
		/// </remarks>
		public bool AutoLog
		{
			get;
			private set;
		}

	}
}
