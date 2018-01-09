using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Extensions;

namespace Chenyuan.Data
{
	/// <summary>
	/// 数据配置类定义
	/// </summary>
	public partial class DataSettings
	{
		/// <summary>
		/// 构造函数
		/// </summary>
		public DataSettings()
		{
			RawDataSettings = new Dictionary<string, string>(StringComparer.CurrentCultureIgnoreCase);
		}

		/// <summary>
		/// 获取数据提供者信息
		/// </summary>
		public string DataProvider { get; set; }

		/// <summary>
		/// 获取默认的数据库连接字符串
		/// </summary>
		public string DataConnectionString { get; set; }

		/// <summary>
		/// 获取额外扩展的数据库连接信息
		/// </summary>
		public IDictionary<string, string> RawDataSettings { get; private set; }

		/// <summary>
		/// 按键值检索数据配置信息
		/// </summary>
		/// <param name="key">键值，当键值为空或不存在于字典时，使用默认配置 DataConnectionString</param>
		/// <returns>目标配置数据</returns>
		public string this[string key]
		{
			get
			{
				if (key.HasValue())
				{
					switch (key.ToLower())
					{
						case "dataprovider":
							return this.DataProvider;
						case "dataconnectionstring":
							return this.DataConnectionString;
						default:
							if (RawDataSettings.ContainsKey(key))
							{
								return RawDataSettings[key];
							}
							return null;
					}
				}
				return this.DataConnectionString;
			}
		}

		/// <summary>
		/// 判断当前配置是否有效
		/// </summary>
		/// <returns></returns>
		public bool IsValid()
		{
			return !String.IsNullOrEmpty(this.DataProvider) && !String.IsNullOrEmpty(this.DataConnectionString);
		}
	}
}
