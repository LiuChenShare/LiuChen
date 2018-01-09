using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 显示资源属性定义
	/// </summary>
	public interface IResourceDisplayAttribute
	{
		/// <summary>
		/// 资源键值
		/// </summary>
		string ResourceKey { get; }

		/// <summary>
		/// 显示的名称
		/// </summary>
		string DisplayName { get; }

		/// <summary>
		/// 提示信息
		/// </summary>
		string Hint { get; }
	}
}
