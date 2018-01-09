using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Chenyuan.Extensions;

namespace Chenyuan.Caching
{
	/// <summary>
	/// 
	/// </summary>
	public partial class RequestCache : ICache
	{
		private const string REGION_NAME = "$$Chenyuan100NET.Request$$";
		private readonly HttpContextBase _context;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public RequestCache(HttpContextBase context)
		{
			_context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected IDictionary GetItems()
		{
			if (_context != null)
				return _context.Items;

			return null;
		}

		/// <summary>
		/// 
		/// </summary>
		public IEnumerable<KeyValuePair<string, object>> Entries
		{
			get
			{
				var items = GetItems();
				if (items == null)
					yield break;

				var enumerator = items.GetEnumerator();
				while (enumerator.MoveNext())
				{
					string key = enumerator.Key as string;
					if (key == null)
						continue;
					if (key.StartsWith(REGION_NAME))
					{
						yield return new KeyValuePair<string, object>(key.Substring(REGION_NAME.Length), enumerator.Value);
					}
				}
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
			var items = GetItems();
			if (items == null)
				return default(T);

			key = BuildKey(key);

			if (items.Contains(key))
			{
				return (T)items[key];
			}
			else
			{
				var value = acquirer();
				if (value != null)
				{
					items.Add(key, value);
				}

				return value;
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

			return items.Contains(BuildKey(key));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		public void Remove(string key)
		{
			var items = GetItems();
			if (items == null)
				return;

			items.Remove(BuildKey(key));
		}

		private string BuildKey(string key)
		{
			return key.HasValue() ? REGION_NAME + key : null;
		}

	}
}
