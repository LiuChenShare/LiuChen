using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.EF
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class BaseDataProviderManager
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="settings"></param>
		protected BaseDataProviderManager(DataSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");
			this.Settings = settings;
		}
		/// <summary>
		/// 
		/// </summary>
		protected DataSettings Settings { get; private set; }
		/// <summary>
		/// 
		/// </summary>
		/// <param name="providerName">服务提供者名称（数据库类型）</param>
		/// <returns></returns>
		public abstract IDataProvider LoadDataProvider(string providerName = null);
	}
}
