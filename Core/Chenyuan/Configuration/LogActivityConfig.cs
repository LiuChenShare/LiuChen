using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Chenyuan.Configuration
{
	/// <summary>
	/// 用于写控制哪些实体需要写行为log的配置文件类
	/// </summary>
	[XmlRoot("LogActivityConfig")]
	public class LogActivityConfig
	{
		/// <summary>
		/// 
		/// </summary>
		[XmlArray("LogEntitys")]
		[XmlArrayItem("LogEntity")]
		public List<LogEntity> LogEntities { get; set; }
	}

	/// <summary>
	/// 
	/// </summary>
	public class LogEntity
	{
		/// <summary>
		/// 需要写行为log的实体类型名称
		/// </summary>
		[XmlAttribute("name")]
		public string Name { get; set; }
	}
}
