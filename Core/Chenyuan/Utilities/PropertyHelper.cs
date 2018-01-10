using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Chenyuan.Utilities
{
    /// <summary>
    /// 
    /// </summary>
    public class PropertyHelper
    {
        private delegate TValue ByRefFunc<TDeclaringType, TValue>(ref TDeclaringType arg);
        private static ConcurrentDictionary<Type, PropertyHelper[]> _reflectionCache = new ConcurrentDictionary<Type, PropertyHelper[]>();
        private Func<object, object> _valueGetter;
        private static readonly MethodInfo _callPropertyGetterOpenGenericMethod = typeof(PropertyHelper).GetMethod("CallPropertyGetter", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo _callPropertyGetterByReferenceOpenGenericMethod = typeof(PropertyHelper).GetMethod("CallPropertyGetterByReference", BindingFlags.Static | BindingFlags.NonPublic);
        private static readonly MethodInfo _callPropertySetterOpenGenericMethod = typeof(PropertyHelper).GetMethod("CallPropertySetter", BindingFlags.Static | BindingFlags.NonPublic);

        /// <summary>
        /// 
        /// </summary>
        public virtual string Name
        {
            get;
            protected set;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="property"></param>
        public PropertyHelper(PropertyInfo property)
        {
            this.Name = property.Name;
            _valueGetter = PropertyHelper.MakeFastPropertyGetter(property);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TDeclaringType"></typeparam>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Action<TDeclaringType, object> MakeFastPropertySetter<TDeclaringType>(PropertyInfo propertyInfo) where TDeclaringType : class
        {
            MethodInfo setMethod = propertyInfo.GetSetMethod();
            Type reflectedType = propertyInfo.ReflectedType;
            Type parameterType = setMethod.GetParameters()[0].ParameterType;
            Delegate firstArgument = setMethod.CreateDelegate(typeof(Action<,>).MakeGenericType(new Type[]
            {
                reflectedType,
                parameterType
            }));
            MethodInfo method = PropertyHelper._callPropertySetterOpenGenericMethod.MakeGenericMethod(new Type[]
            {
                reflectedType,
                parameterType
            });
            Delegate @delegate = Delegate.CreateDelegate(typeof(Action<TDeclaringType, object>), firstArgument, method);
            return (Action<TDeclaringType, object>)@delegate;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public object GetValue(object instance)
        {
            return _valueGetter(instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public static PropertyHelper[] GetProperties(object instance)
        {
            return PropertyHelper.GetProperties(instance, new Func<PropertyInfo, PropertyHelper>(PropertyHelper.CreateInstance), PropertyHelper._reflectionCache);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static Func<object, object> MakeFastPropertyGetter(PropertyInfo propertyInfo)
        {
            MethodInfo getMethod = propertyInfo.GetGetMethod();
            Type reflectedType = getMethod.ReflectedType;
            Type returnType = getMethod.ReturnType;
            Delegate @delegate;
            if (reflectedType.IsValueType)
            {
                Delegate firstArgument = getMethod.CreateDelegate(typeof(PropertyHelper.ByRefFunc<,>).MakeGenericType(new Type[]
                {
                    reflectedType,
                    returnType
                }));
                MethodInfo method = PropertyHelper._callPropertyGetterByReferenceOpenGenericMethod.MakeGenericMethod(new Type[]
                {
                    reflectedType,
                    returnType
                });
                @delegate = Delegate.CreateDelegate(typeof(Func<object, object>), firstArgument, method);
            }
            else
            {
                Delegate firstArgument2 = getMethod.CreateDelegate(typeof(Func<,>).MakeGenericType(new Type[]
                {
                    reflectedType,
                    returnType
                }));
                MethodInfo method2 = PropertyHelper._callPropertyGetterOpenGenericMethod.MakeGenericMethod(new Type[]
                {
                    reflectedType,
                    returnType
                });
                @delegate = Delegate.CreateDelegate(typeof(Func<object, object>), firstArgument2, method2);
            }
            return (Func<object, object>)@delegate;
        }
        private static PropertyHelper CreateInstance(PropertyInfo property)
        {
            return new PropertyHelper(property);
        }
        private static object CallPropertyGetter<TDeclaringType, TValue>(Func<TDeclaringType, TValue> getter, object @this)
        {
            return getter((TDeclaringType)((object)@this));
        }
        private static object CallPropertyGetterByReference<TDeclaringType, TValue>(PropertyHelper.ByRefFunc<TDeclaringType, TValue> getter, object @this)
        {
            TDeclaringType tDeclaringType = (TDeclaringType)((object)@this);
            return getter(ref tDeclaringType);
        }
        private static void CallPropertySetter<TDeclaringType, TValue>(Action<TDeclaringType, TValue> setter, object @this, object value)
        {
            setter((TDeclaringType)((object)@this), (TValue)((object)value));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="instance"></param>
        /// <param name="createPropertyHelper"></param>
        /// <param name="cache"></param>
        /// <returns></returns>
        protected static PropertyHelper[] GetProperties(object instance, Func<PropertyInfo, PropertyHelper> createPropertyHelper, ConcurrentDictionary<Type, PropertyHelper[]> cache)
        {
            Type type = instance.GetType();
            PropertyHelper[] array;
            if (!cache.TryGetValue(type, out array))
            {
                IEnumerable<PropertyInfo> enumerable =
                    from prop in type.GetProperties(BindingFlags.Instance | BindingFlags.Public)
                    where prop.GetIndexParameters().Length == 0 && prop.GetMethod != null
                    select prop;
                List<PropertyHelper> list = new List<PropertyHelper>();
                foreach (PropertyInfo current in enumerable)
                {
                    PropertyHelper item = createPropertyHelper(current);
                    list.Add(item);
                }
                array = list.ToArray();
                cache.TryAdd(type, array);
            }
            return array;
        }
    }
}
