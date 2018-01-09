using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// 
	/// </summary>
	public interface IEngineConfig
	{
		/// <summary>
		/// In addition to configured assemblies examine and load assemblies in the bin directory.
		/// </summary>
		bool DynamicDiscovery { get; set; }

		/// <summary>
		/// A custom <see cref="IEngine"/> to manage the application instead of the default.
		/// </summary>
		string EngineType { get; set; }

		/// <summary>
		/// Specifices where the themes will be stored (~/Themes/)
		/// </summary>
		string ThemeBasePath { get; set; }
	}
}
