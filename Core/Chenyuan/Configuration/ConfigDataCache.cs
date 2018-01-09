using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Caching;

namespace Chenyuan.Configuration
{
	/// <summary>
	/// 缓存相关的操作类
	/// </summary>
	public class ConfigDataCache
	{
		/// <summary>
		/// 获取当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <returns></returns>
		public static object GetCache(string CacheKey)
		{
			Cache objCache = HttpRuntime.Cache;
			return objCache[CacheKey];
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="CacheKey"></param>
		/// <param name="objObject"></param>
		public static void SetCache(string CacheKey, object objObject)
		{
			Cache objCache = HttpRuntime.Cache;
			objCache.Insert(CacheKey, objObject);
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值,并具有文件依赖项
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="objObject"></param>
		/// <param name="filename"></param>
		public static void SetCache(string cacheKey, object objObject, string filename)
		{
			Cache objCache = HttpRuntime.Cache;
			objCache.Insert(cacheKey, objObject, new CacheDependency(filename));
		}

		/// <summary>
		/// 设置当前应用程序指定CacheKey的Cache值
		/// </summary>
		/// <param name="cacheKey"></param>
		/// <param name="objObject"></param>
		/// <param name="absoluteExpiration"></param>
		/// <param name="slidingExpiration"></param>
		public static void SetCache(string cacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
		{
			Cache objCache = HttpRuntime.Cache;
			objCache.Insert(cacheKey, objObject, null, absoluteExpiration, slidingExpiration);
		}
	}
}
