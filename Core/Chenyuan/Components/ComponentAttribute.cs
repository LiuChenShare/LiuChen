using System;

namespace Chenyuan.Components
{
    /// <summary>
    /// 组件对象类特性定义
    /// </summary>
    /// <remarks>
    /// 该特性仅限于类定义使用，描述一个类的组件生命周期
    /// </remarks>
    [AttributeUsage(AttributeTargets.Class)]
    public class ComponentAttribute : Attribute
    {
        /// <summary>
        /// 默认构造函数，生命周期为 Singleton
        /// </summary>
        public ComponentAttribute()
            : this(ComponentLifeStyle.Singleton)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="lifeStyle">指定组件的生命周期</param>
        public ComponentAttribute(ComponentLifeStyle lifeStyle)
        {
            this.LifeStyle = lifeStyle;
        }

        /// <summary>
        /// 获取当前特性定义的组件生命周期
        /// </summary>
        /// <value>特性定义的组件生命周期</value>
        public ComponentLifeStyle LifeStyle
        {
            get;
        }

        /// <summary>
        /// 服务类型
        /// </summary>
        public Type ServiceType
        {
            get;
            set;
        }

        /// <summary>
        /// 服务命名
        /// </summary>
        public string Name
        {
            get;
            set;
        }
    }
}
