using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;
using Chenyuan.Utilities;

namespace Chenyuan.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class ObjectValueDictionary : IDictionary<string, object>, ICollection<KeyValuePair<string, object>>, IEnumerable<KeyValuePair<string, object>>, IEnumerable
        , IDictionary, ICollection, IReadOnlyDictionary<string, object>, IReadOnlyCollection<KeyValuePair<string, object>>, ISerializable, IDeserializationCallback
    {
        #region 构造方法

        private Dictionary<string, object> _dictionary;

        /// <summary>
        /// 
        /// </summary>
        public ObjectValueDictionary()
        {
            _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        public ObjectValueDictionary(IDictionary<string, object> dictionary)
        {
            _dictionary = new Dictionary<string, object>(dictionary, StringComparer.OrdinalIgnoreCase);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        public ObjectValueDictionary(params object[] values)
        {
            _dictionary = new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
            this.Merge(values);
        }

        /// <summary>
        /// 合并对象
        /// </summary>
        /// <param name="values"></param>
        public void Merge(params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                this.AddValues(values[i]);
            }
        }

        /// <summary>
        /// 创建字典
        /// </summary>
        /// <param name="values"></param>
        /// <returns></returns>
        public static ObjectValueDictionary CreateRouteValueDictionaryUncached(object values)
        {
            IDictionary<string, object> dictionary = values as IDictionary<string, object>;
            if (dictionary != null)
            {
                return new ObjectValueDictionary(dictionary);
            }
            return TypeHelper.ObjectToDictionaryUncached(values);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private void AddValues(object value)
        {
            if (value == null)
            {
                return;
            }
            if (value is IDictionary)
            {
                var dict = value as IDictionary;
                foreach (var key in dict.Keys)
                {
                    (_dictionary as IDictionary)[key.ToString()] = dict[key];
                }
            }
            else
            {
                foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(value))
                {
                    object obj2 = descriptor.GetValue(value);
                    this[descriptor.Name] = obj2;
                }
            }
        }

        #endregion

        #region 隐式实现接口

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void Add(string key, object value)
        {
            _dictionary.Add(key, value);
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            _dictionary.Clear();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool ContainsValue(object value)
        {
            return _dictionary.ContainsValue(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, object>.Enumerator GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public bool TryGetValue(string key, out object value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                return _dictionary.Count;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object this[string key]
        {
            get
            {
                object obj2;
                this.TryGetValue(key, out obj2);
                return obj2;
            }
            set
            {
                _dictionary[key] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object>.ValueCollection Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object>.KeyCollection Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        #endregion

        #region 显式实现接口

        #region IDictionary<string, object>

        ICollection<string> IDictionary<string, object>.Keys
        {
            get
            {
                return _dictionary.Keys;
            }
        }

        ICollection<object> IDictionary<string, object>.Values
        {
            get
            {
                return _dictionary.Values;
            }
        }

        #endregion

        #region IEnumerable<KeyValuePair<string, object>>

        IEnumerator<KeyValuePair<string, object>> IEnumerable<KeyValuePair<string, object>>.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #region ICollection<KeyValuePair<string, object>>

        /// <summary>
        /// 
        /// </summary>
        bool ICollection<KeyValuePair<string, object>>.IsReadOnly
        {
            get
            {
                return (_dictionary as ICollection<KeyValuePair<string, object>>).IsReadOnly;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        void ICollection<KeyValuePair<string, object>>.Add(KeyValuePair<string, object> item)
        {
            (_dictionary as ICollection<KeyValuePair<string, object>>).Add(item);
        }

        bool ICollection<KeyValuePair<string, object>>.Contains(KeyValuePair<string, object> item)
        {
            return (_dictionary as ICollection<KeyValuePair<string, object>>).Contains(item);
        }

        void ICollection<KeyValuePair<string, object>>.CopyTo(KeyValuePair<string, object>[] array, int arrayIndex)
        {
            (_dictionary as ICollection<KeyValuePair<string, object>>).CopyTo(array, arrayIndex);
        }

        bool ICollection<KeyValuePair<string, object>>.Remove(KeyValuePair<string, object> item)
        {
            return (_dictionary as ICollection<KeyValuePair<string, object>>).Remove(item);
        }

        #endregion

        #region IDeserializationCallback

        void IDeserializationCallback.OnDeserialization(object sender)
        {
            (_dictionary as IDeserializationCallback).OnDeserialization(sender);
        }

        #endregion

        #region ISerializable

        void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
        {
            (_dictionary as ISerializable).GetObjectData(info, context);
        }

        #endregion

        #region IReadOnlyDictionary<string, object>

        bool IReadOnlyDictionary<string, object>.ContainsKey(string key)
        {
            return (_dictionary as IReadOnlyDictionary<string, object>).ContainsKey(key);
        }

        IEnumerable<string> IReadOnlyDictionary<string, object>.Keys
        {
            get
            {
                return (_dictionary as IReadOnlyDictionary<string, object>).Keys;
            }
        }

        bool IReadOnlyDictionary<string, object>.TryGetValue(string key, out object value)
        {
            return (_dictionary as IReadOnlyDictionary<string, object>).TryGetValue(key, out value);
        }

        IEnumerable<object> IReadOnlyDictionary<string, object>.Values
        {
            get
            {
                return (_dictionary as IReadOnlyDictionary<string, object>).Values;
            }
        }

        object IReadOnlyDictionary<string, object>.this[string key]
        {
            get
            {
                return (_dictionary as IReadOnlyDictionary<string, object>)[key];
            }
        }

        #endregion

        #region IReadOnlyCollection<KeyValuePair<string, object>>

        int IReadOnlyCollection<KeyValuePair<string, object>>.Count
        {
            get
            {
                return (_dictionary as IReadOnlyCollection<KeyValuePair<string, object>>).Count;
            }
        }

        #endregion

        #region ICollection

        void ICollection.CopyTo(Array array, int index)
        {
            (_dictionary as ICollection).CopyTo(array, index);
        }

        int ICollection.Count
        {
            get
            {
                return (_dictionary as ICollection).Count;
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return (_dictionary as ICollection).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return (_dictionary as ICollection).SyncRoot;
            }
        }

        #endregion

        #region IDictionary实现

        void IDictionary.Add(object key, object value)
        {
            (_dictionary as IDictionary).Add(key, value);
        }

        void IDictionary.Clear()
        {
            (_dictionary as IDictionary).Clear();
        }

        bool IDictionary.Contains(object key)
        {
            return (_dictionary as IDictionary).Contains(key);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return (_dictionary as IDictionary).GetEnumerator();
        }

        bool IDictionary.IsFixedSize
        {
            get
            {
                return (_dictionary as IDictionary).IsFixedSize;
            }
        }

        bool IDictionary.IsReadOnly
        {
            get
            {
                return (_dictionary as IDictionary).IsReadOnly;
            }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                return this.Keys;
            }
        }

        void IDictionary.Remove(object key)
        {
            (_dictionary as IDictionary).Remove(key);
        }

        ICollection IDictionary.Values
        {
            get
            {
                return (_dictionary as IDictionary).Values;
            }
        }

        object IDictionary.this[object key]
        {
            get
            {
                return (_dictionary as IDictionary)[key];
            }
            set
            {
                (_dictionary as IDictionary)[key] = value;
            }
        }

        #endregion

        #endregion
    }
}
