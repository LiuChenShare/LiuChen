using System;

namespace Chenyuan.Caching.Defaults
{
    internal sealed class CacheMemoryTotalMemoryPressure : CacheMemoryPressure
    {
        private const int MIN_TOTAL_MEMORY_TRIM_PERCENT = 10;
        //private static readonly long TARGET_TOTAL_MEMORY_TRIM_INTERVAL_TICKS = (long)((ulong)-1294967296);
        internal long MemoryLimit
        {
            get
            {
                return (long)_pressureHigh;
            }
        }
        internal CacheMemoryTotalMemoryPressure()
        {
            throw new NotImplementedException();
            //long totalPhysical = CacheMemoryPressure.TotalPhysical;
            //if (totalPhysical >= 4294967296L)
            //{
            //    _pressureHigh = 99;
            //}
            //else
            //{
            //    if (totalPhysical >= (long)((ulong)-2147483648))
            //    {
            //        _pressureHigh = 98;
            //    }
            //    else
            //    {
            //        if (totalPhysical >= 1073741824L)
            //        {
            //            _pressureHigh = 97;
            //        }
            //        else
            //        {
            //            if (totalPhysical >= 805306368L)
            //            {
            //                _pressureHigh = 96;
            //            }
            //            else
            //            {
            //                _pressureHigh = 95;
            //            }
            //        }
            //    }
            //}
            //_pressureMiddle = _pressureHigh - 2;
            //_pressureLow = _pressureHigh - 9;
            //base.InitHistory();
            //PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED_BASE, _pressureHigh);
        }
        internal override void ReadConfig(CacheSection cacheSection)
        {
            throw new NotImplementedException();
            //int percentagePhysicalMemoryUsedLimit = cacheSection.PercentagePhysicalMemoryUsedLimit;
            //if (percentagePhysicalMemoryUsedLimit == 0)
            //{
            //    return;
            //}
            //_pressureHigh = Math.Max(3, percentagePhysicalMemoryUsedLimit);
            //_pressureMiddle = Math.Max(2, _pressureHigh - 2);
            //_pressureLow = Math.Max(1, _pressureHigh - 9);
            //PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED_BASE, _pressureHigh);
        }
        protected override int GetCurrentPressure()
        {
            throw new NotImplementedException();
            //UnsafeNativeMethods.MEMORYSTATUSEX mEMORYSTATUSEX = default(UnsafeNativeMethods.MEMORYSTATUSEX);
            //mEMORYSTATUSEX.Init();
            //if (UnsafeNativeMethods.GlobalMemoryStatusEx(ref mEMORYSTATUSEX) == 0)
            //{
            //    return 0;
            //}
            //int dwMemoryLoad = mEMORYSTATUSEX.dwMemoryLoad;
            //if (_pressureHigh != 0)
            //{
            //    PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_MACH_MEM_LIMIT_USED, dwMemoryLoad);
            //}
            //return dwMemoryLoad;
        }
        internal override int GetPercentToTrim(DateTime lastTrimTime, int lastTrimPercent)
        {
            int num = 0;
            //if (base.IsAboveHighPressure())
            //{
            //    long ticks = DateTime.UtcNow.Subtract(lastTrimTime).Ticks;
            //    if (ticks > 0L)
            //    {
            //        num = Math.Min(50, (int)((long)lastTrimPercent * CacheMemoryTotalMemoryPressure.TARGET_TOTAL_MEMORY_TRIM_INTERVAL_TICKS / ticks));
            //        num = Math.Max(10, num);
            //    }
            //}
            return num;
        }
    }
}