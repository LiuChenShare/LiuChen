namespace Chenyuan.Components
{
    /// <summary>
    /// 当前解析器接口定义
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IResolver<T>
	{
        /// <summary>
        /// 当前实例对象
        /// </summary>
		T Current
		{
			get;
		}
	}
}
