namespace Chenyuan.Components
{
    /// <summary>
    /// 可唯一ID的对象接口定义
    /// </summary>
	public interface IUniquelyIdentifiable
	{
        /// <summary>
        /// 唯一对象ID
        /// </summary>
		string UniqueId
		{
			get;
		}
	}
}
