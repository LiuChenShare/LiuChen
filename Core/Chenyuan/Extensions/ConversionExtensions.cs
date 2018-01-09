using Chenyuan.Converters;
using Chenyuan.Infrastructure;
using Chenyuan.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;

namespace Chenyuan.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ConversionExtensions
    {

        #region Object

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T Convert<T>(this object value)
        {
            return (T)Convert(value, typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static T Convert<T>(this object value, CultureInfo culture)
        {
            return (T)Convert(value, typeof(T), culture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public static object Convert(this object value, Type to)
        {
            return value.Convert(to, CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="to"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public static object Convert(this object value, Type to, CultureInfo culture)
        {
            Assert.NotNull(to, nameof(to));

            if (value == null || to.IsInstanceOfType(value))
            {
                return value;
            }

            Array valueAsArray = value as Array;
            if (to.IsArray)
            {
                Type destinationElementType = to.GetElementType();
                if (valueAsArray != null)
                {
                    IList valueAsList = (IList)valueAsArray;
                    IList converted = Array.CreateInstance(destinationElementType, valueAsList.Count);
                    for (int i = 0; i < valueAsList.Count; i++)
                    {
                        converted[i] = valueAsList[i].Convert(destinationElementType, culture);
                    }
                    return converted;
                }
                else
                {
                    object element = value.Convert(destinationElementType, culture);
                    IList converted = Array.CreateInstance(destinationElementType, 1);
                    converted[0] = element;
                    return converted;
                }
            }
            else if (valueAsArray != null)
            {
                IList valueAsList = (IList)valueAsArray;
                if (valueAsList.Count > 0)
                {
                    value = valueAsList[0];
                }
            }

            Type fromType = value.GetType();

            if (to.IsInterface || to.IsGenericTypeDefinition || to.IsAbstract)
                throw Error.Argument("to", "Target type '{0}' is not a value type or a non-abstract class.", to.FullName);

            if (value is IConvertible && typeof(IConvertible).IsAssignableFrom(to))
            {
                if (to.IsEnum)
                {
                    if (value is string)
                        return Enum.Parse(to, value.ToString(), true);
                    else if (fromType.IsInteger())
                        return Enum.ToObject(to, value);
                }

                return System.Convert.ChangeType(value, to, culture);
            }

            if (value is DateTime && to == typeof(DateTimeOffset))
                return new DateTimeOffset((DateTime)value);

            if (value is string && to == typeof(Guid))
                return new Guid((string)value);

            // see if source or target types have a TypeConverter that converts between the two
            TypeConverter toConverter = TypeDescriptor.GetConverter(fromType);

            if (toConverter != null && toConverter.CanConvertTo(to))
            {
                return toConverter.ConvertTo(null, culture, value, to);
            }

            TypeConverter fromConverter = TypeDescriptor.GetConverter(to);

            if (fromConverter != null && fromConverter.CanConvertFrom(fromType))
            {
                return fromConverter.ConvertFrom(null, culture, value);
            }

            throw Error.InvalidCast(fromType, to);

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public static bool TryConvert<T>(this object value, out T convertedValue)
        {
            return value.TryConvert<T>(CultureInfo.CurrentCulture, out convertedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="culture"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public static bool TryConvert<T>(this object value, CultureInfo culture, out T convertedValue)
        {
            return Misc.TryAction<T>(delegate
            {
                return value.Convert<T>(culture);
            }, out convertedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="to"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public static bool TryConvert(this object value, Type to, out object convertedValue)
        {
            return value.TryConvert(to, CultureInfo.CurrentCulture, out convertedValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="to"></param>
        /// <param name="culture"></param>
        /// <param name="convertedValue"></param>
        /// <returns></returns>
        public static bool TryConvert(this object value, Type to, CultureInfo culture, out object convertedValue)
        {
            return Misc.TryAction<object>(delegate {
                return value.Convert(to, culture);
            }, out convertedValue);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToChild<T>(this object value) where T : new()
        {
            T model = new T();
            var ParentType = value.GetType();
            var Properties = ParentType.GetProperties();
            foreach (var Propertie in Properties)
            {
                if (Propertie.CanRead && Propertie.CanWrite)
                {
                    Propertie.SetValue(model, Propertie.GetValue(value, null), null);
                }
            }
            return model;
        }

        #endregion

        #region int

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static char ToHex(this int value)
        {
            if (value <= 9)
            {
                return (char)(value + 48);
            }
            return (char)((value - 10) + 97);
        }

        /// <summary>
        /// Returns kilobytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToKb(this int value)
        {
            return value * 1024;
        }

        /// <summary>
        /// Returns megabytes
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static int ToMb(this int value)
        {
            return value * 1024 * 1024;
        }

        /// <summary>Returns a <see cref="TimeSpan"/> that represents a specified number of minutes.</summary>
        /// <param name="minutes">number of minutes</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        /// <example>3.Minutes()</example>
        public static TimeSpan ToMinutes(this int minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of seconds.
        /// </summary>
        /// <param name="seconds">number of seconds</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        /// <example>2.Seconds()</example>
        public static TimeSpan ToSeconds(this int seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of milliseconds.
        /// </summary>
        /// <param name="milliseconds">milliseconds for this timespan</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        public static TimeSpan ToMilliseconds(this int milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of days.
        /// </summary>
        /// <param name="days">Number of days.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        public static TimeSpan ToDays(this int days)
        {
            return TimeSpan.FromDays(days);
        }


        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of hours.
        /// </summary>
        /// <param name="hours">Number of hours.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        public static TimeSpan ToHours(this int hours)
        {
            return TimeSpan.FromHours(hours);
        }

        #endregion

        #region double

        /// <summary>Returns a <see cref="TimeSpan"/> that represents a specified number of minutes.</summary>
        /// <param name="minutes">number of minutes</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        /// <example>3D.Minutes()</example>
        public static TimeSpan ToMinutes(this double minutes)
        {
            return TimeSpan.FromMinutes(minutes);
        }


        /// <summary>Returns a <see cref="TimeSpan"/> that represents a specified number of hours.</summary>
        /// <param name="hours">number of hours</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        /// <example>3D.Hours()</example>
        public static TimeSpan ToHours(this double hours)
        {
            return TimeSpan.FromHours(hours);
        }

        /// <summary>Returns a <see cref="TimeSpan"/> that represents a specified number of seconds.</summary>
        /// <param name="seconds">number of seconds</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        /// <example>2D.Seconds()</example>
        public static TimeSpan ToSeconds(this double seconds)
        {
            return TimeSpan.FromSeconds(seconds);
        }

        /// <summary>Returns a <see cref="TimeSpan"/> that represents a specified number of milliseconds.</summary>
        /// <param name="milliseconds">milliseconds for this timespan</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        public static TimeSpan ToMilliseconds(this double milliseconds)
        {
            return TimeSpan.FromMilliseconds(milliseconds);
        }

        /// <summary>
        /// Returns a <see cref="TimeSpan"/> that represents a specified number of days.
        /// </summary>
        /// <param name="days">Number of days, accurate to the milliseconds.</param>
        /// <returns>A <see cref="TimeSpan"/> that represents a value.</returns>
        public static TimeSpan ToDays(this double days)
        {
            return TimeSpan.FromDays(days);
        }

        #endregion

        #region String

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static T ToEnum<T>(this string value, T defaultValue) where T : IComparable, IFormattable
        {
            T convertedValue = defaultValue;

            if (!string.IsNullOrEmpty(value))
            {
                try
                {
                    convertedValue = (T)Enum.Parse(typeof(T), value.Trim(), true);
                }
                catch (ArgumentException)
                {
                }
            }

            return convertedValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string[] ToArray(this string value)
        {
            return value.ToArray(new char[] { ',' });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ToArray(this string value, params char[] separator)
        {
            return value.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static int ToInt(this string value, int defaultValue = 0)
        {
            int result;
            if (int.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(this string value, float defaultValue = 0)
        {
            float result;
            if (float.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static bool ToBool(this string value, bool defaultValue = false)
        {
            bool result;
            if (bool.TryParse(value, out result))
            {
                return result;
            }
            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, DateTime? defaultValue)
        {
            return value.ToDateTime(null, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, string[] formats, DateTime? defaultValue)
        {
            return value.ToDateTime(formats, DateTimeFormatInfo.InvariantInfo, DateTimeStyles.AllowWhiteSpaces, defaultValue);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="formats"></param>
        /// <param name="provider"></param>
        /// <param name="styles"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static DateTime? ToDateTime(this string value, string[] formats, IFormatProvider provider, DateTimeStyles styles, DateTime? defaultValue)
        {
            DateTime result;

            if (formats.IsNullOrEmpty())
            {
                if (DateTime.TryParse(value, provider, styles, out result))
                {
                    return result;
                }
            }

            if (DateTime.TryParseExact(value, formats, provider, styles, out result))
            {
                return result;
            }

            return defaultValue;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Guid ToGuid(this string value)
        {
            if ((!String.IsNullOrEmpty(value)) && (value.Trim().Length == 22))
            {
                string encoded = string.Concat(value.Trim().Replace("-", "+").Replace("_", "/"), "==");

                byte[] base64 = System.Convert.FromBase64String(encoded);

                return new Guid(base64);
            }

            return Guid.Empty;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this string value)
        {
            return Encoding.Default.GetBytes(value);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <param name="defaultVersion"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Version ToVersion(this string value, Version defaultVersion = null)
        {
            try
            {
                return new Version(value);
            }
            catch
            {
                return defaultVersion ?? new Version("1.0");
            }
        }

        #endregion

        #region Stream

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Stream stream)
        {
            Assert.NotNull(stream, "stream");

            byte[] buffer;

            if (stream is MemoryStream && stream.CanRead && stream.CanSeek)
            {
                int len = System.Convert.ToInt32(stream.Length);
                buffer = new byte[len];
                stream.Read(buffer, 0, len);
                return buffer;
            }

            MemoryStream memStream = null;
            try
            {
                buffer = new byte[1024];
                memStream = new MemoryStream();
                int bytesRead = stream.Read(buffer, 0, buffer.Length);
                if (bytesRead > 0)
                {
                    memStream.Write(buffer, 0, bytesRead);
                    bytesRead = stream.Read(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                if (memStream != null)
                    memStream.Close();
            }

            if (memStream != null)
            {
                return memStream.ToArray();
            }

            return null;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string AsString(this Stream stream)
        {
            string result;
            stream.Position = 0;

            using (StreamReader sr = new StreamReader(stream))
            {
                result = sr.ReadToEnd();
            }

            return result;

        }

        #endregion

        #region ByteArray

        /// <summary>
        /// Converts a byte array into an object.
        /// </summary>
        /// <param name="bytes">Object to deserialize. May be null.</param>
        /// <returns>Deserialized object, or null if input was null.</returns>
        public static object ToObject(this byte[] bytes)
        {
            if (bytes == null)
                return null;

            using (MemoryStream stream = new MemoryStream(bytes))
            {
                return new BinaryFormatter().Deserialize(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Image ToImage(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                return Image.FromStream(stream);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static Stream ToStream(this byte[] bytes)
        {
            return new MemoryStream(bytes);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static string AsString(this byte[] bytes)
        {
            return Encoding.Default.GetString(bytes);
        }

        /// <summary>
        /// Computes the MD5 hash of a byte array
        /// </summary>
        /// <param name="value">The byte array to compute the hash for</param>
        /// <param name="toBase64"></param>
        /// <returns>The hash value</returns>
        //[DebuggerStepThrough]
        public static string Hash(this byte[] value, bool toBase64 = false)
        {
            Assert.NotNull(value, "value");

            if (toBase64)
            {
                return EncodeUtility.SecurityEncodeToBase64(HashEncodeType.MD5, value);
            }
            return EncodeUtility.SecurityEncodeToHex(HashEncodeType.MD5, value, true);
        }

        #endregion

        #region Image/Bitmap

        /// <summary>
        /// 
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ToByteArray(this Image image)
        {
            Assert.NotNull(image, nameof(image));

            byte[] bytes;

            ImageConverter converter = new ImageConverter();
            bytes = (byte[])converter.ConvertTo(image, typeof(byte[]));
            return bytes;
        }

        internal static byte[] ToByteArray(this Image image, ImageFormat format)
        {
            Assert.NotNull(image, nameof(image));
            Assert.NotNull(format, nameof(format));

            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return stream.ToByteArray();
            }
        }

        internal static Image ConvertTo(this Image image, ImageFormat format)
        {
            Assert.NotNull(image, nameof(image));
            Assert.NotNull(format, nameof(format));

            using (var stream = new MemoryStream())
            {
                image.Save(stream, format);
                return Image.FromStream(stream);
            }
        }

        #endregion

        #region Enumerable: Collections/List/Dictionary...

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="values"></param>
        /// <returns></returns>
        public static T ToObject<T>(this IDictionary<string, object> values) where T : class
        {
            return (T)values.ToObject(typeof(T));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="values"></param>
        /// <param name="objectType"></param>
        /// <returns></returns>
        public static object ToObject(this IDictionary<string, object> values, Type objectType)
        {
            Assert.NotEmpty(values, "values");
            Assert.NotNull(objectType, "objectType");

            if (!DictionaryConverter.CanCreateType(objectType))
            {
                throw Error.Argument(
                    "objectType",
                    "The type '{0}' must be a class and have a parameterless default constructor in order to deserialize properly.",
                    objectType.FullName);
            }

            return DictionaryConverter.SafeCreateAndPopulate(objectType, values);
        }

        #endregion

    }
}
