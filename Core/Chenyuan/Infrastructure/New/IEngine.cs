using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Infrastructure.DependencyManagement;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// Classes implementing this interface can serve as a portal for the 
	/// various services composing the Lifenxiang engine. Edit functionality, modules
	/// and implementations access most Lifenxiang functionality through this 
	/// interface.
	/// </summary>
	public interface IEngine
	{
		/// <summary>
		/// 
		/// </summary>
		ContainerManager ContainerManager { get; }

		/// <summary>
		/// Initialize components and plugins in the Lifenxiang environment.
		/// </summary>
		/// <param name="config">Config</param>
		void Initialize(IEngineConfig config);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		T Resolve<T>(string name = null) where T : class;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		object Resolve(Type type, string name = null);

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		Array ResolveAll(Type serviceType);

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		T[] ResolveAll<T>();
	}
}
