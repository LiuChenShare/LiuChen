using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Exceptions;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data.EF
{
	/// <summary>
	/// 
	/// </summary>
	public partial class EfDataProviderManager : BaseDataProviderManager
	{
		private Type _providerType;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="settings"></param>
		public EfDataProviderManager(DataSettings settings) : base(settings)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="providerName">服务提供者（数据库类型）名称</param>
		/// <returns></returns>
		public override IDataProvider LoadDataProvider(string providerName = null)
		{
			if (_providerType == null)
			{
				providerName = providerName ?? Settings.DataProvider;
				if (String.IsNullOrWhiteSpace(providerName))
					throw new ChenyuanException("Data Settings doesn't contain a providerName");

				var type = Type.GetType(providerName);
				if (type == null || !type.IsSubclassOf(typeof(IDataProvider)))
				{
					type = LoadTypeFromModule($"Chenyuan.Data.{providerName}.dll");
				}
				if (type == null || (type.IsSubclassOf(typeof(IDataProvider)) && !type.IsAbstract && !type.IsInterface))
				{
					throw new ChenyuanException($@"Load IDataProvider on ""{providerName}"" or ""Chenyuan.Data.{providerName}"" failure.");
				}
				_providerType = type;
			}
			return EngineContext.Current.ContainerManager.ResolveUnregistered(_providerType) as IDataProvider;
		}

		private Type LoadTypeFromModule(string module)
		{
			Assembly assembly = null;
			try
			{
				//assembly = Assembly.Load(module);
				assembly = Assembly.LoadFile($"{AppDomain.CurrentDomain.BaseDirectory}bin\\{module}");
			}
			catch (Exception e)
			{
				throw new ChenyuanException($"Load module {module} failure: {e.Message}");
			}
			var types = assembly.GetTypes();
			foreach (var t in types)
			{
				var interfaceTypes = ((System.Reflection.TypeInfo)t).ImplementedInterfaces;
				if (interfaceTypes.Exists(x => x.FullName == typeof(IDataProvider).FullName) /*t.IsSubclassOf(typeof(IDataProvider)) */
					&& !t.IsAbstract && !t.IsInterface)
				{
					return t;
				}
			}
			return null;
		}
	}
}
