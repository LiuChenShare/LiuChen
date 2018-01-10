namespace Chenyuan.Filters
{
    /// <summary>
    /// 过滤器接口
    /// </summary>
	public interface IFilter
	{
        /// <summary>
        /// 是否允许多次使用
        /// </summary>
		bool AllowMultiple
		{
			get;
		}

        /// <summary>
        /// 顺序
        /// </summary>
		int Order
		{
			get;
		}
	}
}
