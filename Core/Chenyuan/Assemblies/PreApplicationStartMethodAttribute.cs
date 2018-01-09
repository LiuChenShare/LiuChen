using System;
using System.Linq;
using System.Reflection;

namespace Chenyuan.Assemblies
{
    /// <summary>
    /// 应用预启动方法属性
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true, Inherited = false)]
    public sealed class PreApplicationStartMethodAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">执行方法寄宿的类型</param>
        /// <param name="methodName">方法名，必须是静态的、无参的</param>
        /// <param name="beforeEngineContext">是否在引擎初始化前执行</param>
        public PreApplicationStartMethodAttribute(Type type, string methodName, bool beforeEngineContext)
        {
            var method = type.GetMethod(methodName, BindingFlags.IgnoreCase | BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            if (method == null)
            {
                throw new ArgumentOutOfRangeException("methodName", "未能有效查找到指定方法。");
            }
            if (method.IsGenericMethod)
            {
                throw new ArgumentOutOfRangeException("methodName", "方法不能是泛型。");
            }
            if (method.GetParameters().Any())
            {
                throw new ArgumentOutOfRangeException("methodName", "必须是无参方法。");
            }
            this.Type = type;
            this.MethodName = method.Name;
            this.Method = method;
            this.BeforeEngineContext = beforeEngineContext;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="type">执行方法寄宿的类型</param>
        /// <param name="beforeEngineContext">是否在引擎初始化前执行</param>
        /// <remarks>
        /// 该方法使用默认的方法名 Initialize
        /// </remarks>
        public PreApplicationStartMethodAttribute(Type type, bool beforeEngineContext)
            : this(type, "Initialize", beforeEngineContext)
        {

        }

        /// <summary>
        /// 预处理方法寄宿的类型
        /// </summary>
        public Type Type
        {
            get;
        }

        /// <summary>
        /// 预处理方法名，必须是静态的，无参的
        /// </summary>
        public string MethodName
        {
            get;
        }

        /// <summary>
        /// 预处理方法对象
        /// </summary>
        public MethodInfo Method
        {
            get;
        }

        /// <summary>
        /// 是否引擎初始化前执行
        /// </summary>
        public bool BeforeEngineContext
        {
            get;
        }
    }
}
