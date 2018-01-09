using System;
using System.Collections;
using System.Diagnostics;
using System.Linq.Expressions;
using Chenyuan.Extensions;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// 数据判断类
	/// </summary>
	public static class Guard
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
		/// Throws proper exception if the class reference is null.
		/// </summary>
		/// <typeparam name="TValue"></typeparam>
		/// <param name="value">Class reference to check.</param>
		/// <exception cref="InvalidOperationException">If class reference is null.</exception>
		[DebuggerStepThrough]
		public static void NotNull<TValue>(Func<TValue> value)
		{
			if (value() == null)
				throw new InvalidOperationException("'{0}' cannot be null.".FormatInvariant(value));
		}

		/// <summary>
		/// 判断对象是否非空
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull(object arg, string argName)
		{
			if (arg == null)
				throw new ArgumentNullException(argName);
		}

		/// <summary>
		/// 判断对象是否非空
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull(object arg, string argName, string message)
		{
			if (arg == null)
				throw new ArgumentNullException(argName, message);
		}

		/// <summary>
		/// 判断对象是否非空
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNull<T>(Func<T> arg)
		{
			if (arg() == null)
				throw new ArgumentNullException(GetParamName(arg));
		}

		/// <summary>
		/// 判断对象是否与是TBase的派生类
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <param name="type"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void TypeArgumentAssignedTo<TBase>(Type type, string argName)
		{
			if (!typeof(TBase).IsAssignableFrom(type))
			{
				throw new ArgumentOutOfRangeException(argName, "必须与类型 {0} 兼容。".FormatWith(typeof(TBase).FullName));
			}
		}

		/// <summary>
		/// 判断对象是否与是TBase的派生类
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void TypeArgumentAssignedTo<TBase>(Func<Type> arg)
		{
			var type = arg();
			if (type == null)
			{
				throw new ArgumentNullException(GetParamName(arg));
			}
			if (!typeof(TBase).IsAssignableFrom(type))
			{
				throw new ArgumentOutOfRangeException(GetParamName(arg), "必须与类型 {0} 兼容。".FormatWith(typeof(TBase).FullName));
			}
		}

		/// <summary>
		/// 判断参数序列是否为空
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		[DebuggerStepThrough]
		public static void Arguments<T1, T2>(Func<T1> arg1, Func<T2> arg2)
		{
			if (arg1() == null)
				throw new ArgumentNullException(GetParamName(arg1));

			if (arg2() == null)
				throw new ArgumentNullException(GetParamName(arg2));
		}

		/// <summary>
		/// 判断参数序列是否为空
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="T3"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		[DebuggerStepThrough]
		public static void Arguments<T1, T2, T3>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3)
		{
			if (arg1() == null)
				throw new ArgumentNullException(GetParamName(arg1));

			if (arg2() == null)
				throw new ArgumentNullException(GetParamName(arg2));

			if (arg3() == null)
				throw new ArgumentNullException(GetParamName(arg3));
		}

		/// <summary>
		/// 判断参数序列是否为空
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="T3"></typeparam>
		/// <typeparam name="T4"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		[DebuggerStepThrough]
		public static void Arguments<T1, T2, T3, T4>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4)
		{
			if (arg1() == null)
				throw new ArgumentNullException(GetParamName(arg1));

			if (arg2() == null)
				throw new ArgumentNullException(GetParamName(arg2));

			if (arg3() == null)
				throw new ArgumentNullException(GetParamName(arg3));

			if (arg4() == null)
				throw new ArgumentNullException(GetParamName(arg4));
		}

		/// <summary>
		/// 判断参数序列是否为空
		/// </summary>
		/// <typeparam name="T1"></typeparam>
		/// <typeparam name="T2"></typeparam>
		/// <typeparam name="T3"></typeparam>
		/// <typeparam name="T4"></typeparam>
		/// <typeparam name="T5"></typeparam>
		/// <param name="arg1"></param>
		/// <param name="arg2"></param>
		/// <param name="arg3"></param>
		/// <param name="arg4"></param>
		/// <param name="arg5"></param>
		[DebuggerStepThrough]
		public static void Arguments<T1, T2, T3, T4, T5>(Func<T1> arg1, Func<T2> arg2, Func<T3> arg3, Func<T4> arg4, Func<T5> arg5)
		{
			if (arg1() == null)
				throw new ArgumentNullException(GetParamName(arg1));

			if (arg2() == null)
				throw new ArgumentNullException(GetParamName(arg2));

			if (arg3() == null)
				throw new ArgumentNullException(GetParamName(arg3));

			if (arg4() == null)
				throw new ArgumentNullException(GetParamName(arg4));

			if (arg5() == null)
				throw new ArgumentNullException(GetParamName(arg5));
		}

		/// <summary>
		/// 判断表达式结果是否非空
		/// </summary>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Func<string> arg)
		{
			if (arg().IsEmpty())
				throw Error.ArgumentNullOrEmpty(arg);
		}

		/// <summary>
		/// 判断表达式结果是否非空
		/// </summary>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Func<Guid> arg)
		{
			if (arg() == Guid.Empty)
			{
				string argName = GetParamName(arg);
				throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
			}
		}

		/// <summary>
		/// 判断集合是否存在元素
		/// </summary>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Func<IEnumerable> arg)
		{
			if (!arg().HasItems())
			{
				string argName = GetParamName(arg);
				throw Error.Argument(argName, "List cannot be null and must have at least one item.");
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(string arg, string argName)
		{
			if (arg.IsEmpty())
				throw Error.Argument(argName, "String parameter '{0}' cannot be null or all whitespace.", argName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(IEnumerable arg, string argName)
		{
			if (!arg.HasItems())
				throw Error.Argument(argName, "List cannot be null and must have at least one item.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotEmpty(Guid arg, string argName)
		{
			if (arg == Guid.Empty)
				throw Error.Argument(argName, "Argument '{0}' cannot be an empty guid.", argName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="min"></param>
		/// <param name="max"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentInRange<T>(T arg, T min, T max, string argName) where T : struct, IComparable<T>
		{
			if (arg.CompareTo(min) < 0 || arg.CompareTo(max) > 0)
				throw Error.ArgumentOutOfRange(argName, "The argument '{0}' must be between '{1}' and '{2}'.", argName, min, max);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="maxLength"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotOutOfLength(string arg, int maxLength, string argName)
		{
			if (arg.Trim().Length > maxLength)
			{
				throw Error.Argument(argName, "Argument '{0}' cannot be more than {1} characters long.", argName, maxLength);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotNegative<T>(T arg, string argName, string message = NotNegativeMessage) where T : struct, IComparable<T>
		{
			if (arg.CompareTo(default(T)) < 0)
				throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName, arg));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotZero<T>(T arg, string argName) where T : struct, IComparable<T>
		{
			if (arg.CompareTo(default(T)) == 0)
				throw Error.ArgumentOutOfRange(argName, "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotZero<T>(Func<T> arg) where T : struct, IComparable<T>
		{
			if (arg().CompareTo(default(T)) == 0)
			{
				string argName = GetParamName(arg);
				throw Error.ArgumentOutOfRange(argName, "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		[DebuggerStepThrough]
		public static void ArgumentNotZeroAndNull(Func<object> arg)
		{
			var value = arg();
			if (value != null)
			{
				Type type = value.GetType();
				if (type.IsClass)
				{
					return;
				}
				if (type.IsArray)
				{
					return;
				}
				if (type.IsEnum)
				{
					if ((byte)value == 0)
					{
						return;
					}
				}
				switch (Type.GetTypeCode(type))
				{
					case TypeCode.Boolean:
						if ((bool)value)
						{
							return;
						}
						break;
					case TypeCode.Byte:
						if ((byte)value == 0)
						{
							return;
						}
						break;
				}
			}
			string argName = GetParamName(arg);
			throw Error.ArgumentOutOfRange(argName, "Argument '{0}' must be greater or less than zero. Value: '{1}'.", argName, arg);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="assertion"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void Against<TException>(bool assertion, string message = AgainstMessage) where TException : Exception
		{
			if (assertion)
				throw (TException)Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="assertion"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void Against<TException>(Func<bool> assertion, string message = AgainstMessage) where TException : Exception
		{
			//Execute the lambda and if it evaluates to true then throw the exception.
			if (assertion())
				throw (TException)Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <param name="type"></param>
		[DebuggerStepThrough]
		public static void InheritsFrom<TBase>(Type type)
		{
			InheritsFrom<TBase>(type, InheritsFromMessage.FormatInvariant(type.FullName, typeof(TBase).FullName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <param name="type"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void InheritsFrom<TBase>(Type type, string message)
		{
			if (type.BaseType != typeof(TBase))
				throw new InvalidOperationException(message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TInterface"></typeparam>
		/// <param name="type"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void Implements<TInterface>(Type type, string message = ImplementsMessage)
		{
			if (!typeof(TInterface).IsAssignableFrom(type))
				throw new InvalidOperationException(message.FormatInvariant(type.FullName, typeof(TInterface).FullName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TBase"></typeparam>
		/// <param name="type"></param>
		[DebuggerStepThrough]
		public static void IsSubclassOf<TBase>(Type type)
		{
			var baseType = typeof(TBase);
			if (!baseType.IsSubClass(type))
			{
				throw new InvalidOperationException("Type '{0}' must be a subclass of type '{1}'.".FormatInvariant(type.FullName, baseType.FullName));
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <param name="instance"></param>
		[DebuggerStepThrough]
		public static void IsTypeOf<TType>(object instance)
		{
			IsTypeOf<TType>(instance, IsTypeOfMessage.FormatInvariant(instance.GetType().Name, typeof(TType).FullName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TType"></typeparam>
		/// <param name="instance"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void IsTypeOf<TType>(object instance, string message)
		{
			if (!(instance is TType))
				throw new InvalidOperationException(message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TException"></typeparam>
		/// <param name="compare"></param>
		/// <param name="instance"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void IsEqual<TException>(object compare, object instance, string message = IsEqualMessage) where TException : Exception
		{
			if (!compare.Equals(instance))
				throw (TException)Activator.CreateInstance(typeof(TException), message);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		[DebuggerStepThrough]
		public static void HasDefaultConstructor<T>()
		{
			HasDefaultConstructor(typeof(T));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="t"></param>
		[DebuggerStepThrough]
		public static void HasDefaultConstructor(Type t)
		{
			if (!t.HasDefaultConstructor())
				throw Error.InvalidOperation("The type '{0}' must have a default parameterless constructor.", t.FullName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void ArgumentIsPositive<T>(T arg, string argName, string message = IsPositiveMessage) where T : struct, IComparable<T>
		{
			if (arg.CompareTo(default(T)) < 1)
				throw Error.ArgumentOutOfRange(argName, message.FormatInvariant(argName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		/// <param name="message"></param>
		[DebuggerStepThrough]
		public static void ArgumentIsTrue(bool arg, string argName, string message = IsTrueMessage)
		{
			if (!arg)
				throw Error.Argument(argName, message.FormatInvariant(argName));
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentIsEnumType(Type arg, string argName)
		{
			ArgumentNotNull(arg, argName);
			if (!arg.IsEnum)
				throw Error.Argument(argName, "Type '{0}' must be a valid Enum type.", arg.FullName);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="enumType"></param>
		/// <param name="arg"></param>
		/// <param name="argName"></param>
		[DebuggerStepThrough]
		public static void ArgumentIsEnumType(Type enumType, object arg, string argName)
		{
			ArgumentNotNull(arg, argName);
			if (!Enum.IsDefined(enumType, arg))
			{
				throw Error.ArgumentOutOfRange(argName, "The value of the argument '{0}' provided for the enumeration '{1}' is invalid.", argName, enumType.FullName);
			}
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="arg"></param>
		///// <param name="argName"></param>
		//[DebuggerStepThrough]
		//public static void ArgumentNotDisposed(DisposableObject arg, string argName)
		//{
		//	ArgumentNotNull(arg, argName);
		//	if (arg.IsDisposed)
		//		throw Error.ObjectDisposed(argName);
		//}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="indexArg"></param>
		/// <param name="sizeArg"></param>
		/// <param name="indexArgName"></param>
		/// <param name="sizeArgName"></param>
		[DebuggerStepThrough]
		public static void PagingArgsValid(int indexArg, int sizeArg, string indexArgName, string sizeArgName)
		{
			ArgumentNotNegative<int>(indexArg, indexArgName, "PageIndex cannot be below 0");
			if (indexArg > 0)
			{
				// if pageIndex is specified (> 0), PageSize CANNOT be 0 
				ArgumentIsPositive<int>(sizeArg, sizeArgName, "PageSize cannot be below 1 if a PageIndex greater 0 was provided.");
			}
			else
			{
				// pageIndex 0 actually means: take all!
				ArgumentNotNegative(sizeArg, sizeArgName);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		private static string GetParamName<T>(Expression<Func<T>> expression)
		{
			string name = string.Empty;
			MemberExpression body = expression.Body as MemberExpression;

			if (body != null)
			{
				name = body.Member.Name;
			}

			return name;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="expression"></param>
		/// <returns></returns>
		[DebuggerStepThrough]
		private static string GetParamName<T>(Func<T> expression)
		{
			return expression.Method.Name;
		}

	}
}
