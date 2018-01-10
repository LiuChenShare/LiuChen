using System;
using System.Collections.Concurrent;
using System.Linq;

namespace Chenyuan.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
	public abstract class FilterAttribute : Attribute, IFilter
	{
		private static readonly ConcurrentDictionary<Type, bool> _multiuseAttributeCache = new ConcurrentDictionary<Type, bool>();
		private int _order = -1;
		public bool AllowMultiple
		{
			get
			{
				return FilterAttribute.AllowsMultiple(base.GetType());
			}
		}

		public int Order
		{
			get
			{
				return _order;
			}
			set
			{
				if (value < -1)
				{
					throw new ArgumentOutOfRangeException(nameof(value), Resource.FilterAttribute_OrderOutOfRange);
				}
				_order = value;
			}
		}

		private static bool AllowsMultiple(Type attributeType)
		{
			return _multiuseAttributeCache.GetOrAdd(attributeType, (Type type) => type.GetCustomAttributes(typeof(AttributeUsageAttribute), true).Cast<AttributeUsageAttribute>().First().AllowMultiple);
		}
	}
}
