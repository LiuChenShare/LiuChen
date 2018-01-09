using Chenyuan.Utilities.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Chenyuan.Collections
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class ThreadSafeList<T> : IList<T>, IList
    {
        #region Fields

        private List<T> _list;

        [NonSerialized]
        private readonly ReaderWriterLockSlim _rwLock;

        #endregion

        #region Ctor

        /// <summary>
        /// 
        /// </summary>
        public ThreadSafeList()
            : this(new ReaderWriterLockSlim())
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="rwLock"></param>
        public ThreadSafeList(ReaderWriterLockSlim rwLock)
        {
            _list = new List<T>();
            _rwLock = rwLock ?? new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        public ThreadSafeList(int capacity)
            : this(capacity, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="capacity"></param>
        /// <param name="rwLock"></param>
        public ThreadSafeList(int capacity, ReaderWriterLockSlim rwLock)
        {
            _list = new List<T>(capacity);
            _rwLock = rwLock ?? new ReaderWriterLockSlim();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        public ThreadSafeList(IEnumerable<T> collection)
            : this(collection, null)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="rwLock"></param>
        public ThreadSafeList(IEnumerable<T> collection, ReaderWriterLockSlim rwLock)
        {
            _list = new List<T>(collection);
            _rwLock = rwLock ?? new ReaderWriterLockSlim();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        public int Count
        {
            get
            {
                using (_rwLock.GetReadLock())
                {
                    return _list.Count;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index]
        {
            get
            {
                using (_rwLock.GetReadLock())
                {
                    return _list[index];
                }
            }
            set
            {
                using (_rwLock.GetWriteLock())
                {
                    _list[index] = value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public ReaderWriterLockSlim Lock
        {
            get
            {
                return _rwLock;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public void Add(T item)
        {
            using (_rwLock.GetWriteLock())
            {
                _list.Add(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public void Clear()
        {
            using (_rwLock.GetWriteLock())
            {
                _list.Clear();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
        {
            using (_rwLock.GetReadLock())
            {
                return _list.Contains(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="arrayIndex"></param>
        public void CopyTo(T[] array, int arrayIndex)
        {
            using (_rwLock.GetWriteLock())
            {
                _list.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
        {
            using (_rwLock.GetReadLock())
            {
                return _list.AsReadOnly().GetEnumerator();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public int IndexOf(T item)
        {
            using (_rwLock.GetReadLock())
            {
                return _list.IndexOf(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        /// <param name="item"></param>
        public void Insert(int index, T item)
        {
            using (_rwLock.GetWriteLock())
            {
                _list.Insert(index, item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Remove(T item)
        {
            using (_rwLock.GetWriteLock())
            {
                return _list.Remove(item);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="index"></param>
        public void RemoveAt(int index)
        {
            using (_rwLock.GetWriteLock())
            {
                _list.RemoveAt(index);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        private static void VerifyValueType(object value)
        {
            if (!IsCompatibleObject(value))
            {
                throw Error.Argument("value", "Argument '{0}' is of wrong type. It must be '{1}'.", "value", typeof(T));
            }
        }

        private static bool IsCompatibleObject(object value)
        {
            if (!(value is T) && ((value != null) || typeof(T).IsValueType))
            {
                return false;
            }
            return true;
        }

        #endregion

        #region IList Members

        int IList.Add(object value)
        {
            VerifyValueType(value);
            this.Add((T)value);
            return this.Count - 1;
        }

        bool IList.Contains(object value)
        {
            return (IsCompatibleObject(value) && this.Contains((T)value));
        }

        int IList.IndexOf(object value)
        {
            if (IsCompatibleObject(value))
            {
                return this.IndexOf((T)value);
            }
            return -1;
        }

        void IList.Insert(int index, object value)
        {
            VerifyValueType(value);
            this.Insert(index, (T)value);
        }

        bool IList.IsFixedSize
        {
            get
            {
                return false;
            }
        }

        void IList.Remove(object value)
        {
            if (IsCompatibleObject(value))
            {
                this.Remove((T)value);
            }
        }

        object IList.this[int index]
        {
            get
            {
                return this[index];
            }
            set
            {
                VerifyValueType(value);
                this[index] = (T)value;
            }
        }

        #endregion

        #region ICollection Members

        void ICollection.CopyTo(Array array, int index)
        {
            using (_rwLock.GetWriteLock())
            {
                ((ICollection)_list).CopyTo(array, index);
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public object SyncRoot
        {
            get
            {
                return ((ICollection)_list).SyncRoot;
            }
        }

        #endregion
    }
}
