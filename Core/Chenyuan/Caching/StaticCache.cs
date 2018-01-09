using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Caching.Defaults;
using Chenyuan.Extensions;

namespace Chenyuan.Caching
{
	/// <summary>
	/// 
	/// </summary>
	public partial class StaticCache : ICache
	{
		private const string REGION_NAME = "$$Chenyuan100NET.Static$$";
		private readonly static object s_lock = new object();

		/// <summary>
		/// 
		/// </summary>
		protected ObjectCache Cache
		{
			get
			{
				return MemoryCache.Default;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<KeyValuePair<string, object>> Entries
		{
			get
			{
				return from entry in Cache
					   where entry.Key.StartsWith(REGION_NAME)
					   select new KeyValuePair<string, object>(
						   entry.Key.Substring(REGION_NAME.Length),
						   entry.Value);
			}
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
			key = BuildKey(key);

			if (Cache.Contains(key))
			{
				return (T)Cache.Get(key);
			}
			else
			{
				lock (s_lock)
				{
					if (!Cache.Contains(key))
					{
						var value = acquirer();
						if (value != null)
						{
							var cacheItem = new CacheItem(key, value);
							CacheItemPolicy policy = null;
							if (cacheTime > 0)
							{
								policy = new CacheItemPolicy { AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime) };
							}

							Cache.Add(cacheItem, policy);
						}

						return value;
					}
				}

				return (T)Cache.Get(key);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			return Cache.Contains(BuildKey(key));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			Cache.Remove(BuildKey(key));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		private string BuildKey(string key)
		{
			return key.HasValue() ? REGION_NAME + key : null;
		}

	}
}
