using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading;
using Chenyuan.Collections;

namespace Chenyuan.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public static class TypeHelper
    {
        private static readonly Dictionary<Type, TryGetValueDelegate> _tryGetValueDelegateCache = new Dictionary<Type, TryGetValueDelegate>();
        private static readonly ReaderWriterLockSlim _tryGetValueDelegateCacheLock = new ReaderWriterLockSlim();
        private static readonly MethodInfo _strongTryGetValueImplInfo = typeof(TypeHelper).GetMethod("StrongTryGetValueImpl", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ObjectValueDictionary ObjectToDictionary(object value)
        {
            ObjectValueDictionary routeValueDictionary = new ObjectValueDictionary();
            if (value != null)
            {
                PropertyHelper[] properties = PropertyHelper.GetProperties(value);
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyHelper propertyHelper = properties[i];
                    routeValueDictionary.Add(propertyHelper.Name, propertyHelper.GetValue(value));
                }
            }
            return routeValueDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static ObjectValueDictionary ObjectToDictionaryUncached(object value)
        {
            ObjectValueDictionary routeValueDictionary = new ObjectValueDictionary();
            if (value != null)
            {
                PropertyHelper[] properties = PropertyHelper.GetProperties(value);
                for (int i = 0; i < properties.Length; i++)
                {
                    PropertyHelper propertyHelper = properties[i];
                    routeValueDictionary.Add(propertyHelper.Name, propertyHelper.GetValue(value));
                }
            }
            return routeValueDictionary;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dictionary"></param>
        /// <param name="value"></param>
        public static void AddAnonymousObjectToDictionary(IDictionary<string, object> dictionary, object value)
        {
            ObjectValueDictionary objectValueDictionary = TypeHelper.ObjectToDictionary(value);
            foreach (KeyValuePair<string, object> current in objectValueDictionary)
            {
                dictionary.Add(current);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsAnonymousType(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (Attribute.IsDefined(type, typeof(CompilerGeneratedAttribute), false) && type.IsGenericType && type.Name.Contains("AnonymousType") && (type.Name.StartsWith("<>", StringComparison.OrdinalIgnoreCase) || type.Name.StartsWith("VB$", StringComparison.OrdinalIgnoreCase)))
            {
                TypeAttributes arg_6D_0 = type.Attributes;
                return 0 == 0;
            }
            return false;
        }

        public static object GetDefaultValue(Type type)
        {
            if (!TypeAllowsNullValue(type))
            {
                return Activator.CreateInstance(type);
            }
            return null;
        }

        public static bool IsNullableValueType(Type type)
        {
            return Nullable.GetUnderlyingType(type) != null;
        }

        public static bool TypeAllowsNullValue(Type type)
        {
            return !type.IsValueType || IsNullableValueType(type);
        }

        public static bool IsCompatibleObject<T>(object value)
        {
            return value is T || (value == null && TypeAllowsNullValue(typeof(T)));
        }

        public static Type ExtractGenericInterface(Type queryType, Type interfaceType)
        {
            if (MatchesGenericType(queryType, interfaceType))
            {
                return queryType;
            }
            Type[] interfaces = queryType.GetInterfaces();
            return MatchGenericTypeFirstOrDefault(interfaces, interfaceType);
        }

        public static MissingMethodException EnsureDebuggableException(MissingMethodException originalException, string fullTypeName)
        {
            MissingMethodException result = null;
            if (!originalException.Message.Contains(fullTypeName))
            {
                string message = string.Format(CultureInfo.CurrentCulture, Resource.TypeHelpers_CannotCreateInstance, new object[]
                {
                    originalException.Message,
                    fullTypeName
                });
                result = new MissingMethodException(message, originalException);
            }
            return result;
        }

        public static TryGetValueDelegate CreateTryGetValueDelegate(Type targetType)
        {
            _tryGetValueDelegateCacheLock.EnterReadLock();
            TryGetValueDelegate tryGetValueDelegate;
            try
            {
                if (_tryGetValueDelegateCache.TryGetValue(targetType, out tryGetValueDelegate))
                {
                    return tryGetValueDelegate;
                }
            }
            finally
            {
                _tryGetValueDelegateCacheLock.ExitReadLock();
            }
            Type type = ExtractGenericInterface(targetType, typeof(IDictionary<,>));
            if (type != null)
            {
                Type[] genericArguments = type.GetGenericArguments();
                Type type2 = genericArguments[0];
                Type type3 = genericArguments[1];
                if (type2.IsAssignableFrom(typeof(string)))
                {
                    MethodInfo method = _strongTryGetValueImplInfo.MakeGenericMethod(new Type[]
                    {
                        type2,
                        type3
                    });
                    tryGetValueDelegate = (TryGetValueDelegate)Delegate.CreateDelegate(typeof(TryGetValueDelegate), method);
                }
            }
            if (tryGetValueDelegate == null && typeof(IDictionary).IsAssignableFrom(targetType))
            {
                tryGetValueDelegate = new TryGetValueDelegate(TryGetValueFromNonGenericDictionary);
            }
            _tryGetValueDelegateCacheLock.EnterWriteLock();
            try
            {
                _tryGetValueDelegateCache[targetType] = tryGetValueDelegate;
            }
            finally
            {
                _tryGetValueDelegateCacheLock.ExitWriteLock();
            }
            return tryGetValueDelegate;
        }

        public static TDelegate CreateDelegate<TDelegate>(Assembly assembly, string typeName, string methodName, object thisParameter) where TDelegate : class
        {
            Type type = assembly.GetType(typeName, false);
            if (type == null)
            {
                return default(TDelegate);
            }
            return CreateDelegate<TDelegate>(type, methodName, thisParameter);
        }

        public static TDelegate CreateDelegate<TDelegate>(Type targetType, string methodName, object thisParameter) where TDelegate : class
        {
            ParameterInfo[] parameters = typeof(TDelegate).GetMethod("Invoke").GetParameters();
            Type[] types = Array.ConvertAll<ParameterInfo, Type>(parameters, (ParameterInfo pInfo) => pInfo.ParameterType);
            MethodInfo method = targetType.GetMethod(methodName, types);
            if (method == null)
            {
                return default(TDelegate);
            }
            return Delegate.CreateDelegate(typeof(TDelegate), thisParameter, method, false) as TDelegate;
        }

        private static bool MatchesGenericType(Type type, Type matchType)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == matchType;
        }

        private static Type MatchGenericTypeFirstOrDefault(Type[] types, Type matchType)
        {
            for (int i = 0; i < types.Length; i++)
            {
                Type type = types[i];
                if (MatchesGenericType(type, matchType))
                {
                    return type;
                }
            }
            return null;
        }

        private static bool StrongTryGetValueImpl<TKey, TValue>(object dictionary, string key, out object value)
        {
            IDictionary<TKey, TValue> dictionary2 = (IDictionary<TKey, TValue>)dictionary;
            TValue tValue;
            bool result = dictionary2.TryGetValue((TKey)((object)key), out tValue);
            value = tValue;
            return result;
        }

        private static bool TryGetValueFromNonGenericDictionary(object dictionary, string key, out object value)
        {
            IDictionary dictionary2 = (IDictionary)dictionary;
            bool flag = dictionary2.Contains(key);
            value = (flag ? dictionary2[key] : null);
            return flag;
        }
    }
}
