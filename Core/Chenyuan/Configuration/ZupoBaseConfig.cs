using System.Configuration;
using System.Xml;

namespace Chenyuan.Configuration
{
	/// <summary>
	/// 配置节点基类定义
	/// </summary>
	public abstract class ChenyuanBaseConfig : IConfigurationSectionHandler
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		public virtual object Create(object parent, object configContext, XmlNode section)
		{
			return this.CreateInternal(parent, configContext, section);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		protected abstract object CreateInternal(object parent, object configContext, XmlNode section);
	}
}
