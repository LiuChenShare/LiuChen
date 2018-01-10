using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Utilities
{
    /// <summary>
    /// 随机键生成器
    /// </summary>
    public static class RndKeyGen
    {
        /// <summary>
        /// 创建长度为
        /// </summary>
        /// <param name="length"></param>
        /// <param name="base">计数基数，默认为16</param>
        /// <param name="state">字母状态</param>
        /// <returns></returns>
        public static string Create(int length, int @base = 16, CharState state = CharState.Normal)
        {
            if (@base != 16)
            {
                throw new NotSupportedException("基数只能是16，其它进一步实现中。");
            }
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            byte[] buff = new byte[length];
            rng.GetBytes(buff);
            System.Text.StringBuilder hexString = new System.Text.StringBuilder(64);
            for (int i = 0; i < buff.Length; i++)
            {
                hexString.Append(String.Format("{0:X2}", buff[i]));
            }
            if (state == CharState.Random)
            {
                for (int i = 0; i < hexString.Length; i++)
                {
                    if (hexString[i] >= 'A' && new Random().Next(1, 100) % 2 == 1)
                    {
                        hexString[i] += (char)32;
                    }
                }
            }
            string result = "";
            for (int i = 0; i < length; i++)
            {
                result += hexString[i / 2 + i % 2];
            }
            if (state == CharState.LowCase)
            {
                return result.ToLower();
            }
            return result;
        }

        /// <summary>
        /// 字符状态
        /// </summary>
        public enum CharState
        {
            /// <summary>
            /// 常规，大写
            /// </summary>
            Normal,
            /// <summary>
            /// 小写
            /// </summary>
            LowCase,
            /// <summary>
            /// 大写
            /// </summary>
            UpCase,
            /// <summary>
            /// 随机
            /// </summary>
            Random,
        }
    }
}
