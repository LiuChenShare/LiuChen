using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data
{
	/// <summary>
	/// 
	/// </summary>
	public static class DataSettingsHelper
	{
		private static bool? _databaseIsInstalled;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static bool DatabaseIsInstalled()
		{
			if (!_databaseIsInstalled.HasValue)
			{
				var manager = new DataSettingsManager();
				var settings = manager.LoadSettings();
				_databaseIsInstalled = settings != null && !String.IsNullOrEmpty(settings.DataConnectionString);
			}
			return _databaseIsInstalled.Value;
		}

		/// <summary>
		/// 
		/// </summary>
		public static void ResetCache()
		{
			_databaseIsInstalled = null;
		}
	}
}
