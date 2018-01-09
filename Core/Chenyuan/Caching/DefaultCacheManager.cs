using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chenyuan.Caching
{
	/// <summary>
	/// 
	/// </summary>
	public partial class DefaultCacheManager : ICacheManager
	{
		private readonly ICache _cache;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="cache"></param>
		public DefaultCacheManager(ICache cache)
		{
			_cache = cache;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="acquirer"></param>
		/// <param name="cacheTime"></param>
		/// <returns></returns>
		public T Get<T>(string key, Func<T> acquirer, int cacheTime = 60)
		{
			return _cache.Get(key, acquirer, cacheTime);
		}

		public T Get<T>(string key)
		{
			return default(T);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			return _cache.Contains(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			_cache.Remove(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="pattern"></param>
		public void RemoveByPattern(string pattern)
		{
			var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
			var keysToRemove = new List<String>();

			foreach (var item in _cache.Entries)
			{
				if (regex.IsMatch(item.Key))
				{
					keysToRemove.Add(item.Key);
				}
			}

			foreach (string key in keysToRemove)
			{
				_cache.Remove(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public void Clear()
		{
			var keysToRemove = new List<string>();
			foreach (var item in _cache.Entries)
			{
				keysToRemove.Add(item.Key);
			}

			foreach (string key in keysToRemove)
			{
				_cache.Remove(key);
			}
		}

		public void Set(string key, object data, int cacheTime)
		{
			throw new NotImplementedException();
		}

		public bool IsSet(string key)
		{
			throw new NotImplementedException();
		}

		public IList<string> GetAllKeys()
		{
			throw new NotImplementedException();
		}
	}
}
