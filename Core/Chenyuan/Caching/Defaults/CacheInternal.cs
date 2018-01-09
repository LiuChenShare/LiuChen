using System;
using System.Collections;

namespace Chenyuan.Caching.Defaults
{
    internal abstract class CacheInternal : IEnumerable, IDisposable
    {
        internal const string PrefixFIRST = "A";
        internal const string PrefixResourceProvider = "A";
        internal const string PrefixMapPathVPPFile = "Bf";
        internal const string PrefixMapPathVPPDir = "Bd";
        internal const string PrefixOutputCache = "a";
        internal const string PrefixSqlCacheDependency = "b";
        internal const string PrefixMemoryBuildResult = "c";
        internal const string PrefixPathData = "d";
        internal const string PrefixHttpCapabilities = "e";
        internal const string PrefixMapPath = "f";
        internal const string PrefixHttpSys = "g";
        internal const string PrefixFileSecurity = "h";
        internal const string PrefixInProcSessionState = "j";
        internal const string PrefixStateApplication = "k";
        internal const string PrefixPartialCachingControl = "l";
        internal const string UNUSED = "m";
        internal const string PrefixAdRotator = "n";
        internal const string PrefixWebServiceDataSource = "o";
        internal const string PrefixLoadXPath = "p";
        internal const string PrefixLoadXml = "q";
        internal const string PrefixLoadTransform = "r";
        internal const string PrefixAspCompatThreading = "s";
        internal const string PrefixDataSourceControl = "u";
        internal const string PrefixValidationSentinel = "w";
        internal const string PrefixWebEventResource = "x";
        internal const string PrefixAssemblyPath = "y";
        internal const string PrefixBrowserCapsHash = "z";
        internal const string PrefixLAST = "z";
        protected CacheCommon _cacheCommon;
        private int _disposed;
        internal abstract int PublicCount
        {
            get;
        }
        internal abstract long TotalCount
        {
            get;
        }
        internal bool IsDisposed
        {
            get
            {
                return _disposed == 1;
            }
        }
        internal Cache CachePublic
        {
            get
            {
                return _cacheCommon._cachePublic;
            }
        }
        internal long EffectivePrivateBytesLimit
        {
            get
            {
                return _cacheCommon._cacheMemoryStats.CacheSizePressure.MemoryLimit;
            }
        }
        internal long EffectivePercentagePhysicalMemoryLimit
        {
            get
            {
                return _cacheCommon._cacheMemoryStats.TotalMemoryPressure.MemoryLimit;
            }
        }
        internal object this[string key]
        {
            get
            {
                return this.Get(key);
            }
        }
        internal abstract IDictionaryEnumerator CreateEnumerator();
        internal abstract CacheEntry UpdateCache(CacheKey cacheKey, CacheEntry newEntry, bool replace, CacheItemRemovedReason removedReason, out object valueOld);
        internal abstract long TrimIfNecessary(int percent);
        internal abstract void EnableExpirationTimer(bool enable);
        internal static CacheInternal Create()
        {
            throw new NotImplementedException();
            //CacheCommon cacheCommon = new CacheCommon();
            //uint num = (uint)SystemInfo.GetNumProcessCPUs();
            //int num2 = 1;
            //for (num -= 1u; num > 0u; num >>= 1)
            //{
            //    num2 <<= 1;
            //}
            //CacheInternal cacheInternal;
            //if (num2 == 1)
            //{
            //    cacheInternal = new CacheSingle(cacheCommon, null, 0);
            //}
            //else
            //{
            //    cacheInternal = new CacheMultiple(cacheCommon, num2);
            //}
            //cacheCommon.SetCacheInternal(cacheInternal);
            //cacheCommon.ResetFromConfigSettings();
            //return cacheInternal;
        }
        protected CacheInternal(CacheCommon cacheCommon)
        {
            _cacheCommon = cacheCommon;
        }
        protected virtual void Dispose(bool disposing)
        {
            _cacheCommon.Dispose(disposing);
        }
        public void Dispose()
        {
            _disposed = 1;
            this.Dispose(true);
        }
        internal void ReadCacheInternalConfig(CacheSection cacheSection)
        {
            _cacheCommon.ReadCacheInternalConfig(cacheSection);
        }
        internal long TrimCache(int percent)
        {
            return _cacheCommon.CacheManagerThread(percent);
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.CreateEnumerator();
        }
        public IDictionaryEnumerator GetEnumerator()
        {
            return this.CreateEnumerator();
        }
        internal object Get(string key)
        {
            return this.DoGet(false, key, CacheGetOptions.None);
        }
        internal object Get(string key, CacheGetOptions getOptions)
        {
            return this.DoGet(false, key, getOptions);
        }
        internal object DoGet(bool isPublic, string key, CacheGetOptions getOptions)
        {
            CacheKey cacheKey = new CacheKey(key, isPublic);
            object obj;
            CacheEntry cacheEntry = this.UpdateCache(cacheKey, null, false, CacheItemRemovedReason.Removed, out obj);
            if (cacheEntry == null)
            {
                return null;
            }
            if ((getOptions & CacheGetOptions.ReturnCacheEntry) != CacheGetOptions.None)
            {
                return cacheEntry;
            }
            return cacheEntry.Value;
        }
        internal void UtcInsert(string key, object value)
        {
            this.DoInsert(false, key, value, null, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
        }
        internal void UtcInsert(string key, object value, CacheDependency dependencies)
        {
            this.DoInsert(false, key, value, dependencies, Cache.NoAbsoluteExpiration, Cache.NoSlidingExpiration, CacheItemPriority.Normal, null, true);
        }
        internal void UtcInsert(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration)
        {
            this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, CacheItemPriority.Normal, null, true);
        }
        internal void UtcInsert(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, true);
        }
        internal object UtcAdd(string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback)
        {
            return this.DoInsert(false, key, value, dependencies, utcAbsoluteExpiration, slidingExpiration, priority, onRemoveCallback, false);
        }
        internal object DoInsert(bool isPublic, string key, object value, CacheDependency dependencies, DateTime utcAbsoluteExpiration, TimeSpan slidingExpiration, CacheItemPriority priority, CacheItemRemovedCallback onRemoveCallback, bool replace)
        {
            object result;
            try
            {
                CacheEntry cacheEntry = new CacheEntry(key, value, dependencies, onRemoveCallback, utcAbsoluteExpiration, slidingExpiration, priority, isPublic);
                object obj;
                cacheEntry = this.UpdateCache(cacheEntry, cacheEntry, replace, CacheItemRemovedReason.Removed, out obj);
                if (cacheEntry != null)
                {
                    result = cacheEntry.Value;
                }
                else
                {
                    result = null;
                }
            }
            finally
            {
                if (dependencies != null)
                {
                    ((IDisposable)dependencies).Dispose();
                }
            }
            return result;
        }
        internal object Remove(string key)
        {
            CacheKey cacheKey = new CacheKey(key, false);
            return this.DoRemove(cacheKey, CacheItemRemovedReason.Removed);
        }
        internal object Remove(CacheKey cacheKey, CacheItemRemovedReason reason)
        {
            return this.DoRemove(cacheKey, reason);
        }
        internal object DoRemove(CacheKey cacheKey, CacheItemRemovedReason reason)
        {
            object result;
            this.UpdateCache(cacheKey, null, true, reason, out result);
            return result;
        }
    }
}
