using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 模型属性接口定义
	/// </summary>
	public interface IModelAttribute
	{
		/// <summary>
		/// 模型名称
		/// </summary>
		string Name { get; }
	}
}
