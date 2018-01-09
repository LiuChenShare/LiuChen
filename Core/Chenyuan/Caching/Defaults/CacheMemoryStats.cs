using System;

namespace Chenyuan.Caching.Defaults
{
    internal class CacheMemoryStats
    {
        private DateTime _lastTrimTime = DateTime.MinValue;
        private long _lastTrimDurationTicks;
        private int _lastTrimPercent;
        private long _totalCountBeforeTrim;
        private long _lastTrimCount;
        private int _lastTrimGen2Count = -1;
        private CacheMemoryTotalMemoryPressure _pressureTotalMemory;
        private CacheMemorySizePressure _pressureCacheSize;
        internal CacheMemorySizePressure CacheSizePressure
        {
            get
            {
                return _pressureCacheSize;
            }
        }
        internal CacheMemoryTotalMemoryPressure TotalMemoryPressure
        {
            get
            {
                return _pressureTotalMemory;
            }
        }
        internal CacheMemoryStats(SRefMultiple sizedRef)
        {
            _pressureTotalMemory = new CacheMemoryTotalMemoryPressure();
            _pressureCacheSize = new CacheMemorySizePressure(sizedRef);
        }
        internal bool IsAboveHighPressure()
        {
            return _pressureTotalMemory.IsAboveHighPressure() || _pressureCacheSize.IsAboveHighPressure();
        }
        internal bool IsAboveMediumPressure()
        {
            return _pressureTotalMemory.IsAboveMediumPressure() || _pressureCacheSize.IsAboveMediumPressure();
        }
        internal void ReadConfig(CacheSection cacheSection)
        {
            _pressureTotalMemory.ReadConfig(cacheSection);
            _pressureCacheSize.ReadConfig(cacheSection);
        }
        internal void Update()
        {
            _pressureTotalMemory.Update();
            _pressureCacheSize.Update();
        }
        internal void Dispose()
        {
            _pressureCacheSize.Dispose();
        }
        internal int GetPercentToTrim()
        {
            if (GC.CollectionCount(2) != _lastTrimGen2Count)
            {
                return Math.Max(_pressureTotalMemory.GetPercentToTrim(_lastTrimTime, _lastTrimPercent), _pressureCacheSize.GetPercentToTrim(_lastTrimTime, _lastTrimPercent));
            }
            return 0;
        }
        internal void SetTrimStats(long trimDurationTicks, long totalCountBeforeTrim, long trimCount)
        {
            _lastTrimDurationTicks = trimDurationTicks;
            int num = GC.CollectionCount(2);
            if (num != _lastTrimGen2Count)
            {
                _lastTrimTime = DateTime.UtcNow;
                _totalCountBeforeTrim = totalCountBeforeTrim;
                _lastTrimCount = trimCount;
            }
            else
            {
                _lastTrimCount += trimCount;
            }
            _lastTrimGen2Count = num;
            _lastTrimPercent = (int)(_lastTrimCount * 100L / _totalCountBeforeTrim);
        }
    }
}
