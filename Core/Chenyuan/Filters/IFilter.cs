namespace Chenyuan.Filters
{
    /// <summary>
    /// �������ӿ�
    /// </summary>
	public interface IFilter
	{
        /// <summary>
        /// �Ƿ�������ʹ��
        /// </summary>
		bool AllowMultiple
		{
			get;
		}

        /// <summary>
        /// ˳��
        /// </summary>
		int Order
		{
			get;
		}
	}
}
