using Chenyuan.Extensions;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Utilities
{
    /// <summary>
    /// 诊断服务类定义
    /// </summary>
    public static class Assert
    {
        private const string AgainstMessage = "Assertion evaluation failed with 'false'.";
        private const string ImplementsMessage = "Type '{0}' must implement type '{1}'.";
        private const string InheritsFromMessage = "Type '{0}' must inherit from type '{1}'.";
        private const string IsTypeOfMessage = "Type '{0}' must be of type '{1}'.";
        private const string IsEqualMessage = "Compared objects must be equal.";
        private const string IsPositiveMessage = "Argument '{0}' must be a positive value. Value: '{1}'.";
        private const string IsTrueMessage = "True expected for '{0}' but the condition was False.";
        private const string NotNegativeMessage = "Argument '{0}' cannot be a negative value. Value: '{1}'.";

        /// <summary>
        /// 诊断对象是否为空
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="argument">对象实例</param>
        /// <param name="argumentName">对象参数名</param>
        public static void NotNull<T>(T argument, string argumentName) //where T : class
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName, $"{argumentName} should not be null.");
        }

        /// <summary>
        /// 判断一个字符串是否为空
        /// </summary>
        /// <param name="argument">对象实例</param>
        /// <param name="argumentName">对象参数名</param>
        public static void NotEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argumentName, $"{argumentName} should not be empty or null.");
        }

        /// <summary>
        /// 诊断一个集合是否为空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void NotEmpty<T>(IEnumerable<T> argument, string argumentName)
        {
            if (argument == null)
                throw new ArgumentNullException(argumentName, $"{argumentName} should not be null.");
            if (!argument.Any())
                throw new ArgumentNullException(argumentName, $"{argumentName} should not be empty.");
        }

        /// <summary>
        /// 诊断字符串对象是否有非空白内容
        /// </summary>
        /// <param name="argument"></param>
        /// <param name="argumentName"></param>
        public static void NotNullOrEmpty(string argument, string argumentName)
        {
            if (string.IsNullOrEmpty(argument))
                throw new ArgumentNullException(argument, $"{argumentName} should not be null or empty.");
        }

        /// <summary>
        /// 诊断一个整数是否为正数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Positive(int number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be positive.");
        }

        /// <summary>
        /// 诊断一个长整数是否为正数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Positive(long number, string argumentName)
        {
            if (number <= 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be positive.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="arg"></param>
        /// <param name="argName"></param>
        /// <param name="message"></param>
        [DebuggerStepThrough]
        public static void Positive<T>(T arg, string argName, string message = IsPositiveMessage)
            where T : struct, IComparable<T>
        {
            if (arg.CompareTo(default(T)) < 1)
                throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName));
        }


        /// <summary>
        /// 诊断一个长整数是否为非负数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Nonnegative(long number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be non negative.");
        }

        /// <summary>
        /// 诊断一个整数是否为非负数
        /// </summary>
        /// <param name="number"></param>
        /// <param name="argumentName"></param>
        public static void Nonnegative(int number, string argumentName)
        {
            if (number < 0)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} should be non negative.");
        }

        /// <summary>
        /// 诊断一个Guid是否非空
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="argumentName"></param>
        public static void NotEmptyGuid(Guid guid, string argumentName)
        {
            if (Guid.Empty == guid)
                throw new ArgumentException(argumentName, $"{argumentName} shoud be non-empty GUID.");
        }

        /// <summary>
        /// 相等诊断
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(int expected, int actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} expected value: {expected}, actual value: {actual}");
        }

        /// <summary>
        /// 相等诊断
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(long expected, long actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} expected value: {expected}, actual value: {actual}");
        }

        /// <summary>
        /// 相等诊断
        /// </summary>
        /// <param name="expected"></param>
        /// <param name="actual"></param>
        /// <param name="argumentName"></param>
        public static void Equal(bool expected, bool actual, string argumentName)
        {
            if (expected != actual)
                throw new ArgumentOutOfRangeException(argumentName, $"{argumentName} expected value: {expected}, actual value: {actual}");
        }

        /// <summary>
        /// 类型兼容诊断
        /// </summary>
        /// <typeparam name="TBaseType">基类型</typeparam>
        /// <param name="instance"></param>
        /// <param name="argumentName"></param>
        public static void CompatibleTo<TBaseType>(object instance, string argumentName)
        {
            if (!(instance is TBaseType))
            {
                throw new ArgumentOutOfRangeException(argumentName, $"must compatible to type {typeof(TBaseType)}.");
            }
        }

        /// <summary>
        /// 类型兼容诊断
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="instanceType"></param>
        /// <param name="argumentName"></param>
        public static void CompatibleTo(object instance, Type instanceType, string argumentName)
        {
            if (!instanceType.IsAssignableFrom(instance))
            {
                throw new ArgumentOutOfRangeException(argumentName, $"must compatible to type {instanceType}.");
            }
        }

        /// <summary>
        /// 是否类型兼容
        /// </summary>
        /// <param name="baseType">基类型对象</param>
        /// <param name="implementationType">派生类型对象</param>
        public static void IsCompatiable(Type baseType, Type implementationType)
        {
            if (!baseType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"type {baseType} must compatible to type {implementationType}.");
            }
        }

        /// <summary>
        /// 是否类型兼容
        /// </summary>
        /// <typeparam name="TBaseType">基类型</typeparam>
        /// <typeparam name="TImplementationType">派生类型</typeparam>
        public static void IsCompatiable<TBaseType, TImplementationType>()
        {
            var baseType = typeof(TBaseType);
            var implementationType = typeof(TImplementationType);
            if (!baseType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"type {baseType} must compatible to type {implementationType}.");
            }
        }

        /// <summary>
        /// 是否范类型兼容
        /// </summary>
        /// <param name="baseType">基类型对象</param>
        /// <param name="implementationType">派生类型对象</param>
        public static void IsGenericCompatiable(Type baseType, Type implementationType)
        {
            if (!baseType.IsGenericTypeDefinition() || !implementationType.IsGenericTypeDefinition())
            {
                throw new ArgumentException($"type {baseType} must be generic class defination.");
            }
            if (baseType.GetGenericTypeParameters().Length != implementationType.GetGenericTypeParameters().Length)
            {
                throw new ArgumentException($"generic definitation type {baseType} to {implementationType} has not same type parameters count.");
            }
            //if (!baseType.IsAssignableFrom(implementationType))
            //{
            //    throw new ArgumentException($"type {implementationType} must compatible to type {baseType}.");
            //}
        }

        /// <summary>
        /// 是否范类型兼容
        /// </summary>
        /// <typeparam name="TBaseType">基类型</typeparam>
        /// <typeparam name="TImplementationType">派生类型</typeparam>
        public static void IsGenericCompatiable<TBaseType, TImplementationType>()
        {
            var baseType = typeof(TBaseType);
            var implementationType = typeof(TImplementationType);
            if (!baseType.IsGenericTypeDefinition() || !implementationType.IsGenericTypeDefinition())
            {
                throw new ArgumentException($"type {baseType} must be generic class defination.");
            }
            if (baseType.GetGenericTypeParameters().Length != implementationType.GetGenericTypeParameters().Length)
            {
                throw new ArgumentException($"generic definitation type {baseType} to {implementationType} has not same type parameters count.");
            }
            if (!baseType.IsAssignableFrom(implementationType))
            {
                throw new ArgumentException($"type {implementationType} must compatible to type {baseType}.");
            }
        }
    }
}
