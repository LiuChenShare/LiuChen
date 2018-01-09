using System;

namespace Chenyuan.Caching.Defaults
{
    internal class CacheKey
    {
        protected const byte BitPublic = 32;
        protected const byte BitOutputCache = 64;
        protected string _key;
        protected byte _bits;
        private int _hashCode;
        internal string Key
        {
            get
            {
                return _key;
            }
        }
        internal bool IsOutputCache
        {
            get
            {
                return (_bits & 64) > 0;
            }
        }
        internal bool IsPublic
        {
            get
            {
                return (_bits & 32) > 0;
            }
        }
        internal CacheKey(string key, bool isPublic)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }
            _key = key;
            if (isPublic)
            {
                _bits = 32;
                return;
            }
            if (key[0] == "a"[0])
            {
                _bits |= 64;
            }
        }
        public override int GetHashCode()
        {
            if (_hashCode == 0)
            {
                _hashCode = _key.GetHashCode();
            }
            return _hashCode;
        }
    }
}
