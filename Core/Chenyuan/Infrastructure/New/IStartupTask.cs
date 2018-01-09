using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// 任务启动接口定义
	/// </summary>
	public interface IStartupTask
	{
		/// <summary>
		/// 执行任务
		/// </summary>
		void Execute();

		/// <summary>
		/// 任务顺序
		/// </summary>

		int Order { get; }
	}
}
