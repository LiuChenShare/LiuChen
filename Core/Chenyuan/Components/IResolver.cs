namespace Chenyuan.Components
{
    /// <summary>
    /// ��ǰ�������ӿڶ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public interface IResolver<T>
	{
        /// <summary>
        /// ��ǰʵ������
        /// </summary>
		T Current
		{
			get;
		}
	}
}
