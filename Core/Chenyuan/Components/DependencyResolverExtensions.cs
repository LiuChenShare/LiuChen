using System.Collections.Generic;
using System.Linq;

namespace Chenyuan.Components
{
    public static class DependencyResolverExtensions
	{
		public static TService GetService<TService>(this IDependencyResolver resolver)
		{
			return (TService)((object)resolver.GetService(typeof(TService)));
		}
		public static IEnumerable<TService> GetServices<TService>(this IDependencyResolver resolver)
		{
			return resolver.GetServices(typeof(TService)).Cast<TService>();
		}
	}
}
