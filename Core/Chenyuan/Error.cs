using Chenyuan.Extensions;
using Chenyuan.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    /// <summary>
    /// 
    /// </summary>
    public static class Error
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception Application(string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="innerException"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception Application(Exception innerException, string message, params object[] args)
        {
            return new ApplicationException(message.FormatCurrent(args), innerException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNullOrEmpty(Func<string> arg)
        {
            var argName = arg.Method.Name;
            return new ArgumentException("String parameter '{0}' cannot be null or all whitespace.", argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argName"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNull(string argName)
        {
            return new ArgumentNullException(argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentNull<T>(Func<T> arg)
        {
            var message = "Argument of type '{0}' cannot be null".FormatInvariant(typeof(T));
            var argName = arg.Method.Name;
            return new ArgumentNullException(argName, message);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange<T>(Func<T> arg)
        {
            var argName = arg.Method.Name;
            return new ArgumentOutOfRangeException(argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argName"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName)
        {
            return new ArgumentOutOfRangeException(argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ArgumentOutOfRange(string argName, string message, params object[] args)
        {
            return new ArgumentOutOfRangeException(argName, String.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="argName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception Argument(string argName, string message, params object[] args)
        {
            return new ArgumentException(String.Format(CultureInfo.CurrentCulture, message, args), argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception Argument<T>(Func<T> arg, string message, params object[] args)
        {
            var argName = arg.Method.Name;
            return new ArgumentException(message.FormatCurrent(args), argName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, params object[] args)
        {
            return Error.InvalidOperation(message, null, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation(string message, Exception innerException, params object[] args)
        {
            return new InvalidOperationException(message.FormatCurrent(args), innerException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Func<T> member)
        {
            return InvalidOperation<T>(message, null, member);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidOperation<T>(string message, Exception innerException, Func<T> member)
        {
            Assert.NotNull(message, nameof(message));
            Assert.NotNull(member, nameof(member));

            return new InvalidOperationException(message.FormatCurrent(member.Method.Name), innerException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType)
        {
            return InvalidCast(fromType, toType, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fromType"></param>
        /// <param name="toType"></param>
        /// <param name="innerException"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception InvalidCast(Type fromType, Type toType, Exception innerException)
        {
            return new InvalidCastException("Cannot convert from type '{0}' to '{1}'.".FormatCurrent(fromType.FullName, toType.FullName), innerException);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception NotSupported()
        {
            return new NotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception NotImplemented()
        {
            return new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName)
        {
            return new ObjectDisposedException(objectName);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectName"></param>
        /// <param name="message"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception ObjectDisposed(string objectName, string message, params object[] args)
        {
            return new ObjectDisposedException(objectName, String.Format(CultureInfo.CurrentCulture, message, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception NoElements()
        {
            return new InvalidOperationException("Sequence contains no elements.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static Exception MoreThanOneElement()
        {
            return new InvalidOperationException("Sequence contains more than one element.");
        }

        public static Exception NotSupported(string v1, object[] v2)
        {
            throw new NotImplementedException();
        }
    }
}
