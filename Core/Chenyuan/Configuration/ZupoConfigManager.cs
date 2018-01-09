using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;
using Chenyuan.IO;
using Chenyuan.IO;

namespace Chenyuan.Configuration
{
	/// <summary>
	/// Chenyuan自定义配置文件读取类
	/// </summary>
	public class ChenyuanConfigManager
	{
		private const string DEFAULTLOGACTIVITYCONFIG = "LogActivityConfig";
		/// <summary>
		/// 
		/// </summary>
		public static string LogActivityConfigKey = null;

		private static string GetLogActivityCOnfigKey()
		{
			if (LogActivityConfigKey.HasValue())
			{
				return LogActivityConfigKey;
			}
			return DEFAULTLOGACTIVITYCONFIG;
		}

		/// <summary>
		/// 加载LogActivityConfig配置内容
		/// </summary>
		public static LogActivityConfig LogActivityConfig
		{
			get
			{
				return GetConfigObj<LogActivityConfig>(GetLogActivityCOnfigKey()) ?? new LogActivityConfig();
			}
		}

		/// <summary>
		/// 加载指定类型的配置，默认先读Cache，Cache没有会直接加载.config文件读取，并写入Cache
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		protected static T GetConfigObj<T>(string name) where T : new()
		{
			var cache = ConfigDataCache.GetCache(name);
			if (!(cache is T))
			{
				var path = ConfigurationManager.AppSettings[name];
				if (!path.HasValue())
				{
					cache = new T();
				}
				else
				{
					string filePath = GetPath(path);
					cache = XmlHelper.LoadFromXML<T>(filePath);
					ConfigDataCache.SetCache(name, cache, filePath);
				}
			}
			return (T)cache;
		}

		/// <summary>
		/// 将config配置类对象写入对应的配置文件
		/// </summary>
		/// <typeparam name="T">配置类型</typeparam>
		/// <param name="name">appseting中配置的key名称</param>
		/// <param name="obj">配置对象</param>
		/// <param name="xmlRootName">xml文档跟节点名称</param>
		/// <returns></returns>
		protected static bool SetConfigObj<T>(string name, T obj, string xmlRootName)
		{
			var path = ConfigurationManager.AppSettings[name];
			var filePath = GetPath(path);
			if (File.Exists(filePath))
			{
				XmlHelper.SaveToXml(filePath, obj, typeof(T), xmlRootName);
			}

			return true;
		}

		private static string GetPath(string path)
		{
			string filePath = null;
			if (path.StartsWith("~"))
			{
				filePath = EngineContext.Current.Resolve<IWebHelper>().MapPath(path);
			}
			else
			{
				if (path.StartsWith("/") || path.StartsWith("\\"))
				{
					path = path.Substring(1);
				}
				filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, path);
			}
			return filePath;
		}
	}
}
