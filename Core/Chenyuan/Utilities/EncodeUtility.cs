using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Utilities
{
    ///// <summary>
    ///// 编码实用方法
    ///// </summary>
    //public static class EncodeUtility
    //{
    //    /// <summary>
    //    /// MD5编码
    //    /// </summary>
    //    /// <param name="encodeType">加密编码方法</param>
    //    /// <param name="source">源字符串</param>
    //    /// <param name="lowcase">结果大小写：true-小写，false-大写，null-不处理</param>
    //    /// <param name="encoding">编码前源字符串内容编码</param>
    //    /// <returns>编码后目标字符串</returns>
    //    public static string SecurityEncode(HashEncodeType encodeType, string source, bool? lowcase = null, Encoding encoding = null)
    //    {
    //        byte[] data = StringToBytes(source, encoding);
    //        return SecurityEncode(encodeType, data, lowcase);
    //    }

    //    /// <summary>
    //    /// MD5编码
    //    /// </summary>
    //    /// <param name="encodeType">加密编码方法</param>
    //    /// <param name="source">源字符串</param>
    //    /// <param name="encoding">编码前源字符串内容编码</param>
    //    /// <returns>编码后数据</returns>
    //    public static byte[] SecurityEncode(HashEncodeType encodeType, string source, Encoding encoding = null)
    //    {
    //        byte[] data = StringToBytes(source, encoding);
    //        return SecurityEncode(encodeType, data);
    //    }

    //    /// <summary>
    //    /// MD5编码
    //    /// </summary>
    //    /// <param name="encodeType">加密编码方法</param>
    //    /// <param name="source">源数据</param>
    //    /// <returns>编码后数据</returns>
    //    public static byte[] SecurityEncode(HashEncodeType encodeType, byte[] source)
    //    {
    //        switch (encodeType)
    //        {
    //            case HashEncodeType.MD5:
    //                return MD5Encode(source);
    //            case HashEncodeType.HMAC:
    //                return HMACEncode(source);
    //            default:
    //                throw new NotImplementedException();
    //        }
    //    }

    //    private static byte[] MD5Encode(byte[] data)
    //    {
    //        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();
    //        return md5.ComputeHash(data);
    //    }

    //    private static byte[] HMACEncode(byte[] data)
    //    {
    //        System.Security.Cryptography.HMAC md5 = System.Security.Cryptography.HMAC.Create();
    //        return md5.ComputeHash(data);
    //    }

    //    /// <summary>
    //    /// MD5编码
    //    /// </summary>
    //    /// <param name="encodeType">加密编码方法</param>
    //    /// <param name="source">源字符串</param>
    //    /// <param name="lowcase">结果大小写：true-小写，false-大写，null-不处理</param>
    //    /// <returns>编码后目标字符串</returns>
    //    public static string SecurityEncode(HashEncodeType encodeType, byte[] source, bool? lowcase = null)
    //    {
    //        source = SecurityEncode(encodeType, source);
    //        return MD5BytesToString(source, lowcase.HasValue ? lowcase.Value : true);
    //    }

    //    /// <summary>
    //    /// 把字符串转换为字节
    //    /// </summary>
    //    /// <param name="source">源字符串</param>
    //    /// <param name="encoding">转换后文本编码</param>
    //    /// <returns>目标数据</returns>
    //    private static byte[] StringToBytes(string source, Encoding encoding = null)
    //    {
    //        encoding = encoding ?? Encoding.UTF8;
    //        return encoding.GetBytes(source);
    //    }

    //    /// <summary>
    //    /// 把字节序列转换为字符串
    //    /// </summary>
    //    /// <param name="source">源字节序列</param>
    //    /// <param name="encoding">源文本编码</param>
    //    /// <returns>目标字符串</returns>
    //    private static string BytesToString(byte[] source, Encoding encoding = null)
    //    {
    //        encoding = encoding ?? Encoding.UTF8;
    //        return encoding.GetString(source);
    //    }

    //    /// <summary>
    //    /// MD5字节转字符串
    //    /// </summary>
    //    /// <param name="md5">MD5字节序列</param>
    //    /// <param name="locase">目标字符串大小写，true：小写，false：大写</param>
    //    /// <returns>目标编码</returns>
    //    private static string MD5BytesToString(byte[] md5, bool locase)
    //    {
    //        string format = locase ? "x" : "X";
    //        return string.Join("", md5.Select(o => (o > 15 ? "" : "0") + o.ToString(format)).ToArray());
    //    }
    //}
    ///// <summary>
    /// 编码实用方法
    /// </summary>
    public static class EncodeUtility
    {
        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="source">源字符串</param>
        /// <param name="lowcase">结果大小写：true-小写，false-大写，null-不处理</param>
        /// <param name="encoding">编码前源字符串内容编码</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToHex(HashEncodeType encodeType, string source, bool? lowcase = null, Encoding encoding = null)
        {
            byte[] data = StringToBytes(source, encoding);
            return SecurityEncodeToHex(encodeType, data, lowcase);
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="data">源字符串</param>
        /// <param name="lowcase">结果大小写：true-小写，false-大写，null-不处理</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToHex(HashEncodeType encodeType, byte[] data, bool? lowcase = null)
        {
            var result = SecurityEncode(encodeType, data);
            return BytesToHex(result, (bool)lowcase.HasValue);
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="dataStream">源字符串</param>
        /// <param name="lowcase">结果大小写：true-小写，false-大写，null-不处理</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToHex(HashEncodeType encodeType, Stream dataStream, bool? lowcase = null)
        {
            var result = SecurityEncode(encodeType, dataStream);
            return BytesToHex(result, (bool)lowcase.HasValue);
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码前源字符串内容编码</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToBase64(HashEncodeType encodeType, string source, Encoding encoding = null)
        {
            byte[] data = StringToBytes(source, encoding);
            return SecurityEncodeToBase64(encodeType, data);
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="data">源字符串</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToBase64(HashEncodeType encodeType, byte[] data)
        {
            var result = SecurityEncode(encodeType, data);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// 数据编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="dataStream">源字符串</param>
        /// <returns>编码后目标字符串</returns>
        public static string SecurityEncodeToBase64(HashEncodeType encodeType, Stream dataStream)
        {
            var result = SecurityEncode(encodeType, dataStream);
            return Convert.ToBase64String(result);
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">编码前源字符串内容编码</param>
        /// <returns>编码后数据</returns>
        public static byte[] SecurityEncode(HashEncodeType encodeType, string source, Encoding encoding = null)
        {
            byte[] data = StringToBytes(source, encoding);
            return SecurityEncode(encodeType, data);
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="data">源字符串</param>
        /// <returns>编码后数据</returns>
        public static byte[] SecurityEncode(HashEncodeType encodeType, byte[] data)
        {
            switch (encodeType)
            {
                case HashEncodeType.Unknown:
                    throw new NotImplementedException();
                default:
                    HashAlgorithm hashAlg = HashAlgorithm.Create(encodeType.ToString());
                    return hashAlg.ComputeHash(data);
            }
        }

        /// <summary>
        /// MD5编码
        /// </summary>
        /// <param name="encodeType">加密编码方法</param>
        /// <param name="dataStream">源字符串</param>
        /// <returns>编码后数据</returns>
        public static byte[] SecurityEncode(HashEncodeType encodeType, Stream dataStream)
        {
            switch (encodeType)
            {
                case HashEncodeType.Unknown:
                    throw new NotImplementedException();
                default:
                    HashAlgorithm hashAlg = HashAlgorithm.Create(encodeType.ToString());
                    return hashAlg.ComputeHash(dataStream);
            }
        }

        /// <summary>
        /// 把字符串转换为字节
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="encoding">转换后文本编码</param>
        /// <returns>目标数据</returns>
        private static byte[] StringToBytes(string source, Encoding encoding = null)
        {
            encoding = encoding ?? Encoding.UTF8;
            return encoding.GetBytes(source);
        }

        /// <summary>
        /// 二进制字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="data">二进制字节数组</param>
        /// <param name="locase">是否小写</param>
        /// <returns></returns>
        public static string BytesToHex(byte[] data, bool locase = false)
        {
            string format = locase ? "x2" : "X2";
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                builder.Append(data[i].ToString(format));
            return builder.ToString();
        }
    }
}
