using Autofac;
using Chenyuan.Components;

namespace Chenyuan.Infrastructure.DependencyManagement
{
	/// <summary>
	/// 
	/// </summary>
	public interface IDependencyRegistar
	{
		/// <summary>
		/// 
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="typeFinder"></param>
		void Register(ContainerBuilder builder, ITypeFinder typeFinder);

		/// <summary>
		/// 
		/// </summary>
		int Order { get; }
	}
}
