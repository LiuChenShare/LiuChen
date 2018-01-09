using System;
using System.Threading;

namespace Chenyuan.Caching.Defaults
{
    internal sealed class CacheMemorySizePressure : CacheMemoryPressure
    {
        private const long PRIVATE_BYTES_LIMIT_2GB = 838860800L;
        private const long PRIVATE_BYTES_LIMIT_3GB = 1887436800L;
        private const long PRIVATE_BYTES_LIMIT_64BIT = 1099511627776L;
        private const int SAMPLE_COUNT = 2;
        //private static bool s_isIIS6 = HostingEnvironment.IsUnderIIS6Process;
        private static long s_autoPrivateBytesLimit = -1L;
        private static uint s_pid = 0u;
        private static int s_pollInterval;
        private static long s_workerProcessMemoryLimit = -1L;
        private static long s_effectiveProcessMemoryLimit = -1L;
        private long _totalCacheSize;
        private long[] _cacheSizeSamples;
        private DateTime[] _cacheSizeSampleTimes;
        private int _idx;
        //private SRefMultiple _sizedRef;
        private int _gen2Count;
        private long _memoryLimit;
        private DateTime _startupTime;
        private static long AutoPrivateBytesLimit
        {
            get
            {
                long num = CacheMemorySizePressure.s_autoPrivateBytesLimit;
                if (num == -1L)
                {
                    bool flag = IntPtr.Size == 8;
                    long totalPhysical = CacheMemoryPressure.TotalPhysical;
                    long totalVirtual = CacheMemoryPressure.TotalVirtual;
                    if (totalPhysical != 0L)
                    {
                        throw new NotImplementedException();
                        //long val;
                        //if (flag)
                        //{
                        //    val = 1099511627776L;
                        //}
                        //else
                        //{
                        //    //if (totalVirtual > (long)((ulong)-2147483648))
                        //    //{
                        //    //    val = 1887436800L;
                        //    //}
                        //    //else
                        //    //{
                        //    //    val = 838860800L;
                        //    //}
                        //}
                        ////num = Math.Min(HostingEnvironment.IsHosted ? (totalPhysical * 3L / 5L) : totalPhysical, val);
                    }
                    else
                    {
                        num = (flag ? 1099511627776L : 838860800L);
                    }
                    Interlocked.Exchange(ref CacheMemorySizePressure.s_autoPrivateBytesLimit, num);
                }
                return num;
            }
        }
        internal static long EffectiveProcessMemoryLimit
        {
            get
            {
                long num = CacheMemorySizePressure.s_effectiveProcessMemoryLimit;
                if (num == -1L)
                {
                    num = CacheMemorySizePressure.WorkerProcessMemoryLimit;
                    if (num == 0L)
                    {
                        num = CacheMemorySizePressure.AutoPrivateBytesLimit;
                    }
                    Interlocked.Exchange(ref CacheMemorySizePressure.s_effectiveProcessMemoryLimit, num);
                }
                return num;
            }
        }
        internal static long WorkerProcessMemoryLimit
        {
            get
            {
                long num = CacheMemorySizePressure.s_workerProcessMemoryLimit;
                if (num == -1L)
                {
                    throw new NotImplementedException();
                    //if (UnsafeNativeMethods.GetModuleHandle("aspnet_wp.exe") != IntPtr.Zero)
                    //{
                    //    num = (long)UnsafeNativeMethods.PMGetMemoryLimitInMB() << 20;
                    //}
                    //else
                    //{
                    //    if (UnsafeNativeMethods.GetModuleHandle("w3wp.exe") != IntPtr.Zero)
                    //    {
                    //        num = ServerConfig.GetInstance().GetW3WPMemoryLimitInKB() << 10;
                    //    }
                    //}
                    //Interlocked.Exchange(ref CacheMemorySizePressure.s_workerProcessMemoryLimit, num);
                }
                return num;
            }
        }
        internal long MemoryLimit
        {
            get
            {
                return _memoryLimit;
            }
        }
        internal static int PollInterval
        {
            get
            {
                return CacheMemorySizePressure.s_pollInterval;
            }
        }
        internal CacheMemorySizePressure(SRefMultiple sizedRef)
        {
            throw new NotImplementedException();
            //_sizedRef = sizedRef;
            //_gen2Count = GC.CollectionCount(2);
            //_cacheSizeSamples = new long[2];
            //_cacheSizeSampleTimes = new DateTime[2];
            //_pressureHigh = 99;
            //_pressureMiddle = 98;
            //_pressureLow = 97;
            //_startupTime = DateTime.UtcNow;
            //base.InitHistory();
        }
        internal void Dispose()
        {
            throw new NotImplementedException();
            //SRefMultiple sizedRef = _sizedRef;
            //if (sizedRef != null && Interlocked.CompareExchange<SRefMultiple>(ref _sizedRef, null, sizedRef) == sizedRef)
            //{
            //    sizedRef.Dispose();
            //}
            //ApplicationManager applicationManager = HostingEnvironment.GetApplicationManager();
            //if (applicationManager != null)
            //{
            //    long sizeUpdate = 0L - _cacheSizeSamples[_idx];
            //    applicationManager.GetUpdatedTotalCacheSize(sizeUpdate);
            //}
        }
        internal override void ReadConfig(CacheSection cacheSection)
        {
            throw new NotImplementedException();
            //long privateBytesLimit = cacheSection.PrivateBytesLimit;
            //_memoryLimit = CacheMemorySizePressure.WorkerProcessMemoryLimit;
            //if (privateBytesLimit == 0L && _memoryLimit == 0L)
            //{
            //    _memoryLimit = CacheMemorySizePressure.EffectiveProcessMemoryLimit;
            //}
            //else
            //{
            //    if (privateBytesLimit != 0L && _memoryLimit != 0L)
            //    {
            //        _memoryLimit = Math.Min(_memoryLimit, privateBytesLimit);
            //    }
            //    else
            //    {
            //        if (privateBytesLimit != 0L)
            //        {
            //            _memoryLimit = privateBytesLimit;
            //        }
            //    }
            //}
            //if (_memoryLimit > 0L)
            //{
            //    if (CacheMemorySizePressure.s_pid == 0u)
            //    {
            //        CacheMemorySizePressure.s_pid = (uint)SafeNativeMethods.GetCurrentProcessId();
            //    }
            //    _pressureHigh = 100;
            //    _pressureMiddle = 90;
            //    _pressureLow = 80;
            //}
            //CacheMemorySizePressure.s_pollInterval = (int)Math.Min(cacheSection.PrivateBytesPollTime.TotalMilliseconds, 2147483647.0);
            //PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_PROC_MEM_LIMIT_USED_BASE, (int)(_memoryLimit >> 10));
        }
        protected override int GetCurrentPressure()
        {
            throw new NotImplementedException();
            //int num = GC.CollectionCount(2);
            //SRefMultiple sizedRef = _sizedRef;
            //if (num != _gen2Count && sizedRef != null)
            //{
            //    _gen2Count = num;
            //    _idx ^= 1;
            //    _cacheSizeSampleTimes[_idx] = DateTime.UtcNow;
            //    _cacheSizeSamples[_idx] = sizedRef.ApproximateSize;
            //    ApplicationManager applicationManager = HostingEnvironment.GetApplicationManager();
            //    if (applicationManager != null)
            //    {
            //        long sizeUpdate = _cacheSizeSamples[_idx] - _cacheSizeSamples[_idx ^ 1];
            //        _totalCacheSize = applicationManager.GetUpdatedTotalCacheSize(sizeUpdate);
            //    }
            //    else
            //    {
            //        _totalCacheSize = _cacheSizeSamples[_idx];
            //    }
            //}
            //if (_memoryLimit <= 0L)
            //{
            //    return 0;
            //}
            //long num2 = _cacheSizeSamples[_idx];
            //if (num2 > _memoryLimit)
            //{
            //    num2 = _memoryLimit;
            //}
            //PerfCounters.SetCounter(AppPerfCounter.CACHE_PERCENT_PROC_MEM_LIMIT_USED, (int)(num2 >> 10));
            //return (int)(num2 * 100L / _memoryLimit);
        }
        internal override int GetPercentToTrim(DateTime lastTrimTime, int lastTrimPercent)
        {
            int result = 0;
            if (base.IsAboveHighPressure())
            {
                long num = _cacheSizeSamples[_idx];
                if (num > _memoryLimit)
                {
                    result = Math.Min(100, (int)((num - _memoryLimit) * 100L / num));
                }
            }
            return result;
        }
        internal bool HasLimit()
        {
            return _memoryLimit > 0L;
        }
    }
}