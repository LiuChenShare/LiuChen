using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.IO;
using Chenyuan.IO;

namespace Chenyuan.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class BinaryExtensions
    {
        /// <summary>
        /// 将字节数组转换为十六进制字符串
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="reverse"></param>
        /// <param name="toUpper"></param>
        /// <returns></returns>
        public static string ToHexString(this byte[] bytes, bool reverse = false, bool toUpper = false)
        {
            if (reverse)
            {
                Array.Reverse(bytes);
            }
            var result = BitConverter.ToString(bytes).Replace("-", "");
            if (toUpper)
            {
                return result.ToUpper();
            }
            return result.ToLower();
            //var format = toUpper ? "{0:X2}" : "{0:x2}";
            //StringBuilder builder = new StringBuilder();
            //foreach(var @byte in bytes)
            //{
            //    builder.AppendFormat("");
            //}
            //return builder.ToString();
        }

        /// <summary>
        /// 将一个字节转换为十六进制字符串
        /// </summary>
        /// <param name="byte">字节数据</param>
        /// <param name="toUpper">是否大写，默认为否</param>
        /// <returns>字节对应的十六进制字符串</returns>
        public static string ToHexString(this byte @byte, bool toUpper = false)
        {
            var format = toUpper ? "{0:X2}" : "{0:x2}";
            return format.FormatWith(@byte);
        }

        /// <summary>
        /// 十六进制字符串转换为字节数组
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     当hex长度不是2的倍数或字符串内容不满足十六进制要求的时候
        /// </exception>
        /// <returns></returns>
        public static byte[] HexToBytes(this string hex)
        {
            if (hex.Length % 2 == 1)
            {
                throw new ArgumentOutOfRangeException("hex");
            }
            byte[] result = new byte[hex.Length / 2];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = (byte)((HexCharToByte(hex[i * 2]) << 4) + HexCharToByte(hex[i * 2 + 1]));
            }
            return result;
        }

        /// <summary>
        /// 十六进制字符串转换为字节
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     当hex长度大于2或字符串内容不满足十六进制要求的时候
        /// </exception>
        /// <returns></returns>
        public static byte HexToByte(this string hex)
        {
            switch (hex.Length)
            {
                case 0:
                    return 0;
                case 1:
                    return HexCharToByte(hex[0]);
                case 2:
                    return (byte)((HexCharToByte(hex[0]) << 4) + HexCharToByte(hex[1]));
                default:
                    throw new ArgumentOutOfRangeException("hex");
            }
        }

        /// <summary>
        /// 十六进制字符转换为字节数据
        /// </summary>
        /// <param name="hex">十六进制字符</param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     当hex长度大于2或字符串内容不满足十六进制要求的时候
        /// </exception>
        /// <returns></returns>
        public static byte HexCharToByte(this char hex)
        {
            if (hex >= '0' && hex <= '9')
            {
                return (byte)(hex - '0');
            }
            if (hex >= 'A' && hex <= 'F')
            {
                return (byte)(hex - 'A');
            }
            if (hex >= 'a' && hex <= 'f')
            {
                return (byte)(hex - 'a');
            }
            throw new ArgumentOutOfRangeException();
        }

        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        /// <param name="pictureStream"></param>
        /// <returns></returns>
        public static Size GetPictureSize(this Stream pictureStream)
        {
            if (pictureStream == null || pictureStream.Length == 0)
            {
                return new Size();
            }

            Size size;

            try
            {
                pictureStream.Seek(0, SeekOrigin.Begin);
                using (var reader = new BinaryReader(pictureStream, Encoding.UTF8, true))
                {
                    size = ImageHeader.GetDimensions(reader);
                }
            }
            catch (Exception)
            {
                // something went wrong with fast image access,
                // so get original size the classic way
                try
                {
                    pictureStream.Seek(0, SeekOrigin.Begin);
                    using (var b = new Bitmap(pictureStream))
                    {
                        size = new Size(b.Width, b.Height);
                    }
                }
                catch
                {
                    size = new Size();
                }
            }
            finally
            {
            }

            return size;
        }

        /// <summary>
        /// 获取图片尺寸
        /// </summary>
        /// <param name="pictureBinary"></param>
        /// <returns></returns>
        public static Size GetPictureSize(this byte[] pictureBinary)
        {
            if (pictureBinary == null || pictureBinary.Length == 0)
            {
                return new Size();
            }

            using (var stream = new MemoryStream(pictureBinary))
            {
                return stream.GetPictureSize();
            }
        }
    }
}
