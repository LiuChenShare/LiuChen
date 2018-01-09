using System;

namespace Chenyuan.Caching.Defaults
{
    internal abstract class CacheMemoryPressure
    {
        protected const int TERABYTE_SHIFT = 40;
        protected const long TERABYTE = 1099511627776L;
        protected const int GIGABYTE_SHIFT = 30;
        protected const long GIGABYTE = 1073741824L;
        protected const int MEGABYTE_SHIFT = 20;
        protected const long MEGABYTE = 1048576L;
        protected const int KILOBYTE_SHIFT = 10;
        protected const long KILOBYTE = 1024L;
        protected const int HISTORY_COUNT = 6;
        protected int _pressureHigh;
        protected int _pressureMiddle;
        protected int _pressureLow;
        protected int _i0;
        protected int[] _pressureHist;
        protected int _pressureTotal;
        protected int _pressureAvg;
        private static long s_totalPhysical;
        private static long s_totalVirtual;
        internal static long TotalPhysical
        {
            get
            {
                return s_totalPhysical;
            }
        }
        internal static long TotalVirtual
        {
            get
            {
                return s_totalVirtual;
            }
        }
        internal int PressureLast
        {
            get
            {
                return _pressureHist[_i0];
            }
        }
        internal int PressureAvg
        {
            get
            {
                return _pressureAvg;
            }
        }
        internal int PressureHigh
        {
            get
            {
                return _pressureHigh;
            }
        }
        internal int PressureLow
        {
            get
            {
                return _pressureLow;
            }
        }
        internal int PressureMiddle
        {
            get
            {
                return _pressureMiddle;
            }
        }
        static CacheMemoryPressure()
        {
            //UnsafeNativeMethods.MEMORYSTATUSEX mEMORYSTATUSEX = default(UnsafeNativeMethods.MEMORYSTATUSEX);
            //mEMORYSTATUSEX.Init();
            //if (UnsafeNativeMethods.GlobalMemoryStatusEx(ref mEMORYSTATUSEX) != 0)
            //{
            //    s_totalPhysical = mEMORYSTATUSEX.ullTotalPhys;
            //    s_totalVirtual = mEMORYSTATUSEX.ullTotalVirtual;
            //}
        }
        protected abstract int GetCurrentPressure();
        internal abstract int GetPercentToTrim(DateTime lastTrimTime, int lastTrimPercent);
        internal virtual void ReadConfig(CacheSection cacheSection)
        {
        }
        protected void InitHistory()
        {
            int currentPressure = this.GetCurrentPressure();
            _pressureHist = new int[6];
            for (int i = 0; i < 6; i++)
            {
                _pressureHist[i] = currentPressure;
                _pressureTotal += currentPressure;
            }
            _pressureAvg = currentPressure;
        }
        internal void Update()
        {
            int currentPressure = this.GetCurrentPressure();
            _i0 = (_i0 + 1) % 6;
            _pressureTotal -= _pressureHist[_i0];
            _pressureTotal += currentPressure;
            _pressureHist[_i0] = currentPressure;
            _pressureAvg = _pressureTotal / 6;
        }
        internal bool IsAboveHighPressure()
        {
            return this.PressureLast >= this.PressureHigh;
        }
        internal bool IsAboveMediumPressure()
        {
            return this.PressureLast > this.PressureMiddle;
        }
    }
}