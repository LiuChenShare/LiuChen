using System;
using System.Linq;
using System.Reflection;

namespace Chenyuan.Extensions
{
    /// <summary>
    /// 代理扩展类定义
    /// </summary>
    public static class DelegateExtensions
    {
        readonly static MethodInfo s_makeFuncGenericHandler;
        static DelegateExtensions()
        {
            MethodInfo[] methods = typeof(DelegateExtensions).GetMethods();
            s_makeFuncGenericHandler = methods.Where(x => x.Name == "Convert" && x.IsStatic && x.IsGenericMethod && x.GetGenericArguments().Length == 1 && x.GetParameters().Length == 1 && x.GetParameters()[0].ParameterType == typeof(Func<object>)).Single();
        }

        /// <summary>
        /// 执行行为，忽略异常
        /// </summary>
        /// <param name="action">要执行的行为对象</param>
        public static void Try(this Action action)
        {
            try
            {
                action();
            }
            catch { }
        }

        /// <summary>
        /// 执行方法，忽略异常
        /// </summary>
        /// <typeparam name="T">方法返回的结果类型</typeparam>
        /// <param name="func">要执行的方法</param>
        /// <param name="default">当有异常时的默认值，默认为类型参数 <typeparamref name="T"/> 的默认值</param>
        /// <returns></returns>
        public static T Try<T>(this Func<T> func, T @default = default(T))
        {
            try
            {
                return func();
            }
            catch
            {
                return @default;
            }
        }

        /// <summary>
        /// Func类型转换
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Func<TResult> Convert<TResult>(this Func<object> source) => () => (TResult)source();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <param name="resultType"></param>
        /// <returns></returns>
        public static object Convert(this Func<object> func, Type resultType) => s_makeFuncGenericHandler.MakeGenericMethod(resultType).Invoke(null, new object[] { func });
    }
}
