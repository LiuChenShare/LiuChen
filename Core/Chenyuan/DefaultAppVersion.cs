﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Extensions;

namespace Chenyuan
{
	/// <summary>
	/// 
	/// </summary>
	public class DefaultAppVersion : IAppVersion
	{
		private readonly Version _infoVersion = new Version("2.0.0.0");
		private readonly Version _version = Assembly.GetExecutingAssembly().GetName().Version;
		private readonly List<Version> _breakingChangesHistory = new List<Version>()
		{ 
            // IMPORTANT: Add app versions from low to high
            // NOTE: do not specify build & revision unless you have good reasons for it.
            //       A release with breaking changes should definitely have at least
            //       a greater minor version.
            new Version("1.2.1"),
			new Version("2.0.0") // MC: had to be :-(
        };

		/// <summary>
		/// 
		/// </summary>
		public DefaultAppVersion()
		{
			this.Intitialize();

			BreakingChangesHistory.Reverse();

			// get informational version
			var infoVersionAttr = Assembly.GetExecutingAssembly().GetAttribute<AssemblyInformationalVersionAttribute>(false);
			if (infoVersionAttr != null)
			{
				_infoVersion = new Version(infoVersionAttr.InformationalVersion);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected void Intitialize()
		{

		}

		/// <summary>
		/// 
		/// </summary>
		public List<Version> BreakingChangesHistory
		{
			get
			{
				return _breakingChangesHistory;
			}
		}

		/// <summary>
		/// Gets the app version
		/// </summary>
		public string CurrentVersion
		{
			get
			{
				return "{0}.{1}".FormatInvariant(_infoVersion.Major, _infoVersion.Minor);
			}
		}

		/// <summary>
		/// Gets the app full version
		/// </summary>
		public string CurrentFullVersion
		{
			get
			{
				return _infoVersion.ToString();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public Version FullVersion
		{
			get
			{
				//return s_version;
				return _infoVersion; // MC: (???)
			}
		}

		/// <summary>
		/// Gets a list of Lifenxiang.Net versions in which breaking changes occured,
		/// which could lead to plugins malfunctioning.
		/// </summary>
		/// <remarks>
		/// A plugin's <c>MinAppVersion</c> is checked against this list to assume
		/// it's compatibility with the current app version.
		/// </remarks>
		IEnumerable<Version> IAppVersion.BreakingChangesHistory
		{
			get
			{
				return this.BreakingChangesHistory;
			}
		}
	}
}
