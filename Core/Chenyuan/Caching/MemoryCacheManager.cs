using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Chenyuan.Caching
{
    public partial class MemoryCacheManager:ICacheManager
    {
        protected ObjectCache Cache
        {
            get
            {
                return MemoryCache.Default;
            }
        }

        public virtual T Get<T>(string key)
        {
            return (T)Cache[key];
        }

        public virtual void Set(string key,object data,int cacheTime)
        {
            if (data == null)
                return;
            var policy = new CacheItemPolicy();
            if(cacheTime>0)
                policy.AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(cacheTime);
            Cache.Add(new CacheItem(key,data),policy);
        }

        public virtual bool IsSet(string key)
        {
            return (Cache.Contains(key));
        }

        public virtual void Remove(string key)
        {
            Cache.Remove(key);
        }

        public virtual void RemoveByPattern(string pattern)
        {
            var regex = new Regex(pattern,RegexOptions.Singleline|RegexOptions.Compiled|RegexOptions.IgnoreCase);
            var keys = new List<string>();
            foreach(var item in Cache)
            {
                if (regex.IsMatch(item.Key))
                {
                    keys.Add(item.Key);
                }
            }
            foreach(string key in keys)
            {
                Remove(key);
            }
        }

        public virtual void Clear()
        {
            foreach (var item in Cache)
                Remove(item.Key);
        }

        /// <summary>
        /// 读取缓存中的所有key
        /// </summary>
        /// <returns></returns>
        public IList<string> GetAllKeys()
        {
            IList<string> allKeys = new List<string>();
            foreach(var item in Cache)
            {
                allKeys.Add(item.Key);
            }
            return allKeys;
        }

    }
}
