using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Data;
using Chenyuan.Extensions;

namespace Chenyuan.Messages
{
	/// <summary>
	/// 
	/// </summary>
	public class TokenCollection : KeyedCollection<string, Token>
	{
		private static StringComparer TranslateComparer(StringComparison stringComparison)
		{
			switch (stringComparison)
			{
				case StringComparison.CurrentCulture:
					return StringComparer.CurrentCulture;
				case StringComparison.CurrentCultureIgnoreCase:
					return StringComparer.CurrentCultureIgnoreCase;
				case StringComparison.InvariantCulture:
					return StringComparer.InvariantCulture;
				case StringComparison.InvariantCultureIgnoreCase:
					return StringComparer.InvariantCultureIgnoreCase;
				case StringComparison.Ordinal:
					return StringComparer.Ordinal;
				case StringComparison.OrdinalIgnoreCase:
					return StringComparer.OrdinalIgnoreCase;
				default:
					return StringComparer.CurrentCultureIgnoreCase;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="keyComparison"></param>
		public TokenCollection(StringComparison keyComparison = StringComparison.CurrentCultureIgnoreCase)
			: base(TranslateComparer(keyComparison))
		{
			this.KeyComparison = keyComparison;
		}

		/// <summary>
		/// 键比较方式
		/// </summary>
		public StringComparison KeyComparison
		{
			get;
			private set;
		}

		/// <summary>
		/// 添加或者替换
		/// </summary>
		/// <param name="token"></param>
		public void AddOrReplace(Token token)
		{
			if (this.Contains(token))
			{
				this.Remove(token.Key);
			}
			this.Add(token);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="neverHtmlEncode"></param>
		public void AddOrReplace(string key, string value, bool neverHtmlEncode = false)
		{
			this.AddOrReplace(new Token(key, value, neverHtmlEncode));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="neverHtmlEncode"></param>
		public void AddOrReplace(string key, object value, bool neverHtmlEncode = false)
		{
			this.AddOrReplace(new Token(key, "{0}".FormatInvariant(value), neverHtmlEncode));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="item"></param>
		/// <returns></returns>
		protected override string GetKeyForItem(Token item)
		{
			return item.Key;
		}

		/// <summary>
		/// 级联加载数据
		/// </summary>
		/// <typeparam name="TEntity"></typeparam>
		/// <param name="entity"></param>
		/// <returns></returns>
		public TokenCollection LoadTokens<TEntity>(TEntity entity)
			where TEntity : BaseEntity
		{
			return entity.LoadTokens(this);
		}
	}
}
