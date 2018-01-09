using System;

namespace Chenyuan.Caching.Defaults
{
    public delegate void CacheItemUpdateCallback(string key, CacheItemUpdateReason reason, out object expensiveObject, out CacheDependency dependency, out DateTime absoluteExpiration, out TimeSpan slidingExpiration);
}
