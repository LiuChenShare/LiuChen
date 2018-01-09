using Chenyuan.Extensions;
using Chenyuan.Utilities;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Collections
{
    /// <summary>
    /// A data structure that contains multiple values for each key.
    /// </summary>
    /// <typeparam name="TKey">The type of key.</typeparam>
    /// <typeparam name="TValue">The type of value.</typeparam>
    public class Multimap<TKey, TValue> : IEnumerable<KeyValuePair<TKey, IList<TValue>>>
    {
        private readonly IDictionary<TKey, IList<TValue>> _items;
        private readonly Func<IList<TValue>> _listCreator;
        private readonly bool _isReadonly = false;

        /// <summary>
        /// 
        /// </summary>
        public Multimap()
            : this(false)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="threadSafe"></param>
        internal Multimap(bool threadSafe)
        {
            if (threadSafe)
            {
                _items = new ConcurrentDictionary<TKey, IList<TValue>>();
                _listCreator = () => new ThreadSafeList<TValue>();
            }
            else
            {
                _items = new Dictionary<TKey, IList<TValue>>();
                _listCreator = () => new List<TValue>();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listCreator"></param>
        public Multimap(Func<IList<TValue>> listCreator)
            : this(new Dictionary<TKey, IList<TValue>>(), listCreator)
        {
        }

        internal Multimap(IDictionary<TKey, IList<TValue>> dictionary, Func<IList<TValue>> listCreator)
        {
            _items = dictionary;
            _listCreator = listCreator;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="isReadonly"></param>
        protected Multimap(IDictionary<TKey, IList<TValue>> dictionary, bool isReadonly)
        {
            Assert.NotNull(dictionary, nameof(dictionary));

            _items = dictionary;

            if (isReadonly && dictionary != null)
            {
                foreach (var kvp in dictionary)
                {
                    dictionary[kvp.Key] = kvp.Value.AsReadOnly();
                }
            }

            _isReadonly = isReadonly;
        }

        /// <summary>
        /// Gets the count of groups/keys.
        /// </summary>
        public int Count
        {
            get
            {
                return _items.Keys.Count;
            }
        }

        /// <summary>
        /// Gets the total count of items in all groups.
        /// </summary>
        public int TotalValueCount
        {
            get
            {
                return _items.Values.Sum(x => x.Count);
            }
        }

        /// <summary>
        /// Gets the collection of values stored under the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        public virtual IList<TValue> this[TKey key]
        {
            get
            {
                if (!_items.ContainsKey(key))
                {
                    if (!_isReadonly)
                        _items[key] = _listCreator();
                    else
                        return null;
                }

                return _items[key];
            }
        }

        /// <summary>
        /// Gets the collection of keys.
        /// </summary>
        public virtual ICollection<TKey> Keys
        {
            get
            {
                return _items.Keys;
            }
        }

        /// <summary>
        /// Gets the collection of collections of values.
        /// </summary>
        public virtual ICollection<IList<TValue>> Values
        {
            get
            {
                return _items.Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IEnumerable<TValue> Find(TKey key, Expression<Func<TValue, bool>> predicate)
        {
            Assert.NotNull(key, nameof(key));
            Assert.NotNull(predicate, nameof(predicate));

            if (_items.ContainsKey(key))
            {
                return _items[key].Where(predicate.Compile());
            }

            return Enumerable.Empty<TValue>();
        }

        /// <summary>
        /// Adds the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Add(TKey key, TValue value)
        {
            CheckNotReadonly();

            this[key].Add(value);
        }

        /// <summary>
        /// Adds the specified values to the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="values">The values.</param>
        public virtual void AddRange(TKey key, IEnumerable<TValue> values)
        {
            CheckNotReadonly();

            this[key].AddRange(values);
        }

        /// <summary>
        /// Removes the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if such a value existed and was removed; otherwise <c>false</c>.</returns>
        public virtual bool Remove(TKey key, TValue value)
        {
            CheckNotReadonly();

            if (!_items.ContainsKey(key))
                return false;

            bool result = _items[key].Remove(value);
            if (_items[key].Count == 0)
                _items.Remove(key);

            return result;
        }

        /// <summary>
        /// Removes all values for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>True</c> if any such values existed; otherwise <c>false</c>.</returns>
        public virtual bool RemoveAll(TKey key)
        {
            CheckNotReadonly();
            return _items.Remove(key);
        }

        /// <summary>
        /// Removes all values.
        /// </summary>
        public virtual void Clear()
        {
            CheckNotReadonly();
            _items.Clear();
        }

        /// <summary>
        /// Determines whether the multimap contains any values for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns><c>True</c> if the multimap has one or more values for the specified key, otherwise <c>false</c>.</returns>
        public virtual bool ContainsKey(TKey key)
        {
            return _items.ContainsKey(key);
        }

        /// <summary>
        /// Determines whether the multimap contains the specified value for the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns><c>True</c> if the multimap contains such a value; otherwise, <c>false</c>.</returns>
        public virtual bool ContainsValue(TKey key, TValue value)
        {
            return _items.ContainsKey(key) && _items[key].Contains(value);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the multimap.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the multimap.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns an enumerator that iterates through the multimap.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> object that can be used to iterate through the multimap.</returns>
        public virtual IEnumerator<KeyValuePair<TKey, IList<TValue>>> GetEnumerator()
        {
            foreach (KeyValuePair<TKey, IList<TValue>> pair in _items)
                yield return pair;
        }

        private void CheckNotReadonly()
        {
            if (_isReadonly)
                throw new NotSupportedException("Multimap is read-only.");
        }

        #region Static members

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static Multimap<TKey, TValue> ThreadSafe()
        {
            return new Multimap<TKey, TValue>(true);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Multimap<TKey, TValue> CreateFromLookup(ILookup<TKey, TValue> source)
        {
            Assert.NotNull(source, nameof(source));

            var map = new Multimap<TKey, TValue>();

            foreach (IGrouping<TKey, TValue> group in source)
            {
                map.AddRange(group.Key, group);
            }

            return map;
        }

        #endregion
    }
}
