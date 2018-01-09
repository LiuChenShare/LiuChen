using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Configuration
{
	/// <summary>
	/// Represents a LifenxiangConfig
	/// </summary>
	public class ChenyuanEngineContextConfig : ChenyuanBaseConfig, IEngineConfig
	{
		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		protected virtual ChenyuanEngineContextConfig CreateInstance()
		{
			return new ChenyuanEngineContextConfig();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="parent"></param>
		/// <param name="configContext"></param>
		/// <param name="section"></param>
		/// <returns></returns>
		protected override object CreateInternal(object parent, object configContext, XmlNode section)
		{
			var config = CreateInstance();
			var dynamicDiscoveryNode = section.SelectSingleNode("DynamicDiscovery");
			if (dynamicDiscoveryNode != null && dynamicDiscoveryNode.Attributes != null)
			{
				var attribute = dynamicDiscoveryNode.Attributes["Enabled"];
				if (attribute != null)
					config.DynamicDiscovery = Convert.ToBoolean(attribute.Value);
			}

			var engineNode = section.SelectSingleNode("Engine");
			if (engineNode != null && engineNode.Attributes != null)
			{
				var attribute = engineNode.Attributes["Type"];
				if (attribute != null)
					config.EngineType = attribute.Value;
			}

			var themeNode = section.SelectSingleNode("Themes");
			if (themeNode != null && themeNode.Attributes != null)
			{
				var attribute = themeNode.Attributes["basePath"];
				if (attribute != null)
					config.ThemeBasePath = attribute.Value;
			}
			if (config.ThemeBasePath.IsEmpty())
			{
				config.ThemeBasePath = "~/themes/";
			}

			return config;
		}

		/// <summary>
		/// In addition to configured assemblies examine and load assemblies in the bin directory.
		/// </summary>
		public bool DynamicDiscovery { get; set; }

		/// <summary>
		/// A custom <see cref="IEngine"/> to manage the application instead of the default.
		/// </summary>
		public string EngineType { get; set; }

		/// <summary>
		/// Specifices where the themes will be stored (~/Themes/)
		/// </summary>
		public string ThemeBasePath { get; set; }
	}
}
