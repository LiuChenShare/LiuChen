using System;
using Chenyuan.Utilities;

namespace Chenyuan.Data.Entity
{
    /// <summary>
    /// 实体上下文键值特性定义
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class EntityContextKeyAttribute : Attribute
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="contextKey">上下文键值</param>
        public EntityContextKeyAttribute(string contextKey)
        {
            Assert.NotNull(contextKey, nameof(contextKey));
            Assert.NotEmpty(contextKey, nameof(contextKey));
            this.ContextKey = contextKey;
        }

        /// <summary>
        /// 获取上下文键值
        /// </summary>
        public string ContextKey
        {
            get;
        }
    }
}
