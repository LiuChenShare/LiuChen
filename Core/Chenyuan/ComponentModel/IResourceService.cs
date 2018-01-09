using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ComponentModel
{
	/// <summary>
	/// 资源获取服务接口定义
	/// </summary>
	public interface IResourceService
	{
		/// <summary>
		/// 获取资源方法
		/// </summary>
		/// <param name="resourceKey">资源键值</param>
		/// <param name="languageId"></param>
		/// <returns></returns>
		string GetResource(string resourceKey, Guid? languageId = null);
	}
}
