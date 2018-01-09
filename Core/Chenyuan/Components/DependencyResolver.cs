using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;

namespace Chenyuan.Components
{
    public class DependencyResolver
	{
		private sealed class CacheDependencyResolver : IDependencyResolver
		{
			private readonly ConcurrentDictionary<Type, object> _cache = new ConcurrentDictionary<Type, object>();
			private readonly ConcurrentDictionary<Type, IEnumerable<object>> _cacheMultiple = new ConcurrentDictionary<Type, IEnumerable<object>>();
			private readonly Func<Type, object> _getServiceDelegate;
			private readonly Func<Type, IEnumerable<object>> _getServicesDelegate;
			private readonly IDependencyResolver _resolver;
			public CacheDependencyResolver(IDependencyResolver resolver)
			{
				_resolver = resolver;
				_getServiceDelegate = new Func<Type, object>(_resolver.GetService);
				_getServicesDelegate = new Func<Type, IEnumerable<object>>(_resolver.GetServices);
			}
			public object GetService(Type serviceType)
			{
				return _cache.GetOrAdd(serviceType, _getServiceDelegate);
			}
			public IEnumerable<object> GetServices(Type serviceType)
			{
				return _cacheMultiple.GetOrAdd(serviceType, _getServicesDelegate);
			}
		}
		private class DefaultDependencyResolver : IDependencyResolver
		{
			public object GetService(Type serviceType)
			{
				if (serviceType.IsInterface || serviceType.IsAbstract)
				{
					return null;
				}
				object result;
				try
				{
					result = Activator.CreateInstance(serviceType);
				}
				catch
				{
					result = null;
				}
				return result;
			}
			public IEnumerable<object> GetServices(Type serviceType)
			{
				return Enumerable.Empty<object>();
			}
		}
		private class DelegateBasedDependencyResolver : IDependencyResolver
		{
			private Func<Type, object> _getService;
			private Func<Type, IEnumerable<object>> _getServices;
			public DelegateBasedDependencyResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
			{
				_getService = getService;
				_getServices = getServices;
			}
			public object GetService(Type type)
			{
				object result;
				try
				{
					result = _getService(type);
				}
				catch
				{
					result = null;
				}
				return result;
			}
			public IEnumerable<object> GetServices(Type type)
			{
				return _getServices(type);
			}
		}
		private static DependencyResolver _instance = new DependencyResolver();
		private IDependencyResolver _current;
		private CacheDependencyResolver _currentCache;
		public static IDependencyResolver Current
		{
			get
			{
				return _instance.InnerCurrent;
			}
		}
		internal static IDependencyResolver CurrentCache
		{
			get
			{
				return _instance.InnerCurrentCache;
			}
		}
		public IDependencyResolver InnerCurrent
		{
			get
			{
				return _current;
			}
		}
		internal IDependencyResolver InnerCurrentCache
		{
			get
			{
				return _currentCache;
			}
		}
		public DependencyResolver()
		{
			this.InnerSetResolver(new DefaultDependencyResolver());
		}
		public static void SetResolver(IDependencyResolver resolver)
		{
            _instance.InnerSetResolver(resolver);
		}
		public static void SetResolver(object commonServiceLocator)
		{
            _instance.InnerSetResolver(commonServiceLocator);
		}
		public static void SetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
		{
            _instance.InnerSetResolver(getService, getServices);
		}
		public void InnerSetResolver(IDependencyResolver resolver)
		{
			if (resolver == null)
			{
				throw new ArgumentNullException("resolver");
			}
			_current = resolver;
			_currentCache = new CacheDependencyResolver(_current);
		}
		public void InnerSetResolver(object commonServiceLocator)
		{
			if (commonServiceLocator == null)
			{
				throw new ArgumentNullException("commonServiceLocator");
			}
			Type type = commonServiceLocator.GetType();
			MethodInfo method = type.GetMethod("GetInstance", new Type[]
			{
				typeof(Type)
			});
			MethodInfo method2 = type.GetMethod("GetAllInstances", new Type[]
			{
				typeof(Type)
			});
			if (method == null || method.ReturnType != typeof(object) || method2 == null || method2.ReturnType != typeof(IEnumerable<object>))
			{
				throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resource.DependencyResolver_DoesNotImplementICommonServiceLocator, new object[]
				{
					type.FullName
				}), "commonServiceLocator");
			}
			Func<Type, object> getService = (Func<Type, object>)Delegate.CreateDelegate(typeof(Func<Type, object>), commonServiceLocator, method);
			Func<Type, IEnumerable<object>> getServices = (Func<Type, IEnumerable<object>>)Delegate.CreateDelegate(typeof(Func<Type, IEnumerable<object>>), commonServiceLocator, method2);
			this.InnerSetResolver(new DelegateBasedDependencyResolver(getService, getServices));
		}
		public void InnerSetResolver(Func<Type, object> getService, Func<Type, IEnumerable<object>> getServices)
		{
			if (getService == null)
			{
				throw new ArgumentNullException("getService");
			}
			if (getServices == null)
			{
				throw new ArgumentNullException("getServices");
			}
			this.InnerSetResolver(new DelegateBasedDependencyResolver(getService, getServices));
		}
	}
}
