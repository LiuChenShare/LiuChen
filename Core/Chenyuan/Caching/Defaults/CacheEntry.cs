using System;
using System.Collections;
using System.Threading;

namespace Chenyuan.Caching.Defaults
{
    internal sealed class CacheEntry : CacheKey, ICacheDependencyChanged
    {
        internal enum EntryState : byte
        {
            NotInCache,
            AddingToCache,
            AddedToCache,
            RemovingFromCache = 4,
            RemovedFromCache = 8,
            Closed = 16
        }
        private static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        private static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;
        private const CacheItemPriority CacheItemPriorityMin = CacheItemPriority.Low;
        private const CacheItemPriority CacheItemPriorityMax = CacheItemPriority.NotRemovable;
        private static readonly TimeSpan OneYear = new TimeSpan(365, 0, 0, 0);
        private const byte EntryStateMask = 31;
        private object _value;
        private DateTime _utcCreated;
        private DateTime _utcExpires;
        private TimeSpan _slidingExpiration;
        private byte _expiresBucket;
        private ExpiresEntryRef _expiresEntryRef;
        private byte _usageBucket;
        private UsageEntryRef _usageEntryRef;
        private DateTime _utcLastUpdate;
        private CacheDependency _dependency;
        private object _onRemovedTargets;
        internal object Value
        {
            get
            {
                return _value;
            }
        }
        internal DateTime UtcCreated
        {
            get
            {
                return _utcCreated;
            }
        }
        internal EntryState State
        {
            get
            {
                return (EntryState)(_bits & 31);
            }
            set
            {
                _bits = (byte)(((int)_bits & -32) | (int)value);
            }
        }
        internal DateTime UtcExpires
        {
            get
            {
                return _utcExpires;
            }
            set
            {
                _utcExpires = value;
            }
        }
        internal TimeSpan SlidingExpiration
        {
            get
            {
                return _slidingExpiration;
            }
        }
        internal byte ExpiresBucket
        {
            get
            {
                return _expiresBucket;
            }
            set
            {
                _expiresBucket = value;
            }
        }
        internal ExpiresEntryRef ExpiresEntryRef
        {
            get
            {
                return _expiresEntryRef;
            }
            set
            {
                _expiresEntryRef = value;
            }
        }
        internal byte UsageBucket
        {
            get
            {
                return _usageBucket;
            }
        }
        internal UsageEntryRef UsageEntryRef
        {
            get
            {
                return _usageEntryRef;
            }
            set
            {
                _usageEntryRef = value;
            }
        }
        internal DateTime UtcLastUsageUpdate
        {
            get
            {
                return _utcLastUpdate;
            }
            set
            {
                _utcLastUpdate = value;
            }
        }
        internal CacheDependency Dependency
        {
            get
            {
                return _dependency;
            }
        }
        internal CacheEntry(string key, object value, CacheDependency dependency, CacheItemRemovedCallback onRemovedHandler, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, bool isPublic) : base(key, isPublic)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (slidingExpiration < TimeSpan.Zero || OneYear < slidingExpiration)
            {
                throw new ArgumentOutOfRangeException(nameof(slidingExpiration));
            }
            if (utcAbsoluteExpiration != Cache.NoAbsoluteExpiration && slidingExpiration != Cache.NoSlidingExpiration)
            {
                throw new ArgumentException(@"SR.GetString(""Invalid_expiration_combination"")");
            }
            if (priority < CacheItemPriority.Low || CacheItemPriority.NotRemovable < priority)
            {
                throw new ArgumentOutOfRangeException(nameof(priority));
            }
            _value = value;
            _dependency = dependency;
            _onRemovedTargets = onRemovedHandler;
            _utcCreated = DateTime.UtcNow;
            _slidingExpiration = slidingExpiration;
            if (_slidingExpiration > TimeSpan.Zero)
            {
                _utcExpires = _utcCreated + _slidingExpiration;
            }
            else
            {
                _utcExpires = utcAbsoluteExpiration;
            }
            _expiresEntryRef = ExpiresEntryRef.INVALID;
            _expiresBucket = 255;
            _usageEntryRef = UsageEntryRef.INVALID;
            if (priority == CacheItemPriority.NotRemovable)
            {
                _usageBucket = 255;
                return;
            }
            _usageBucket = (byte)(priority - CacheItemPriority.Low);
        }
        internal bool HasExpiration()
        {
            return _utcExpires < DateTime.MaxValue;
        }
        internal bool InExpires()
        {
            return !_expiresEntryRef.IsInvalid;
        }
        internal bool HasUsage()
        {
            return _usageBucket != 255;
        }
        internal bool InUsage()
        {
            return !_usageEntryRef.IsInvalid;
        }
        internal void MonitorDependencyChanges()
        {
            CacheDependency dependency = _dependency;
            if (dependency != null && this.State == EntryState.AddedToCache)
            {
                if (!dependency.Use())
                {
                    throw new InvalidOperationException(@"SR.GetString(""Cache_dependency_used_more_that_once"")");
                }
                dependency.SetCacheDependencyChanged(this);
            }
        }
        void ICacheDependencyChanged.DependencyChanged(object sender, EventArgs e)
        {
            if (this.State == CacheEntry.EntryState.AddedToCache)
            {
                throw new NotImplementedException();
                //HttpRuntime.CacheInternal.Remove(this, CacheItemRemovedReason.DependencyChanged);
            }
        }
        private void CallCacheItemRemovedCallback(CacheItemRemovedCallback callback, CacheItemRemovedReason reason)
        {
            throw new NotImplementedException();
            /*
            if (base.IsPublic)
            {
                try
                {
                    if (HttpContext.Current == null)
                    {
                        using (new ApplicationImpersonationContext())
                        {
                            callback(_key, _value, reason);
                            goto IL_47;
                        }
                    }
                    callback(_key, _value, reason);
                    IL_47:
                    return;
                }
                catch (Exception ex)
                {
                    HttpApplicationFactory.RaiseError(ex);
                    try
                    {
                        WebBaseEvent.RaiseRuntimeError(ex, this);
                    }
                    catch
                    {
                    }
                    return;
                }
            }
            try
            {
                using (new ApplicationImpersonationContext())
                {
                    callback(_key, _value, reason);
                }
            }
            catch
            {
            }
            */
        }
        internal void Close(CacheItemRemovedReason reason)
        {
            this.State = CacheEntry.EntryState.Closed;
            object obj = null;
            object[] array = null;
            bool flag = false;
            try
            {
                Monitor.Enter(this, ref flag);
                if (_onRemovedTargets != null)
                {
                    obj = _onRemovedTargets;
                    if (obj is Hashtable)
                    {
                        ICollection expr_3A = ((Hashtable)obj).Keys;
                        array = new object[expr_3A.Count];
                        expr_3A.CopyTo(array, 0);
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
            if (obj != null)
            {
                if (array != null)
                {
                    object[] array2 = array;
                    for (int i = 0; i < array2.Length; i++)
                    {
                        object obj2 = array2[i];
                        if (obj2 is CacheDependency)
                        {
                            ((CacheDependency)obj2).ItemRemoved();
                        }
                        else
                        {
                            this.CallCacheItemRemovedCallback((CacheItemRemovedCallback)obj2, reason);
                        }
                    }
                }
                else
                {
                    if (obj is CacheItemRemovedCallback)
                    {
                        this.CallCacheItemRemovedCallback((CacheItemRemovedCallback)obj, reason);
                    }
                    else
                    {
                        ((CacheDependency)obj).ItemRemoved();
                    }
                }
            }
            if (_dependency != null)
            {
                _dependency.DisposeInternal();
            }
        }
        internal void AddCacheDependencyNotify(CacheDependency dependency)
        {
            bool flag = false;
            try
            {
                Monitor.Enter(this, ref flag);
                if (_onRemovedTargets == null)
                {
                    _onRemovedTargets = dependency;
                }
                else
                {
                    if (_onRemovedTargets is Hashtable)
                    {
                        ((Hashtable)_onRemovedTargets)[dependency] = dependency;
                    }
                    else
                    {
                        Hashtable hashtable = new Hashtable(2);
                        hashtable[_onRemovedTargets] = _onRemovedTargets;
                        hashtable[dependency] = dependency;
                        _onRemovedTargets = hashtable;
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
        internal void RemoveCacheDependencyNotify(CacheDependency dependency)
        {
            bool flag = false;
            try
            {
                Monitor.Enter(this, ref flag);
                if (_onRemovedTargets != null)
                {
                    if (_onRemovedTargets == dependency)
                    {
                        _onRemovedTargets = null;
                    }
                    else
                    {
                        Hashtable hashtable = (Hashtable)_onRemovedTargets;
                        hashtable.Remove(dependency);
                        if (hashtable.Count == 0)
                        {
                            _onRemovedTargets = null;
                        }
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
    }
}
