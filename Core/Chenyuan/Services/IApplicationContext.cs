using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Data;

namespace Chenyuan.Services
{
	/// <summary>
	/// 应用上下文
	/// </summary>
	public interface IApplicationContext
	{
		/// <summary>
		/// 获取当前应用对象
		/// </summary>
		IApplicationIdentity CurrentApplicatin
		{
			get;
		}

		/// <summary>
		/// IsSingleAppMode ? Guid.Empty : CurrentApplicatin.Id
		/// </summary>
		Guid CurrentAppIdIfMultiAppMode { get; }
	}
}
