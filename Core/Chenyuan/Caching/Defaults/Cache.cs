using System;
using System.Collections;
using System.Security.Permissions;

namespace Chenyuan.Caching.Defaults
{
    public sealed class Cache : IEnumerable
    {
        private class SentinelEntry
        {
            private string _key;
            private CacheDependency _expensiveObjectDependency;
            private CacheItemUpdateCallback _cacheItemUpdateCallback;
            public string Key
            {
                get
                {
                    return _key;
                }
            }
            public CacheDependency ExpensiveObjectDependency
            {
                get
                {
                    return _expensiveObjectDependency;
                }
            }
            public CacheItemUpdateCallback CacheItemUpdateCallback
            {
                get
                {
                    return _cacheItemUpdateCallback;
                }
            }
            public SentinelEntry(string key, CacheDependency expensiveObjectDependency, CacheItemUpdateCallback callback)
            {
                _key = key;
                _expensiveObjectDependency = expensiveObjectDependency;
                _cacheItemUpdateCallback = callback;
            }
            public static void OnCacheItemRemovedCallback(string key, object value, CacheItemRemovedReason reason)
            {
                Cache.SentinelEntry sentinelEntry = value as Cache.SentinelEntry;
                CacheItemUpdateReason reason2;
                switch (reason)
                {
                    case CacheItemRemovedReason.Expired:
                        reason2 = CacheItemUpdateReason.Expired;
                        break;
                    case CacheItemRemovedReason.Underused:
                        return;
                    case CacheItemRemovedReason.DependencyChanged:
                        reason2 = CacheItemUpdateReason.DependencyChanged;
                        if (sentinelEntry.ExpensiveObjectDependency.HasChanged)
                        {
                            return;
                        }
                        break;
                    default:
                        return;
                }
                CacheItemUpdateCallback cacheItemUpdateCallback = sentinelEntry.CacheItemUpdateCallback;
                throw new NotImplementedException();
                /*
                try
                {
                    object obj;
                    CacheDependency cacheDependency;
                    DateTime absoluteExpiration;
                    TimeSpan slidingExpiration;
                    cacheItemUpdateCallback(sentinelEntry.Key, reason2, out obj, out cacheDependency, out absoluteExpiration, out slidingExpiration);
                    if (obj != null && (cacheDependency == null || !cacheDependency.HasChanged))
                    {
                        HttpRuntime.Cache.Insert(sentinelEntry.Key, obj, cacheDependency, absoluteExpiration, slidingExpiration, sentinelEntry.CacheItemUpdateCallback);
                    }
                    else
                    {
                        HttpRuntime.Cache.Remove(sentinelEntry.Key);
                    }
                }
                catch (Exception e)
                {
                    HttpRuntime.Cache.Remove(sentinelEntry.Key);
                    try
                    {
                        WebBaseEvent.RaiseRuntimeError(e, value);
                    }
                    catch
                    {
                    }
                }
                */
            }
        }
        public static readonly DateTime NoAbsoluteExpiration = DateTime.MaxValue;
        public static readonly TimeSpan NoSlidingExpiration = TimeSpan.Zero;
        private CacheInternal _cacheInternal;
        private static CacheItemRemovedCallback s_sentinelRemovedCallback = new CacheItemRemovedCallback(Cache.SentinelEntry.OnCacheItemRemovedCallback);
        public int Count
        {
            get
            {
                return _cacheInternal.PublicCount;
            }
        }
        public object this[string key]
        {
            get
            {
                return this.Get(key);
            }
            set
            {
                this.Insert(key, value);
            }
        }
        public long EffectivePrivateBytesLimit
        {
            get
            {
                return _cacheInternal.EffectivePrivateBytesLimit;
            }
        }
        public long EffectivePercentagePhysicalMemoryLimit
        {
            get
            {
                return _cacheInternal.EffectivePercentagePhysicalMemoryLimit;
            }
        }
        [SecurityPermission(SecurityAction.Demand, Unrestricted = true)]
        public Cache()
        {
        }
        internal Cache(int dummy)
        {
        }
        internal void SetCacheInternal(CacheInternal cacheInternal)
        {
            _cacheInternal = cacheInternal;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)_cacheInternal).GetEnumerator();
        }
        public IDictionaryEnumerator GetEnumerator()
        {
            return _cacheInternal.GetEnumerator();
        }
        public object Get(string key)
        {
            return _cacheInternal.DoGet(true, key, CacheGetOptions.None);
        }
        internal object Get(string key, CacheGetOptions getOptions)
        {
            return _cacheInternal.DoGet(true, key, getOptions);
        }
        public void Insert(string key, object value)
        {
            _cacheInternal.DoInsert(true, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
        }
        public void Insert(string key, object value, CacheDependency dependencies)
        {
            _cacheInternal.DoInsert(true, key, value, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
        }
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            throw new NotImplementedException();
            //DateTime utcAbsoluteExpiration = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
            //_cacheInternal.DoInsert(true, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, CacheItemPriority.Normal, null, true);
        }
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
            //DateTime utcAbsoluteExpiration = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
            //_cacheInternal.DoInsert(true, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, true);
        }
        public void Insert(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemUpdateCallback onUpdateCallback)
        {
            throw new NotImplementedException();
            //if (dependencies == null && absoluteExpiration == Cache.NoAbsoluteExpiration && slidingExpiration == Cache.NoSlidingExpiration)
            //{
            //    throw new ArgumentException(SR.GetString("Invalid_Parameters_To_Insert"));
            //}
            //if (onUpdateCallback == null)
            //{
            //    throw new ArgumentNullException("onUpdateCallback");
            //}
            //DateTime utcAbsoluteExpiration = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
            //_cacheInternal.DoInsert(true, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.NotRemovable, null, true);
            //string[] cachekeys = new string[]
            //{
            //    key
            //};
            //CacheDependency cacheDependency = new CacheDependency(null, cachekeys);
            //if (dependencies == null)
            //{
            //    dependencies = cacheDependency;
            //}
            //else
            //{
            //    AggregateCacheDependency aggregateCacheDependency = new AggregateCacheDependency();
            //    aggregateCacheDependency.Add(new CacheDependency[]
            //    {
            //        dependencies,
            //        cacheDependency
            //    });
            //    dependencies = aggregateCacheDependency;
            //}
            //_cacheInternal.DoInsert(false, "w" + key, new Cache.SentinelEntry(key, cacheDependency, onUpdateCallback), dependencies, utcAbsoluteExpiration, slidingExpiration, CacheItemPriority.NotRemovable, Cache.s_sentinelRemovedCallback, true);
        }
        public object Add(string key, object value, CacheDependency dependencies, DateTime absoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            throw new NotImplementedException();
            //DateTime utcAbsoluteExpiration = DateTimeUtil.ConvertToUniversalTime(absoluteExpiration);
            //return _cacheInternal.DoInsert(true, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, false);
        }
        public object Remove(string key)
        {
            CacheKey cacheKey = new CacheKey(key, true);
            return _cacheInternal.DoRemove(cacheKey, CacheItemRemovedReason.Removed);
        }
    }
}
