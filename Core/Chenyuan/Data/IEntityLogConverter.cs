using System.Xml.Linq;

namespace Chenyuan.Data
{
	/// <summary>
	/// 实体日志转换接口
	/// </summary>
	public interface IEntityLogConverter
	{
		/// <summary>
		/// 转换为实体日志XML对象
		/// </summary>
		/// <returns></returns>
		XElement ToEntityLog();

		/// <summary>
		/// 转换为实体日志字符串
		/// </summary>
		/// <returns></returns>
		string ToEntityLogString();
	}
}
