namespace Chenyuan.Caching.Defaults
{
    internal struct ExpiresEntryRef
    {
        internal static readonly ExpiresEntryRef INVALID = new ExpiresEntryRef(0, 0);
        private const uint ENTRY_MASK = 255u;
        private const uint PAGE_MASK = 4294967040u;
        private const int PAGE_SHIFT = 8;
        private uint _ref;
        internal int PageIndex
        {
            get
            {
                return (int)(_ref >> 8);
            }
        }
        internal int Index
        {
            get
            {
                return (int)(_ref & 255u);
            }
        }
        internal bool IsInvalid
        {
            get
            {
                return _ref == 0u;
            }
        }
        internal ExpiresEntryRef(int pageIndex, int entryIndex)
        {
            _ref = (uint)(pageIndex << 8 | (entryIndex & 255));
        }
        public override bool Equals(object value)
        {
            return value is ExpiresEntryRef && _ref == ((ExpiresEntryRef)value)._ref;
        }
        public static bool operator !=(ExpiresEntryRef r1, ExpiresEntryRef r2)
        {
            return r1._ref != r2._ref;
        }
        public static bool operator ==(ExpiresEntryRef r1, ExpiresEntryRef r2)
        {
            return r1._ref == r2._ref;
        }
        public override int GetHashCode()
        {
            return (int)_ref;
        }
    }
}
