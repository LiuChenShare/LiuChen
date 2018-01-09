using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Utilities
{
    /// <summary>
    /// 哈希编码算法类型
    /// </summary>
    public enum HashEncodeType
    {
        /// <summary>
        /// 未知类型
        /// </summary>
        Unknown,
        /// <summary>
        /// MD5编码
        /// </summary>
        MD5,
        /// <summary>
        /// HMAC编码
        /// </summary>
        HMAC,
    }
}
