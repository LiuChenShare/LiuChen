using System;
using System.Configuration;
using System.Runtime.CompilerServices;
using Chenyuan.Configuration;
using Chenyuan.Exceptions;
using Chenyuan.Extensions;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// Provides access to the singleton instance of the Lifenxiang engine.
	/// </summary>
	public class EngineContext
	{
		/// <summary>
		/// 
		/// </summary>
		public static string DefaultEngineConfigSectionName;
		private static string s_defaultEngineConfigSectionName = "ChenyuanConfig";

		/// <summary>
		/// 
		/// </summary>
		public static string EngineConfigSectionName
		{
			get
			{
				if (DefaultEngineConfigSectionName.IsNullOrEmpty())
				{
					return s_defaultEngineConfigSectionName;
				}
				return DefaultEngineConfigSectionName;
			}
		}

		#region Initialization Methods

		/// <summary>Initializes a static instance of the Lifenxiang factory.</summary>
		/// <param name="forceRecreate">Creates a new factory instance even though the factory has been previously initialized.</param>
		[MethodImpl(MethodImplOptions.Synchronized)]
		public static IEngine Initialize(bool forceRecreate)
		{
			if (Singleton<IEngine>.Instance == null || forceRecreate)
			{
				var config = EngineContext.LoadConfig();
				if (config == null)
				{
					throw new ChenyuanException("Config " + EngineConfigSectionName + " must be inherit from interface " + typeof(IEngineConfig).FullName);
				}
				Singleton<IEngine>.Instance = CreateEngineInstance(config);
				Singleton<IEngine>.Instance.Initialize(config);
			}
			return Singleton<IEngine>.Instance;
		}

		/// <summary>Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.</summary>
		/// <param name="engine">The engine to use.</param>
		/// <remarks>Only use this method if you know what you're doing.</remarks>
		public static void Replace(IEngine engine)
		{
			Singleton<IEngine>.Instance = engine;
		}

		/// <summary>
		/// Creates a factory instance and adds a http application injecting facility.
		/// </summary>
		/// <returns>A new factory</returns>
		public static IEngine CreateEngineInstance(IEngineConfig config)
		{
			if (config != null && !string.IsNullOrEmpty(config.EngineType))
			{
				var engineType = Type.GetType(config.EngineType);
				if (engineType == null)
					throw new ConfigurationErrorsException("The type '" + engineType + "' could not be found. Please check the configuration at /configuration/LifenxiangConfig/engine[@engineType] or check for missing assemblies.");
				if (!typeof(IEngine).IsAssignableFrom(engineType))
					throw new ConfigurationErrorsException("The type '" + engineType + "' doesn't implement 'Chenyuan.Core.Infrastructure.IEngine' and cannot be configured in /configuration/LifenxiangConfig/engine[@engineType] for that purpose.");
				return Activator.CreateInstance(engineType) as IEngine;
			}

			return new DefaultAppEngine();
		}

		#endregion

		/// <summary>Gets the singleton Lifenxiang engine used to access Lifenxiang services.</summary>
		public static IEngine Current
		{
			get
			{
				if (Singleton<IEngine>.Instance == null)
				{
					Initialize(false);
				}
				return Singleton<IEngine>.Instance;
			}
		}

		/// <summary>
		/// 加载配置信息
		/// </summary>
		/// <returns></returns>
		public static IEngineConfig LoadConfig()
		{
			var result = ConfigurationManager.GetSection(EngineConfigSectionName);
			if (result == null)
			{
				result = new ChenyuanEngineContextConfig();
			}
			return result as IEngineConfig;
		}
	}
}
