namespace Chenyuan.Caching.Defaults
{
    internal struct UsageEntryRef
    {
        internal static readonly UsageEntryRef INVALID = new UsageEntryRef(0, 0);
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
        internal int Ref1Index
        {
            get
            {
                return (int)((sbyte)(_ref & 255u));
            }
        }
        internal int Ref2Index
        {
            get
            {
                return (int)(-(int)((sbyte)(_ref & 255u)));
            }
        }
        internal bool IsRef1
        {
            get
            {
                return (sbyte)(_ref & 255u) > 0;
            }
        }
        internal bool IsRef2
        {
            get
            {
                return (sbyte)(_ref & 255u) < 0;
            }
        }
        internal bool IsInvalid
        {
            get
            {
                return _ref == 0u;
            }
        }
        internal UsageEntryRef(int pageIndex, int entryIndex)
        {
            _ref = (uint)(pageIndex << 8 | (entryIndex & 255));
        }
        public override bool Equals(object value)
        {
            return value is UsageEntryRef && _ref == ((UsageEntryRef)value)._ref;
        }
        public static bool operator ==(UsageEntryRef r1, UsageEntryRef r2)
        {
            return r1._ref == r2._ref;
        }
        public static bool operator !=(UsageEntryRef r1, UsageEntryRef r2)
        {
            return r1._ref != r2._ref;
        }
        public override int GetHashCode()
        {
            return (int)_ref;
        }
    }
}
