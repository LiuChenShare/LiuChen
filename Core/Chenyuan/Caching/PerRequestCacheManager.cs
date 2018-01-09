using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Caching
{
	/// <summary>
	/// Represents a manager for caching during an HTTP request (short term caching)
	/// </summary>
	public partial class PerRequestCacheManager : ICacheManager
	{
		private readonly HttpContextBase _context;

		/// <summary>
		/// Ctor
		/// </summary>
		/// <param name="context">Context</param>
		public PerRequestCacheManager(HttpContextBase context)
		{
			_context = context;
		}

		/// <summary>
		/// Creates a new instance of the NopRequestCache class
		/// </summary>
		protected virtual IDictionary GetItems()
		{
			if (_context != null)
				return _context.Items;

			return null;
		}

		/// <summary>
		/// Gets or sets the value associated with the specified key.
		/// </summary>
		/// <typeparam name="T">Type</typeparam>
		/// <param name="key">The key of the value to get.</param>
		/// <returns>The value associated with the specified key.</returns>
		public virtual T Get<T>(string key)
		{
			var items = GetItems();
			if (items == null)
				return default(T);

			return (T)items[key];
		}

		/// <summary>
		/// Adds the specified key and object to the cache.
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="data">Data</param>
		/// <param name="cacheTime">Cache time</param>
		protected virtual void Set(string key, object data, int cacheTime)
		{
			var items = GetItems();
			if (items == null)
				return;

			if (data != null)
			{
				if (items.Contains(key))
					items[key] = data;
				else
					items.Add(key, data);
			}
		}

		/// <summary>
		/// Removes the value with the specified key from the cache
		/// </summary>
		/// <param name="key">/key</param>
		public virtual void Remove(string key)
		{
			var items = GetItems();
			if (items == null)
				return;

			items.Remove(key);
		}

		/// <summary>
		/// Removes items by pattern
		/// </summary>
		/// <param name="pattern">pattern</param>
		public virtual void RemoveByPattern(string pattern)
		{
			var items = GetItems();
			if (items == null)
				return;

			var enumerator = items.GetEnumerator();
			var regex = new Regex(pattern, RegexOptions.Singleline | RegexOptions.Compiled | RegexOptions.IgnoreCase);
			var keysToRemove = new List<String>();
			while (enumerator.MoveNext())
			{
				if (regex.IsMatch(enumerator.Key.ToString()))
				{
					keysToRemove.Add(enumerator.Key.ToString());
				}
			}

			foreach (string key in keysToRemove)
			{
				items.Remove(key);
			}
		}

		/// <summary>
		/// Clear all cache data
		/// </summary>
		public virtual void Clear()
		{
			var items = GetItems();
			if (items == null)
				return;

			var enumerator = items.GetEnumerator();
			var keysToRemove = new List<String>();
			while (enumerator.MoveNext())
			{
				keysToRemove.Add(enumerator.Key.ToString());
			}

			foreach (string key in keysToRemove)
			{
				items.Remove(key);
			}
		}

		/// <summary>
		/// Variable (lock) to support thread-safe
		/// </summary>
		private static readonly object _syncObject = new object();

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
			lock (_syncObject)
			{
				if (this.Contains(key))
				{
					return this.Get<T>(key);
				}

				var result = acquirer();
				if (cacheTime > 0)
					this.Set(key, result, cacheTime);
				return result;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			var items = GetItems();
			if (items == null)
				return false;

			return (items[key] != null);
		}

		void ICacheManager.Set(string key, object data, int cacheTime)
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
