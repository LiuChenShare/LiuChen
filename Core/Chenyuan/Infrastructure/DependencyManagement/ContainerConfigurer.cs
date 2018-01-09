using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Components;

namespace Chenyuan.Infrastructure.DependencyManagement
{
	/// <summary>
	/// Configures the inversion of control container with services used by Lifenxiang.
	/// </summary>
	public class ContainerConfigurer
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="engine"></param>
		/// <param name="containerManager"></param>
		/// <param name="broker"></param>
		/// <param name="configuration"></param>
		public virtual void Configure(IEngine engine, ContainerManager containerManager, EventBroker broker, IEngineConfig configuration)
		{
			//other dependencies
			containerManager.AddComponentInstance<IEngineConfig>(configuration, "Chenyuan.configuration");
			containerManager.AddComponentInstance<IEngine>(engine, "Chenyuan.engine");
			containerManager.AddComponentInstance<ContainerConfigurer>(this, "Chenyuan.containerConfigurer");

			//type finder
			containerManager.AddComponent<ITypeFinder, WebAppTypeFinder>("Chenyuan.typeFinder");

			//register dependencies provided by other assemblies
			var typeFinder = containerManager.Resolve<ITypeFinder>();
			containerManager.UpdateContainer(x =>
			{
				var drTypes = typeFinder.FindClassesOfType<IDependencyRegistar>();
				var drInstances = new List<IDependencyRegistar>();
				foreach (var drType in drTypes)
					drInstances.Add((IDependencyRegistar)Activator.CreateInstance(drType));
				//sort
				drInstances = drInstances.OrderBy(t => t.Order).ToList();
				foreach (var dependencyRegistrar in drInstances)
					dependencyRegistrar.Register(x, typeFinder);
			});

			//event broker
			containerManager.AddComponentInstance(broker);
		}
	}
}
