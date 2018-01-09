namespace Chenyuan.Components
{
    /// <summary>
    /// Components组件生命周期定义
    /// </summary>
    public enum ComponentLifeStyle
    {
        /// <summary>
        /// 临时组件
        /// </summary>
        /// <remarks>
        /// 每次请求都临时生成组件对象实体
        /// </remarks>
        Transident,

        /// <summary>
        /// 每个请求环境生成唯一的组件对象实体
        /// </summary>
        /// <remarks>
        /// 在每个请求运行环境内，保持实体组件对象的一致性。仅对可以识别请求环境的情况下有效，否则使用默认的Transident
        /// </remarks>
        PerRequest,

        /// <summary>
        /// 基于运行域的单件模式
        /// </summary>
        PerDomain,

        /// <summary>
        /// 单件模式
        /// </summary>
        /// <remarks>
        /// 全局保留组件对象实体
        /// </remarks>
        Singleton,
    }
}
