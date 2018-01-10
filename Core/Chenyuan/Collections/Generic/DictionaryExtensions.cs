using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Chenyuan.Collections.Generic
{
	[EditorBrowsable(EditorBrowsableState.Never)]
	public static class DictionaryExtensions
	{
		public static void RemoveFromDictionary<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, bool> removeCondition)
		{
			dictionary.RemoveFromDictionary((KeyValuePair<TKey, TValue> entry, Func<KeyValuePair<TKey, TValue>, bool> innerCondition) => innerCondition(entry), removeCondition);
		}
		public static void RemoveFromDictionary<TKey, TValue, TState>(this IDictionary<TKey, TValue> dictionary, Func<KeyValuePair<TKey, TValue>, TState, bool> removeCondition, TState state)
		{
			int num = 0;
			TKey[] array = new TKey[dictionary.Count];
			foreach (KeyValuePair<TKey, TValue> current in dictionary)
			{
				if (removeCondition(current, state))
				{
					array[num] = current.Key;
					num++;
				}
			}
			for (int i = 0; i < num; i++)
			{
				dictionary.Remove(array[i]);
			}
		}
		public static bool TryGetValue<T>(this IDictionary<string, object> collection, string key, out T value)
		{
			object obj;
			if (collection.TryGetValue(key, out obj) && obj is T)
			{
				value = (T)((object)obj);
				return true;
			}
			value = default(T);
			return false;
		}
		internal static IEnumerable<KeyValuePair<string, TValue>> FindKeysWithPrefix<TValue>(this IDictionary<string, TValue> dictionary, string prefix)
		{
			TValue value;
			if (dictionary.TryGetValue(prefix, out value))
			{
				yield return new KeyValuePair<string, TValue>(prefix, value);
			}
			foreach (KeyValuePair<string, TValue> current in dictionary)
			{
				KeyValuePair<string, TValue> keyValuePair = current;
				string key = keyValuePair.Key;
				if (key.Length > prefix.Length && key.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
				{
					if (prefix.Length == 0)
					{
						yield return current;
					}
					else
					{
						char c = key[prefix.Length];
						char c2 = c;
						if (c2 == '.' || c2 == '[')
						{
							yield return current;
						}
					}
				}
			}
			yield break;
		}
	}
}
