using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Components;
using Chenyuan.Data;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure.DependencyManagement;
using Chenyuan.Plugins;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// 默认应用引擎
	/// </summary>
	public class DefaultAppEngine : IEngine
	{
		#region Fields

		private ContainerManager _containerManager;
		private ContainerConfigurer _containerConfigurer;
		private EventBroker _eventBroker;

		#endregion

		#region Ctor

		//      /// <summary>
		///// Creates an instance of the content engine using default settings and configuration.
		///// </summary>
		//public DefaultAppEngine(IEngineConfig config)
		//          : this(config, EventBroker.Instance, new ContainerConfigurer())
		//{
		//      }

		/// <summary>
		/// Creates an instance of the content engine using default settings and configuration.
		/// </summary>
		public DefaultAppEngine()
			: this(EventBroker.Instance, new ContainerConfigurer())
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="broker"></param>
		/// <param name="configurer"></param>
		public DefaultAppEngine(EventBroker broker, ContainerConfigurer configurer)
		{
			_containerConfigurer = configurer;
			_eventBroker = broker;
		}

		///// <summary>
		///// 
		///// </summary>
		///// <param name="config"></param>
		///// <param name="broker"></param>
		///// <param name="configurer"></param>
		//public DefaultAppEngine(IEngineConfig config, EventBroker broker, ContainerConfigurer configurer)
		//{
		//}

		#endregion

		#region Utilities

		private void RunStartupTasks()
		{
			var typeFinder = _containerManager.Resolve<ITypeFinder>();
			var startUpTaskTypes = typeFinder.FindClassesOfType<IStartupTask>();
			var startUpTasks = new List<IStartupTask>();

			foreach (var startUpTaskType in startUpTaskTypes)
			{
				if (PluginManager.IsActivePluginAssembly(startUpTaskType.Assembly))
				{
					startUpTasks.Add((IStartupTask)Activator.CreateInstance(startUpTaskType));
				}
			}

			//sort
			foreach (var startUpTask in startUpTasks.OrderBy(st => st.Order))
			{
				startUpTask.Execute();
			}
		}

		private void InitializeContainer(ContainerConfigurer configurer, EventBroker broker, IEngineConfig config)
		{
			var builder = new ContainerBuilder();

			_containerManager = new ContainerManager(builder.Build());
			configurer.Configure(this, _containerManager, broker, config);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Initialize components and plugins in the sm environment.
		/// </summary>
		/// <param name="config">Config</param>
		public void Initialize(IEngineConfig config)
		{
			InitializeContainer(_containerConfigurer, _eventBroker, config);
			bool databaseInstalled = DataSettingsHelper.DatabaseIsInstalled();
			if (databaseInstalled)
			{
				//startup tasks
				RunStartupTasks();
			}
		}

		/// <summary>
		/// 解析服务
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T Resolve<T>(string name = null) where T : class
		{
			if (name.HasValue())
			{
				return ContainerManager.ResolveNamed<T>(name);
			}
			return ContainerManager.Resolve<T>();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public object Resolve(Type type, string name = null)
		{
			if (name.HasValue())
			{
				return ContainerManager.ResolveNamed(name, type);
			}
			return ContainerManager.Resolve(type);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public Array ResolveAll(Type serviceType)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T[] ResolveAll<T>()
		{
			return ContainerManager.ResolveAll<T>();
		}

		#endregion

		#region Properties

		/// <summary>
		/// 
		/// </summary>
		public IContainer Container
		{
			get { return _containerManager.Container; }
		}

		/// <summary>
		/// 
		/// </summary>
		public ContainerManager ContainerManager
		{
			get { return _containerManager; }
		}

		#endregion
	}
}
