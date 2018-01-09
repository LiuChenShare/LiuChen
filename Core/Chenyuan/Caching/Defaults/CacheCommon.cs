using System;
using System.Diagnostics;
using System.Threading;

namespace Chenyuan.Caching.Defaults
{
    internal class CacheCommon
    {
        private const int MEMORYSTATUS_INTERVAL_5_SECONDS = 5000;
        private const int MEMORYSTATUS_INTERVAL_30_SECONDS = 30000;
        internal CacheInternal _cacheInternal;
        internal Cache _cachePublic;
        protected internal CacheMemoryStats _cacheMemoryStats;
        private object _timerLock = new object();
        private DisposableGCHandleRef<Timer> _timerHandleRef;
        private int _currentPollInterval = 30000;
        internal int _inCacheManagerThread;
        internal bool _enableMemoryCollection;
        internal bool _enableExpiration;
        internal bool _internalConfigRead;
        internal SRefMultiple _srefMultiple;
        private int _disposed;
        internal CacheCommon()
        {
            _cachePublic = new Cache(0);
            _srefMultiple = new SRefMultiple();
            _cacheMemoryStats = new CacheMemoryStats(_srefMultiple);
            _enableMemoryCollection = true;
            _enableExpiration = true;
        }
        internal void Dispose(bool disposing)
        {
            if (disposing && Interlocked.Exchange(ref _disposed, 1) == 0)
            {
                this.EnableCacheMemoryTimer(false);
                _cacheMemoryStats.Dispose();
            }
        }
        internal void AddSRefTarget(object o)
        {
            _srefMultiple.AddSRefTarget(o);
        }
        internal void SetCacheInternal(CacheInternal cacheInternal)
        {
            _cacheInternal = cacheInternal;
            _cachePublic.SetCacheInternal(cacheInternal);
        }
        internal void ReadCacheInternalConfig(CacheSection cacheSection)
        {
            if (_internalConfigRead)
            {
                return;
            }
            bool flag = false;
            try
            {
                Monitor.Enter(this, ref flag);
                if (!_internalConfigRead)
                {
                    _internalConfigRead = true;
                    if (cacheSection != null)
                    {
                        _enableMemoryCollection = !cacheSection.DisableMemoryCollection;
                        _enableExpiration = !cacheSection.DisableExpiration;
                        _cacheMemoryStats.ReadConfig(cacheSection);
                        _currentPollInterval = CacheMemorySizePressure.PollInterval;
                        this.ResetFromConfigSettings();
                    }
                }
            }
            finally
            {
                if (flag)
                {
                    Monitor.Exit(this);
                }
            }
        }
        internal void ResetFromConfigSettings()
        {
            this.EnableCacheMemoryTimer(_enableMemoryCollection);
            _cacheInternal.EnableExpirationTimer(_enableExpiration);
        }
        internal void EnableCacheMemoryTimer(bool enable)
        {
            object timerLock = _timerLock;
            lock (timerLock)
            {
                if (enable)
                {
                    if (_timerHandleRef == null)
                    {
                        Timer t = new Timer(new TimerCallback(this.CacheManagerTimerCallback), null, _currentPollInterval, _currentPollInterval);
                        _timerHandleRef = new DisposableGCHandleRef<Timer>(t);
                    }
                    else
                    {
                        _timerHandleRef.Target.Change(_currentPollInterval, _currentPollInterval);
                    }
                }
                else
                {
                    DisposableGCHandleRef<Timer> timerHandleRef = _timerHandleRef;
                    if (timerHandleRef != null && Interlocked.CompareExchange<DisposableGCHandleRef<Timer>>(ref _timerHandleRef, null, timerHandleRef) == timerHandleRef)
                    {
                        timerHandleRef.Dispose();
                    }
                }
            }
            if (!enable)
            {
                while (_inCacheManagerThread != 0)
                {
                    Thread.Sleep(100);
                }
            }
        }
        private void AdjustTimer()
        {
            object timerLock = _timerLock;
            lock (timerLock)
            {
                if (_timerHandleRef != null)
                {
                    if (_cacheMemoryStats.IsAboveHighPressure())
                    {
                        if (_currentPollInterval > 5000)
                        {
                            _currentPollInterval = 5000;
                            _timerHandleRef.Target.Change(_currentPollInterval, _currentPollInterval);
                        }
                    }
                    else
                    {
                        if (_cacheMemoryStats.CacheSizePressure.PressureLast > _cacheMemoryStats.CacheSizePressure.PressureLow / 2 || _cacheMemoryStats.TotalMemoryPressure.PressureLast > _cacheMemoryStats.TotalMemoryPressure.PressureLow / 2)
                        {
                            int num = Math.Min(CacheMemorySizePressure.PollInterval, 30000);
                            if (_currentPollInterval != num)
                            {
                                _currentPollInterval = num;
                                _timerHandleRef.Target.Change(_currentPollInterval, _currentPollInterval);
                            }
                        }
                        else
                        {
                            if (_currentPollInterval != CacheMemorySizePressure.PollInterval)
                            {
                                _currentPollInterval = CacheMemorySizePressure.PollInterval;
                                _timerHandleRef.Target.Change(_currentPollInterval, _currentPollInterval);
                            }
                        }
                    }
                }
            }
        }
        private void CacheManagerTimerCallback(object state)
        {
            this.CacheManagerThread(0);
        }
        internal long CacheManagerThread(int minPercent)
        {
            if (Interlocked.Exchange(ref _inCacheManagerThread, 1) != 0)
            {
                return 0L;
            }
            long result;
            try
            {
                if (_timerHandleRef == null)
                {
                    result = 0L;
                }
                else
                {
                    _cacheMemoryStats.Update();
                    this.AdjustTimer();
                    int num = Math.Max(minPercent, _cacheMemoryStats.GetPercentToTrim());
                    long totalCount = _cacheInternal.TotalCount;
                    Stopwatch stopwatch = Stopwatch.StartNew();
                    long num2 = _cacheInternal.TrimIfNecessary(num);
                    stopwatch.Stop();
                    if (num > 0 && num2 > 0L)
                    {
                        _cacheMemoryStats.SetTrimStats(stopwatch.Elapsed.Ticks, totalCount, num2);
                    }
                    result = num2;
                }
            }
            finally
            {
                Interlocked.Exchange(ref _inCacheManagerThread, 0);
            }
            return result;
        }
    }
}
