using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Caching
{
    public interface ICacheManager
    {
        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        T Get<T>(string key);

        /// <summary>
        /// 添加缓存
        /// </summary>
        /// <param name="key"></param>
        /// <param name="data"></param>
        /// <param name="cacheTime"></param>
        void Set(string key,object data,int cacheTime);

        /// <summary>
        /// 是否缓存指定key的值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool IsSet(string key);

        /// <summary>
        /// 移除指定项
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        /// 依据正则移除匹配项
        /// </summary>
        /// <param name="pattern"></param>
        void RemoveByPattern(string pattern);

        /// <summary>
        /// 清空所有缓存项
        /// </summary>
        void Clear();
        /// <summary>
        /// 读取缓存中的所有key
        /// </summary>
        /// <returns></returns>
        IList<string> GetAllKeys();
    }
}
