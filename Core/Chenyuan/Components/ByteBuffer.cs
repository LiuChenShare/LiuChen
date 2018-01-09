using System;
using System.Collections.Generic;
using System.Text;

namespace Chenyuan.Components
{
    /// <summary>
    /// 字节缓存类
    /// </summary>
	public sealed class ByteBuffer
	{
		private int _currentLength;
		private readonly int _maxLength;
		private readonly List<byte[]> _segments = new List<byte[]>();

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="maxLength">最大长度</param>
		public ByteBuffer(int maxLength)
		{
			_maxLength = maxLength;
		}

        /// <summary>
        /// 追加字节数据
        /// </summary>
        /// <param name="segment"></param>
		public void Append(byte[] segment)
		{
			checked
			{
                int length = _currentLength + segment.Length;
				if (length > _maxLength)
				{
					throw new InvalidOperationException("out of range");
				}
                _currentLength += length;
                _segments.Add(segment);
			}
		}

        /// <summary>
        /// 获取字节数组
        /// </summary>
        /// <returns></returns>
		public byte[] GetByteArray()
		{
			byte[] array = new byte[_currentLength];
			int num = 0;
			for (int i = 0; i < _segments.Count; i++)
			{
				byte[] array2 = _segments[i];
				Buffer.BlockCopy(array2, 0, array, num, array2.Length);
				num += array2.Length;
			}
			return array;
		}

        /// <summary>
        /// 获取字符串内容
        /// </summary>
        /// <returns></returns>
		public string GetString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Decoder decoder = Encoding.UTF8.GetDecoder();
			for (int i = 0; i < _segments.Count; i++)
			{
				bool flush = i == _segments.Count - 1;
				byte[] array = _segments[i];
				int charCount = decoder.GetCharCount(array, 0, array.Length, flush);
				char[] array2 = new char[charCount];
				int chars = decoder.GetChars(array, 0, array.Length, array2, 0, flush);
				stringBuilder.Append(array2, 0, chars);
			}
			return stringBuilder.ToString();
		}
	}
}
