using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Chenyuan.Lottery.Services
{
    /// <summary>
    /// 数据层服务基类
    /// </summary>
    public abstract class BaseService
    {
        protected readonly IWorkContext _workContext;

        public BaseService(IWorkContext workContext)
        {
            _workContext = workContext;
        }

        /// <summary>
        /// 指定位数随机数生成
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        protected string GetRandom(int length)
        {
            string rndnum = string.Empty;
            Random random = new Random();
            for (int i = 0; i < length; i++)
            {
                rndnum += random.Next(9).ToString();
            }
            return rndnum;
        }


        #region 【转换】json string 字典
        /// <summary>
        /// 将字典类型序列化为json字符串
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="dict">要序列化的字典数据</param>
        /// <returns>json字符串</returns>
        protected static string SerializeDictionaryToJsonString<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            if (dict.Count == 0)
                return "";

            string jsonStr = JsonConvert.SerializeObject(dict);
            return jsonStr;
        }

        /// <summary>
        /// 将json字符串反序列化为字典类型
        /// </summary>
        /// <typeparam name="TKey">字典key</typeparam>
        /// <typeparam name="TValue">字典value</typeparam>
        /// <param name="jsonStr">json字符串</param>
        /// <returns>字典数据</returns>
        protected static Dictionary<TKey, TValue> DeserializeStringToDictionary<TKey, TValue>(string jsonStr)
        {
            if (string.IsNullOrEmpty(jsonStr))
                return new Dictionary<TKey, TValue>();

            Dictionary<TKey, TValue> jsonDict = JsonConvert.DeserializeObject<Dictionary<TKey, TValue>>(jsonStr);

            return jsonDict;
        }
        #endregion
    }
}
