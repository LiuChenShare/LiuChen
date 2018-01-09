using System;
using System.Collections.Generic;

namespace Chenyuan.Components
{
    public interface IDependencyResolver
	{
		object GetService(Type serviceType);
		IEnumerable<object> GetServices(Type serviceType);
	}
}
