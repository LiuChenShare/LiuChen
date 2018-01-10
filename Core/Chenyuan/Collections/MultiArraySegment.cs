using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Collections
{
    [Serializable]
    public struct MultiArraySegment<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyList<T>, IReadOnlyCollection<T>
    {
        private T[] _array;
        private int _offset;
        private int _count;

        public T this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public int Offset
        {
            get
            {
                return _offset;
            }
        }

        public int Count
        {
            get
            {
                return _count;
            }
        }

        public T[] Clone(int offset, int count)
        {
            return null;
        }

        public MultiArraySegment(T[] array, params T[] arrays)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            _array = array;
            _offset = 0;
            _count = array.Length;
        }

        public MultiArraySegment(T[] array, int offset, int count, params T[] arrays)
        {
            if (array == null)
            {
                throw new ArgumentNullException("array");
            }
            //if (offset < 0)
            //{
            //    throw new ArgumentOutOfRangeException("offset", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            //}
            //if (count < 0)
            //{
            //    throw new ArgumentOutOfRangeException("count", Environment.GetResourceString("ArgumentOutOfRange_NeedNonNegNum"));
            //}
            //if (array.Length - offset < count)
            //{
            //    throw new ArgumentException(Environment.GetResourceString("Argument_InvalidOffLen"));
            //}
            _array = array;
            _offset = offset;
            _count = count;
        }

        public override int GetHashCode()
        {
            if (_array != null)
            {
                return _array.GetHashCode() ^ _offset ^ _count;
            }
            return 0;
        }

        public override bool Equals(object obj)
        {
            return obj is MultiArraySegment<T> && this.Equals((MultiArraySegment<T>)obj);
        }

        public bool Equals(MultiArraySegment<T> obj)
        {
            return obj._array == _array && obj._offset == _offset && obj._count == _count;
        }

        public static bool operator ==(MultiArraySegment<T> a, MultiArraySegment<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(MultiArraySegment<T> a, MultiArraySegment<T> b)
        {
            return !(a == b);
        }

        #region interface implements

        #region IReadOnlyList<T>

        T IReadOnlyList<T>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IReadOnlyCollection<T>

        int IReadOnlyCollection<T>.Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IList<T>

        T IList<T>.this[int index]
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICollection<T>

        int ICollection<T>.Count
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        bool ICollection<T>.IsReadOnly
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        void ICollection<T>.Add(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.Clear()
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Contains(T item)
        {
            throw new NotImplementedException();
        }

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable<T>

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region IEnumerable

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        int IList<T>.IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        bool ICollection<T>.Remove(T item)
        {
            throw new NotImplementedException();
        }

        void IList<T>.RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        #endregion

        #endregion

        [Serializable]
        private sealed class ArraySegmentEnumerator : IEnumerator<T>, IDisposable, IEnumerator
        {
            private T[] _array;
            private int _start;
            private int _end;
            private int _current;
            public T Current
            {
                get
                {
                    //if (_current < _start)
                    //{
                    //    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumNotStarted"));
                    //}
                    //if (_current >= _end)
                    //{
                    //    throw new InvalidOperationException(Environment.GetResourceString("InvalidOperation_EnumEnded"));
                    //}
                    return _array[_current];
                }
            }
            object IEnumerator.Current
            {
                get
                {
                    return this.Current;
                }
            }
            internal ArraySegmentEnumerator(ArraySegment<T> arraySegment)
            {
                //_array = arraySegment._array;
                //_start = arraySegment._offset;
                //_end = _start + arraySegment._count;
                //_current = _start - 1;
            }
            public bool MoveNext()
            {
                if (_current < _end)
                {
                    _current++;
                    return _current < _end;
                }
                return false;
            }
            void IEnumerator.Reset()
            {
                _current = _start - 1;
            }
            public void Dispose()
            {
            }
        }

    }
}
