using Chenyuan.Fasterflect.Extensions.Core;
using Chenyuan.Infrastructure;
using Chenyuan.Utilities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Extensions
{
    /// <summary>
    /// 类型扩展方法
    /// </summary>
    public static partial class TypeExtension
    {
        static MethodInfo s_methodGenericTypeof = null;
        private static Type[] s_predefinedTypes;
        private static Type[] s_predefinedGenericTypes;

        static TypeExtension()
        {
            s_methodGenericTypeof = typeof(TypeExtension).GetRuntimeMethods().Where(m => m.IsGenericMethod && m.Name == "InstanceOf").Single();
            s_predefinedTypes = new Type[] { typeof(string), typeof(decimal), typeof(DateTime), typeof(TimeSpan), typeof(Guid) };
            s_predefinedGenericTypes = new Type[] { typeof(Nullable<>) };
        }

        /// <summary>
        /// 判断指定类型是否可以转换为当前类型
        /// </summary>
        /// <param name="source">当前类型</param>
        /// <param name="dest">原始类型</param>
        /// <returns>执行结果，true：是，false：否</returns>
        public static bool IsAssignableFrom(this Type source, Type dest)
        {
            return source.GetTypeInfo().IsAssignableFrom(dest.GetTypeInfo());
        }

        /// <summary>
        /// 判断是否可以将指定实例对象转换为当前类型的实体
        /// </summary>
        /// <param name="source">当前类型</param>
        /// <param name="instance">目标实例对象</param>
        /// <returns>执行结果，true：是，false：否</returns>
        public static bool IsAssignableFrom(this Type source, object instance)
        {
            return InstanceOf(instance, source);
        }

        /// <summary>
        /// 判断一个类型是否是类类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsClass(this Type type)
        {
            return type.GetTypeInfo().IsClass;
        }

        /// <summary>
        /// 判断一个类型是否是接口类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInterface(this Type type)
        {
            return type.GetTypeInfo().IsInterface;
        }

        /// <summary>
        /// 判断一个类型是否为范类型定义类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericTypeDefinition(this Type type)
        {
            return type.GetTypeInfo().IsGenericTypeDefinition;
        }

        /// <summary>
        /// 判断一个类型是否为范类型类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericType(this Type type)
        {
            return type.GetTypeInfo().IsGenericType;
        }

        /// <summary>
        /// 获取范类型定义类型的类型参数描述信息
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Type[] GetGenericTypeParameters(this Type type)
        {
            return type.GetTypeInfo().GenericTypeParameters;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TInstanceType"></typeparam>
        /// <param name="instance"></param>
        /// <returns></returns>
        private static bool InstanceOf<TInstanceType>(object instance)
        {
            return instance is TInstanceType;
        }

        /// <summary>
        /// 判断实例对象是否可以转换为目标类型
        /// </summary>
        /// <param name="instance">原始实例对象</param>
        /// <param name="instanceType">目标类型</param>
        /// <returns>执行结果，true：是，false：否</returns>
        public static bool InstanceOf(object instance, Type instanceType)
        {
            var method = s_methodGenericTypeof.MakeGenericMethod(instanceType);
            var result = method.Invoke(null, new object[] { instance });
            return (bool)result;
        }

        /// <summary>
        /// 获取类完整名称（不带版本信息）
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string AssemblyQualifiedNameWithoutVersion(this Type type)
        {
            string[] strArray = type.AssemblyQualifiedName.Split(new char[] { ',' });
            return string.Format("{0},{1}", strArray[0], strArray[1]);
        }

        /// <summary>
        /// 指定类型是否可枚举
        /// </summary>
        /// <param name="seqType"></param>
        /// <returns></returns>
        public static bool IsSequenceType(this Type seqType)
        {
            return (
                (((seqType != typeof(string))
                && (seqType != typeof(byte[])))
                && (seqType != typeof(char[])))
                && (FindIEnumerable(seqType) != null));
        }

        /// <summary>
        /// 是否预定义简单类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPredefinedSimpleType(this Type type)
        {
            if ((type.IsPrimitive && (type != typeof(IntPtr))) && (type != typeof(UIntPtr)))
            {
                return true;
            }
            if (type.IsEnum)
            {
                return true;
            }
            return s_predefinedTypes.Any(t => t == type);
            //foreach (Type type2 in s_predefinedTypes)
            //{
            //    if (type2 == type)
            //    {
            //        return true;
            //    }
            //}
            //return false;
        }

        /// <summary>
        /// 是否结构
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsStruct(this Type type)
        {
            if (type.IsValueType)
            {
                return !type.IsPredefinedSimpleType();
            }
            return false;
        }

        /// <summary>
        /// 是否预定义泛类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPredefinedGenericType(this Type type)
        {
            if (type.IsGenericType)
            {
                type = type.GetGenericTypeDefinition();
            }
            else
            {
                return false;
            }
            return s_predefinedGenericTypes.Any(t => t == type);
        }

        /// <summary>
        /// 是否预定义类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsPredefinedType(this Type type)
        {
            if ((!IsPredefinedSimpleType(type) && !IsPredefinedGenericType(type)) && ((type != typeof(byte[]))))
            {
                return (string.Compare(type.FullName, "System.Xml.Linq.XElement", StringComparison.Ordinal) == 0);
            }
            return true;
        }

        /// <summary>
        /// 是否整形
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsInteger(this Type type)
        {

            switch (Type.GetTypeCode(type))
            {
                case TypeCode.SByte:
                case TypeCode.Byte:
                case TypeCode.Int16:
                case TypeCode.UInt16:
                case TypeCode.Int32:
                case TypeCode.UInt32:
                case TypeCode.Int64:
                case TypeCode.UInt64:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 是否可空
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullable(this Type type)
        {
            return type != null && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// 是否试用null赋值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsNullAssignable(this Type type)
        {
            return !type.IsValueType || type.IsNullable();
        }

        /// <summary>
        /// 是否可实例化泪洗过
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsConstructable(this Type type)
        {
            Assert.NotNull(type, nameof(type));

            if (type.IsAbstract || type.IsInterface || type.IsArray || type.IsGenericTypeDefinition || type == typeof(void))
                return false;

            if (!HasDefaultConstructor(type))
                return false;

            return true;
        }

        /// <summary>
        /// 是否匿名类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsAnonymous(this Type type)
        {
            if (type.IsGenericType)
            {
                var d = type.GetGenericTypeDefinition();
                if (d.IsClass && d.IsSealed && d.Attributes.HasFlag(TypeAttributes.NotPublic))
                {
                    var attributes = d.GetCustomAttributes(typeof(System.Runtime.CompilerServices.CompilerGeneratedAttribute), false);
                    if (attributes != null && attributes.Length > 0)
                    {
                        //WOW! We have an anonymous type!!!
                        return true;
                    }
                }
            }
            return false;
        }

        /// <summary>
        /// 是否有默认构造函数
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool HasDefaultConstructor(this Type type)
        {
            Assert.NotNull(type, nameof(type));

            if (type.IsValueType)
                return true;

            return type.GetConstructors(BindingFlags.Instance | BindingFlags.Public)
                .Any(ctor => ctor.GetParameters().Length == 0);
        }

        /// <summary>
        /// 是否子类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="check"></param>
        /// <returns></returns>
        public static bool IsSubClass(this Type type, Type check)
        {
            Type implementingType;
            return IsSubClass(type, check, out implementingType);
        }

        /// <summary>
        /// 是否子类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="check"></param>
        /// <param name="implementingType"></param>
        /// <returns></returns>
        public static bool IsSubClass(this Type type, Type check, out Type implementingType)
        {
            Assert.NotNull(type, nameof(type));
            Assert.NotNull(check, nameof(check));

            return IsSubClassInternal(type, type, check, out implementingType);
        }

        private static bool IsSubClassInternal(Type initialType, Type currentType, Type check, out Type implementingType)
        {
            if (currentType == check)
            {
                implementingType = currentType;
                return true;
            }

            if (check.IsInterface && (initialType.IsInterface || currentType == initialType))
            {
                foreach (Type t in currentType.GetInterfaces())
                {
                    if (IsSubClassInternal(initialType, t, check, out implementingType))
                    {
                        if (check == implementingType)
                            implementingType = currentType;
                        return true;
                    }
                }
            }

            if (currentType.IsGenericType && !currentType.IsGenericTypeDefinition)
            {
                if (IsSubClassInternal(initialType, currentType.GetGenericTypeDefinition(), check, out implementingType))
                {
                    implementingType = currentType;
                    return true;
                }
            }

            if (currentType.BaseType == null)
            {
                implementingType = null;
                return false;
            }

            return IsSubClassInternal(initialType, currentType.BaseType, check, out implementingType);
        }

        /// <summary>
        /// 是否索引属性
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static bool IsIndexed(this PropertyInfo property)
        {
            Assert.NotNull(property, nameof(property));
            return !property.GetIndexParameters().IsNullOrEmpty();
        }

        /// <summary>
        /// Determines whether the member is an indexed property.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <returns>
        /// 	<c>true</c> if the member is an indexed property; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsIndexed(this MemberInfo member)
        {
            Assert.NotNull(member, nameof(member));

            PropertyInfo propertyInfo = member as PropertyInfo;

            if (propertyInfo != null)
                return propertyInfo.IsIndexed();
            else
                return false;
        }

        /// <summary>
        /// Checks to see if the specified type is assignable.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsType<TType>(this Type type)
        {
            return typeof(TType).IsAssignableFrom(type);
        }

        /// <summary>
        /// 是否唯一成员（不存在重载的成员）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="memberTypes"></param>
        /// <returns></returns>
        public static MemberInfo GetSingleMember(this Type type, string name, MemberTypes memberTypes)
        {
            return type.GetSingleMember(
                name,
                memberTypes,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);
        }

        /// <summary>
        /// 是否唯一成员（不存在重载的成员）
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="memberTypes"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static MemberInfo GetSingleMember(this Type type, string name, MemberTypes memberTypes, BindingFlags bindingAttr)
        {
            return type.GetMember(
                name,
                memberTypes,
                bindingAttr).SingleOrDefault();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetNameAndAssemblyName(this Type type)
        {
            Assert.NotNull(type, nameof(type));
            return type.FullName + ", " + type.Assembly.GetName().Name;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="bindingAttr"></param>
        /// <returns></returns>
        public static IEnumerable<MemberInfo> GetFieldsAndProperties(this Type type, BindingFlags bindingAttr)
        {
            foreach (var fi in type.GetFields(bindingAttr))
            {
                yield return fi;
            }

            foreach (var pi in type.GetProperties(bindingAttr))
            {
                yield return pi;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <param name="name"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static MemberInfo GetFieldOrProperty(this Type type, string name, bool ignoreCase)
        {
            BindingFlags flags = BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance;
            if (ignoreCase)
                flags |= BindingFlags.IgnoreCase;

            return type.GetSingleMember(
                name,
                MemberTypes.Field | MemberTypes.Property,
                flags);
        }

        /// <summary>
        /// 根据类型对象获取类型的默认值
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static object GetDefaultValue(this Type type)
        {
            var defaultExpression = Expression.Default(type);
            var lambdaExpression = Expression.Lambda<Func<object>>(defaultExpression);
            var lambdaDelegate = lambdaExpression.Compile();
            return lambdaDelegate();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <param name="source"></param>
        /// <param name="destType"></param>
        /// <returns></returns>
        public static object ConvertTo<TSource>(this TSource source, Type destType)
        {
            Func<TSource> func = () => source;
            return Expression.Lambda<Func<object>>(Expression.Convert(func as Expression, destType)).Compile()();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="targetType"></param>
        /// <param name="memberType"></param>
        /// <param name="bindingAttr"></param>
        /// <param name="filter"></param>
        /// <param name="filterCriteria"></param>
        /// <returns></returns>
        public static List<MemberInfo> FindMembers(this Type targetType, MemberTypes memberType, BindingFlags bindingAttr, MemberFilter filter, object filterCriteria)
        {
            Assert.NotNull(targetType, nameof(targetType));

            List<MemberInfo> memberInfos = new List<MemberInfo>(targetType.FindMembers(memberType, bindingAttr, filter, filterCriteria));

            // fix weirdness with FieldInfos only being returned for the current Type
            // find base type fields and add them to result
            if ((memberType & MemberTypes.Field) != 0
              && (bindingAttr & BindingFlags.NonPublic) != 0)
            {
                // modify flags to not search for public fields
                BindingFlags nonPublicBindingAttr = bindingAttr ^ BindingFlags.Public;

                while ((targetType = targetType.BaseType) != null)
                {
                    memberInfos.AddRange(targetType.FindMembers(MemberTypes.Field, nonPublicBindingAttr, filter, filterCriteria));
                }
            }

            return memberInfos;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <param name="innerType"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateGeneric(this Type genericTypeDefinition, Type innerType, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, new Type[] { innerType }, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <param name="innerTypes"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, params object[] args)
        {
            return CreateGeneric(genericTypeDefinition, innerTypes, (t, a) => Activator.CreateInstance(t, args));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="genericTypeDefinition"></param>
        /// <param name="innerTypes"></param>
        /// <param name="instanceCreator"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public static object CreateGeneric(this Type genericTypeDefinition, Type[] innerTypes, Func<Type, object[], object> instanceCreator, params object[] args)
        {
            Assert.NotNull(genericTypeDefinition, nameof(genericTypeDefinition));
            Assert.NotEmpty(innerTypes, nameof(innerTypes));
            Assert.NotNull(instanceCreator, nameof(instanceCreator));
            Type specificType = genericTypeDefinition.MakeGenericType(innerTypes);

            return instanceCreator(specificType, args);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="listType"></param>
        /// <returns></returns>
        public static IList CreateGenericList(this Type listType)
        {
            Assert.NotNull(listType, nameof(listType));
            return (IList)typeof(List<>).CreateGeneric(listType);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            Assert.NotNull(type, nameof(type));
            return type.IsAssignableFrom(typeof(IEnumerable));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGenericDictionary(this Type type)
        {
            if (type.IsInterface && type.IsGenericType)
            {
                return typeof(IDictionary<,>).Equals(type.GetGenericTypeDefinition());
            }
            return (type.GetInterface(typeof(IDictionary<,>).Name) != null);
        }

        /// <summary>
        /// Gets the member's value on the object.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="target">The target object.</param>
        /// <returns>The member's value on the object.</returns>
        public static object GetValue(this MemberInfo member, object target)
        {
            Assert.NotNull(member, nameof(member));
            Assert.NotNull(target, nameof(target));

            var type = target.GetType();

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return target.GetFieldValue(member.Name);
                case MemberTypes.Property:
                    return target.GetPropertyValue(member.Name);
                default:
                    throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatInvariant(member.Name), "member");
            }
        }

        /// <summary>
        /// Sets the member's value on the target object.
        /// </summary>
        /// <param name="member">The member.</param>
        /// <param name="target">The target.</param>
        /// <param name="value">The value.</param>
        public static void SetValue(this MemberInfo member, object target, object value)
        {
            Assert.NotNull(member, nameof(member));
            Assert.NotNull(target, nameof(target));

            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    target.SetFieldValue(member.Name, value);
                    break;
                //return ((FieldInfo)member).GetValue(target);
                case MemberTypes.Property:
                    try
                    {
                        target.SetPropertyValue(member.Name, value);
                    }
                    catch (TargetParameterCountException e)
                    {
                        throw new ArgumentException("PropertyInfo '{0}' has index parameters".FormatInvariant(member.Name), "member", e);
                    }
                    break;
                default:
                    throw new ArgumentException("MemberInfo '{0}' is not of type FieldInfo or PropertyInfo".FormatInvariant(member.Name), "member");
            }
        }

        /// <summary>
        /// Gets the underlying type of a <see cref="Nullable{T}" /> type.
        /// </summary>
        /// <param name="type"></param>
        public static Type GetNonNullableType(this Type type)
        {
            if (!IsNullable(type))
            {
                return type;
            }
            return type.GetGenericArguments()[0];
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be read.
        /// </summary>
        /// <param name="member">The MemberInfo to determine whether can be read.</param>
        /// <returns>
        /// 	<c>true</c> if the specified MemberInfo can be read; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// For methods this will return <c>true</c> if the return type
        /// is not <c>void</c> and the method is parameterless.
        /// </remarks>
        public static bool CanReadValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanRead;
                case MemberTypes.Method:
                    MethodInfo mi = (MethodInfo)member;
                    return mi.ReturnType != typeof(void) && mi.GetParameters().Length == 0;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Determines whether the specified MemberInfo can be set.
        /// </summary>
        /// <param name="member">The MemberInfo to determine whether can be set.</param>
        /// <returns>
        /// 	<c>true</c> if the specified MemberInfo can be set; otherwise, <c>false</c>.
        /// </returns>
        public static bool CanSetValue(this MemberInfo member)
        {
            switch (member.MemberType)
            {
                case MemberTypes.Field:
                    return true;
                case MemberTypes.Property:
                    return ((PropertyInfo)member).CanWrite;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns single attribute from the type
        /// </summary>
        /// <typeparam name="TAttribute">Attribute to use</typeparam>
        /// <param name="target">Attribute provider</param>
        ///<param name="inherits"><see cref="MemberInfo.GetCustomAttributes(Type,bool)"/></param>
        /// <returns><em>Null</em> if the attribute is not found</returns>
        /// <exception cref="InvalidOperationException">If there are 2 or more attributes</exception>
        public static TAttribute GetAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target.GetCustomAttributes(typeof(TAttribute), inherits);
                if (attributes.Length > 1)
                {
                    throw Error.MoreThanOneElement();
                }
                return (TAttribute)attributes[0];
            }

            return null;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TAttribute"></typeparam>
        /// <param name="target"></param>
        /// <param name="inherits"></param>
        /// <returns></returns>
        public static bool HasAttribute<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            return target.IsDefined(typeof(TAttribute), inherits);
        }

        /// <summary>
        /// Given a particular MemberInfo, return the custom attributes of the
        /// given type on that member.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to retrieve.</typeparam>
        /// <param name="target">The member to look at.</param>
        /// <param name="inherits">True to include attributes inherited from base classes.</param>
        /// <returns>Array of found attributes.</returns>
        public static TAttribute[] GetAttributes<TAttribute>(this ICustomAttributeProvider target, bool inherits) where TAttribute : Attribute
        {
            if (target.IsDefined(typeof(TAttribute), inherits))
            {
                var attributes = target
                    .GetCustomAttributes(typeof(TAttribute), inherits)
                    .Cast<TAttribute>();

                return SortAttributesIfPossible(attributes).ToArray();
            }
            return new TAttribute[0];
        }

        /// <summary>
        /// Given a particular MemberInfo, find all the attributes that apply to this
        /// member. Specifically, it returns the attributes on the type, then (if it's a
        /// property accessor) on the property, then on the member itself.
        /// </summary>
        /// <typeparam name="TAttribute">Type of attribute to retrieve.</typeparam>
        /// <param name="member">The member to look at.</param>
        /// <param name="inherits">true to include attributes inherited from base classes.</param>
        /// <returns>Array of found attributes.</returns>
        public static TAttribute[] GetAllAttributes<TAttribute>(this MemberInfo member, bool inherits)
            where TAttribute : Attribute
        {
            List<TAttribute> attributes = new List<TAttribute>();

            if (member.DeclaringType != null)
            {
                attributes.AddRange(GetAttributes<TAttribute>(member.DeclaringType, inherits));

                MethodBase methodBase = member as MethodBase;
                if (methodBase != null)
                {
                    PropertyInfo prop = GetPropertyFromMethod(methodBase);
                    if (prop != null)
                    {
                        attributes.AddRange(GetAttributes<TAttribute>(prop, inherits));
                    }
                }
            }
            attributes.AddRange(GetAttributes<TAttribute>(member, inherits));
            return attributes.ToArray();
        }

        internal static IEnumerable<TAttribute> SortAttributesIfPossible<TAttribute>(IEnumerable<TAttribute> attributes)
            where TAttribute : Attribute
        {
            if (typeof(IOrdered).IsAssignableFrom(typeof(TAttribute)))
            {
                return attributes.Cast<IOrdered>().OrderBy(x => x.Ordinal).Cast<TAttribute>();
            }

            return attributes;
        }

        /// <summary>
        /// Given a MethodBase for a property's get or set method,
        /// return the corresponding property info.
        /// </summary>
        /// <param name="method">MethodBase for the property's get or set method.</param>
        /// <returns>PropertyInfo for the property, or null if method is not part of a property.</returns>
        public static PropertyInfo GetPropertyFromMethod(this MethodBase method)
        {
            Assert.NotNull(method, nameof(method));

            PropertyInfo property = null;
            if (method.IsSpecialName)
            {
                Type containingType = method.DeclaringType;
                if (containingType != null)
                {
                    if (method.Name.StartsWith("get_", StringComparison.InvariantCulture) ||
                        method.Name.StartsWith("set_", StringComparison.InvariantCulture))
                    {
                        string propertyName = method.Name.Substring(4);
                        property = containingType.GetProperty(propertyName);
                    }
                }
            }
            return property;
        }

        internal static Type FindIEnumerable(this Type seqType)
        {
            if (seqType == null || seqType == typeof(string))
                return null;
            if (seqType.IsArray)
                return typeof(IEnumerable<>).MakeGenericType(seqType.GetElementType());
            if (seqType.IsGenericType)
            {
                foreach (Type arg in seqType.GetGenericArguments())
                {
                    Type ienum = typeof(IEnumerable<>).MakeGenericType(arg);
                    if (ienum.IsAssignableFrom(seqType))
                        return ienum;
                }
            }
            Type[] ifaces = seqType.GetInterfaces();
            if (ifaces != null && ifaces.Length > 0)
            {
                foreach (Type iface in ifaces)
                {
                    Type ienum = FindIEnumerable(iface);
                    if (ienum != null)
                        return ienum;
                }
            }
            if (seqType.BaseType != null && seqType.BaseType != typeof(object))
                return FindIEnumerable(seqType.BaseType);
            return null;
        }

        /// <summary>
        /// 是否类型兼容判断
        /// </summary>
        /// <param name="source">源类型</param>
        /// <param name="genericType">目标类型</param>
        /// <returns>判断结果</returns>
        /// <remarks>
        ///     如果能把source类型赋值给genericType，则兼容
        ///     否则，如果genericType不是泛类型，则不兼容
        ///     如果genericType是类，而source是接口，则不兼容
        ///     如果source是接口或genericType是接口，则判断
        /// </remarks>
        public static bool IsCompatibleToGenericType(this Type source, Type genericType)
        {
            if (genericType.IsAssignableFrom(source))
            {
                return true;
            }
            if (!genericType.IsGenericType)
            {
                return false;
            }
            if (genericType.IsClass && source.IsInterface)
            {
                return false;
            }
            if (!genericType.ContainsGenericParameters)
            {
                genericType = genericType.GetGenericTypeDefinition();
            }
            if (genericType.IsInterface || source.IsInterface)
            {
                if (source.ContainsGenericParameters)
                {
                    source = source.GetGenericTypeDefinition();
                }
                if (source == genericType)
                {
                    return true;
                }
                var interfaces = source.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface == genericType)
                    {
                        return true;
                    }
                }
            }
            else
            {
                while (source != typeof(object))
                {
                    if (source.IsGenericType)
                    {
                        if (source.GetGenericTypeDefinition() == genericType)
                        {
                            return true;
                        }
                    }
                    source = source.BaseType;
                }
            }
            return false;
        }

        /// <summary>
        /// 对指定类型，获取其实现泛类型的类型参数集合
        /// </summary>
        /// <param name="source"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static Type[] GetCompatibleGenericTypeParameterTypes(this Type source, Type genericType)
        {
            if (!genericType.IsGenericType || !source.IsCompatibleToGenericType(genericType))
            {
                return new Type[0];
            }
            if (!genericType.ContainsGenericParameters)
            {
                genericType = genericType.GetGenericTypeDefinition();
            }
            if (source.IsGenericType && source.ContainsGenericParameters)
            {
                return new Type[0];
            }
            if (genericType.IsInterface || source.IsInterface)
            {
                if (source.IsGenericType && source.GetGenericTypeDefinition() == genericType)
                {
                    return source.GetGenericArguments();
                }
                var interfaces = source.GetInterfaces();
                foreach (var @interface in interfaces)
                {
                    if (@interface.GetGenericTypeDefinition() == genericType)
                    {
                        return @interface.GetGenericArguments();
                    }
                }
            }
            else
            {
                while (source != typeof(object))
                {
                    if (source.IsGenericType)
                    {
                        if (source.GetGenericTypeDefinition() == genericType)
                        {
                            return source.GetGenericArguments();
                        }
                    }
                    source = source.BaseType;
                }
            }
            return new Type[0];
        }

        /// <summary>
        /// 是否某个泛类型的子类
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType"></param>
        /// <returns></returns>
        public static bool IsSubOfGenericClass(this Type type, Type genericType)
        {
            Assert.NotNull(type, nameof(type));
            Assert.NotNull(genericType, nameof(genericType));
            if (genericType.IsInterface)
            {
                var interfaces = type.GetInterfaces().Where(t => t.IsGenericType);
                foreach (var t in interfaces)
                {
                    if (t.GetGenericTypeDefinition() == genericType)
                    {
                        return true;
                    }
                }
            }
            else
            {
                Type baseType = type.BaseType;
                while (baseType != null && baseType != typeof(object))
                {
                    if (baseType.IsGenericType && genericType == baseType.GetGenericTypeDefinition())
                    {
                        return true;
                    }
                    baseType = baseType.BaseType;
                }
            }
            return false;
        }

        /// <summary>
        /// 判断一个对象是否定义了指定的特性类
        /// </summary>
        /// <typeparam name="TAttribute">特性类类型</typeparam>
        /// <param name="instance">对象实体</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        /// <exception cref="System.ArgumentNullException">当instance为空时</exception>
        public static bool IsAttributeDefined<TAttribute>(this object instance, bool inherit = true)
        {
            Assert.NotNull(instance, nameof(instance));
            return IsAttributeDefined(instance.GetType(), typeof(TAttribute), inherit);
        }

        /// <summary>
        /// 判断一个对象是否定义了指定的特性类
        /// </summary>
        /// <param name="instance">对象实体</param>
        /// <param name="attributeType">特性类类型</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        /// <exception cref="System.ArgumentNullException">当instance为空时</exception>
        public static bool IsAttributeDefined(this object instance, Type attributeType, bool inherit = true)
        {
            Assert.NotNull(instance, nameof(instance));
            return IsAttributeDefined(instance.GetType(), attributeType, inherit);
        }

        /// <summary>
        /// 判断一个对象是否定义了指定的特性类
        /// </summary>
        /// <typeparam name="TAttribute">特性类类型</typeparam>
        /// <typeparam name="TInstance">实体类型。当TInstance是object且instance不是null时使用instance的实际类型</typeparam>
        /// <param name="instance">对象实体。当TInstance是object且instance不是null时使用instance的实际类型</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        /// <remarks>当TInstance是object且instance不是null时使用instance的实际类型</remarks>
        public static bool IsAttributeDefined<TAttribute, TInstance>(this TInstance instance, bool inherit = true)
        {
            Type instanceType = typeof(TInstance);
            if (instanceType == typeof(object) && instance != null)
            {
                instanceType = instance.GetType();
            }
            return IsAttributeDefined(instanceType, typeof(TAttribute), inherit);
        }

        /// <summary>
        /// 判断一个对象是否定义了指定的特性类
        /// </summary>
        /// <typeparam name="TInstance">实体类型。当TInstance是object且instance不是null时使用instance的实际类型</typeparam>
        /// <param name="instance">对象实体。当TInstance是object且instance不是null时使用instance的实际类型</param>
        /// <param name="attributeType">特性类类型</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        /// <remarks>当TInstance是object且instance不是null时使用instance的实际类型</remarks>
        public static bool IsAttributeDefined<TInstance>(this TInstance instance, Type attributeType, bool inherit = true)
        {
            Type instanceType = typeof(TInstance);
            if (instanceType == typeof(object) && instance != null)
            {
                instanceType = instance.GetType();
            }
            return IsAttributeDefined(instanceType, attributeType, inherit);
        }

        /// <summary>
        /// 判断一个类型是否定义了指定的特性
        /// </summary>
        /// <typeparam name="TAttribute">特性类类型</typeparam>
        /// <param name="instanceType">实体类型</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        public static bool IsAttributeDefined<TAttribute>(this Type instanceType, bool inherit = true)
        {
            return IsAttributeDefined(instanceType, typeof(TAttribute), inherit);
        }

        /// <summary>
        /// 判断一个类型是否定义了指定的特性
        /// </summary>
        /// <param name="instanceType">实体类型</param>
        /// <param name="attributeType">特性类类型</param>
        /// <param name="inherit">是否获取继承的定义</param>
        /// <returns>结果，有定义为true，否则为false</returns>
        public static bool IsAttributeDefined(this Type instanceType, Type attributeType, bool inherit = true)
        {
            return instanceType.GetCustomAttributes(attributeType, inherit).Length > 0;
        }
    }
}
