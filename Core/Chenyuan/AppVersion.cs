using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
	/// <summary>
	/// 
	/// </summary>
	public static class AppVersion
	{
		/// <summary>
		/// 
		/// </summary>
		public static IAppVersion DefaultAppVersion
		{
			get
			{
				if (s_DefaultAppVersion == null)
				{
					s_DefaultAppVersion = new DefaultAppVersion();
				}
				return s_DefaultAppVersion;
			}
			set
			{
				s_DefaultAppVersion = value;
			}
		}
		static IAppVersion s_DefaultAppVersion;
	}
}
