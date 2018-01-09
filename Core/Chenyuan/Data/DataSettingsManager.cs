using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;
using Chenyuan.Extensions;

namespace Chenyuan.Data
{
	/// <summary>
	/// 
	/// </summary>
	public partial class DataSettingsManager
	{
		/// <summary>
		/// `
		/// </summary>
		protected const char c_separator = ':';
		/// <summary>
		/// 
		/// </summary>
		protected const string c_filename = "Settings.txt";

		/// <summary>
		/// Maps a virtual path to a physical disk path.
		/// </summary>
		/// <param name="path">The path to map. E.g. "~/bin"</param>
		/// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
		protected virtual string MapPath(string path)
		{
			if (HostingEnvironment.IsHosted)
			{
				//hosted
				return HostingEnvironment.MapPath(path);
			}
			else
			{
				//not hosted. For example, run in unit tests
				string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
				path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
				return Path.Combine(baseDirectory, path);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		protected virtual DataSettings ParseSettings(string text)
		{
			var shellSettings = new DataSettings();
			if (String.IsNullOrEmpty(text))
				return shellSettings;

			//Old way of file reading. This leads to unexpected behavior when a user's FTP program transfers these files as ASCII (\r\n becomes \n).
			//var settings = text.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
			var settings = new List<string>();
			using (var reader = new StringReader(text))
			{
				string str;
				while ((str = reader.ReadLine()) != null)
					settings.Add(str);
			}

			foreach (var setting in settings)
			{
				var separatorIndex = setting.IndexOf(c_separator);
				if (separatorIndex == -1)
				{
					continue;
				}
				string key = setting.Substring(0, separatorIndex).Trim();
				string value = setting.Substring(separatorIndex + 1).Trim();

				switch (key)
				{
					case "DataProvider":
						if (shellSettings.DataProvider.HasValue())
						{
							//数据唯一性控制
							throw new InvalidDataException("config DataProvider has been existed.");
						}
						shellSettings.DataProvider = value;
						break;
					case "DataConnectionString":
						if (shellSettings.DataConnectionString.HasValue())
						{
							//数据唯一性控制
							throw new InvalidDataException("config DataConnectionString has been existed.");
						}
						shellSettings.DataConnectionString = value;
						break;
					default:
						shellSettings.RawDataSettings.Add(key, value);
						break;
				}
			}

			return shellSettings;
		}

		/// <summary>
		/// 获取配置数据的字符串表现形式
		/// </summary>
		/// <param name="settings"></param>
		/// <returns></returns>
		protected virtual string ComposeSettings(DataSettings settings)
		{
			if (settings == null)
				return "";
			List<string> list = new List<string>();
			list.Add("DataProvider: {0}".FormatWith(settings.DataProvider));
			list.Add("DataConnectionString: {0}".FormatWith(settings.DataConnectionString));
			list.AddRange(settings.RawDataSettings.Where(x => !x.Key.Equals("DataProvider", StringComparison.CurrentCultureIgnoreCase) && !x.Key.Equals("DataConnectionString")).Select(x => "{0}: {1}".FormatWith(x.Key, x.Value)));
			return string.Join(Environment.NewLine, list);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public virtual DataSettings LoadSettings()
		{
			//use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
			string filePath = Path.Combine(MapPath("~/App_Data/"), c_filename);
			if (File.Exists(filePath))
			{
				string text = File.ReadAllText(filePath);
				return ParseSettings(text);
			}
			else
				return new DataSettings();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="settings"></param>
		public virtual void SaveSettings(DataSettings settings)
		{
			if (settings == null)
				throw new ArgumentNullException("settings");

			//use webHelper.MapPath instead of HostingEnvironment.MapPath which is not available in unit tests
			string filePath = Path.Combine(MapPath("~/App_Data/"), c_filename);
			if (!File.Exists(filePath))
			{
				using (File.Create(filePath))
				{
					//we use 'using' to close the file after it's created
				}
			}

			var text = ComposeSettings(settings);
			File.WriteAllText(filePath, text);
		}
	}
}
