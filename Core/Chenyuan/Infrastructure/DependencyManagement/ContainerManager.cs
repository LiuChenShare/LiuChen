using Autofac;
using Autofac.Builder;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Components;
using Chenyuan.Exceptions;

namespace Chenyuan.Infrastructure.DependencyManagement
{
	/// <summary>
	/// 
	/// </summary>
	public class ContainerManager
	{
		private readonly Autofac.IContainer _container;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="container"></param>
		public ContainerManager(Autofac.IContainer container)
		{
			_container = container;
		}

		/// <summary>
		/// 
		/// </summary>
		public Autofac.IContainer Container
		{
			get { return _container; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent<TService>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponent<TService, TService>(key, lifeStyle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent(Type service, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponent(service, service, key, lifeStyle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <typeparam name="TImplementation"></typeparam>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent<TService, TImplementation>(string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponent(typeof(TService), typeof(TImplementation), key, lifeStyle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="implementation"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponent(Type service, Type implementation, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			UpdateContainer(x =>
			{
				var serviceTypes = new List<Type> { service };

				if (service.IsGenericType)
				{
					var temp = x.RegisterGeneric(implementation).As(
						serviceTypes.ToArray()).PerLifeStyle(lifeStyle);
					if (!string.IsNullOrEmpty(key))
					{
						temp.Keyed(key, service);
					}
				}
				else
				{
					var temp = x.RegisterType(implementation).As(
						serviceTypes.ToArray()).PerLifeStyle(lifeStyle);
					if (!string.IsNullOrEmpty(key))
					{
						temp.Keyed(key, service);
					}
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance<TService>(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponentInstance(typeof(TService), instance, key, lifeStyle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance(Type service, object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			UpdateContainer(x =>
			{
				var registration = x.RegisterInstance(instance).Keyed(key, service).As(service).PerLifeStyle(lifeStyle);
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="instance"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentInstance(object instance, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponentInstance(instance.GetType(), instance, key, lifeStyle);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <typeparam name="TImplementation"></typeparam>
		/// <param name="properties"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentWithParameters<TService, TImplementation>(IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			AddComponentWithParameters(typeof(TService), typeof(TImplementation), properties);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <param name="implementation"></param>
		/// <param name="properties"></param>
		/// <param name="key"></param>
		/// <param name="lifeStyle"></param>
		public void AddComponentWithParameters(Type service, Type implementation, IDictionary<string, string> properties, string key = "", ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton)
		{
			UpdateContainer(x =>
			{
				var serviceTypes = new List<Type> { service };

				var temp = x.RegisterType(implementation).As(serviceTypes.ToArray()).
					WithParameters(properties.Select(y => new NamedParameter(y.Key, y.Value)));
				if (!string.IsNullOrEmpty(key))
				{
					temp.Keyed(key, service);
				}
			});
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T Resolve<T>(string key = "") where T : class
		{
			if (string.IsNullOrEmpty(key))
			{
				return Scope().Resolve<T>();
			}
			return Scope().ResolveKeyed<T>(key);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name"></param>
		/// <returns></returns>
		public T ResolveNamed<T>(string name) where T : class
		{
			return Scope().ResolveNamed<T>(name);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object Resolve(Type type)
		{
			return Scope().Resolve(type);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="type"></param>
		/// <returns></returns>
		public object ResolveNamed(string name, Type type)
		{
			return Scope().ResolveNamed(name, type);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="service"></param>
		/// <returns></returns>
		public object ResolveService(Service service)
		{
			return Scope().ResolveService(service);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="service"></param>
		/// <returns></returns>
		public T ResolveService<T>(Service service)
		{
			return (T)Scope().ResolveService(service);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public T[] ResolveAll<T>(string key = "")
		{
			if (string.IsNullOrEmpty(key))
			{
				return Scope().Resolve<IEnumerable<T>>().ToArray();
			}
			return Scope().ResolveKeyed<IEnumerable<T>>(key).ToArray();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <returns></returns>
		public T ResolveUnregistered<T>() where T : class
		{
			return ResolveUnregistered(typeof(T)) as T;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="type"></param>
		/// <returns></returns>
		public object ResolveUnregistered(Type type)
		{
			var constructors = type.GetConstructors();
			foreach (var constructor in constructors)
			{
				try
				{
					var parameters = constructor.GetParameters();
					var parameterInstances = new List<object>();
					foreach (var parameter in parameters)
					{
						var service = Resolve(parameter.ParameterType);
						if (service == null)
							throw new ChenyuanException("Unkown dependency");
						parameterInstances.Add(service);
					}
					return Activator.CreateInstance(type, parameterInstances.ToArray());
				}
				catch (ChenyuanException)
				{

				}
			}
			throw new ChenyuanException("No contructor was found that had all the dependencies satisfied.");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="instance"></param>
		/// <returns></returns>
		public bool TryResolve(Type serviceType, out object instance)
		{
			return Scope().TryResolve(serviceType, out instance);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TService"></typeparam>
		/// <param name="instance"></param>
		/// <returns></returns>
		public bool TryResolve<TService>(out TService instance)
		{
			object result;
			if (this.TryResolve(typeof(TService), out result))
			{
				instance = (TService)result;
				return true;
			}
			instance = default(TService);
			return false;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public bool IsRegistered(Type serviceType)
		{
			return Scope().IsRegistered(serviceType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public object ResolveOptional(Type serviceType)
		{
			return Scope().ResolveOptional(serviceType);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <param name="name"></param>
		/// <returns></returns>
		public object ResolveOptionalWithName(Type serviceType, string name)
		{
			object result;
			Scope().TryResolveNamed(name, serviceType, out result);
			return result;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="action"></param>
		public void UpdateContainer(Action<ContainerBuilder> action)
		{
			var builder = new ContainerBuilder();
			action.Invoke(builder);
			builder.Update(_container);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public ILifetimeScope Scope()
		{
			try
			{
				return AutofacRequestLifetimeHttpModule.GetLifetimeScope(Container, null);
			}
			catch
			{
				return Container;
			}
		}

	}

	/// <summary>
	/// 
	/// </summary>
	public static class ContainerManagerExtensions
	{
		/// <summary>
		/// 
		/// </summary>
		public static IComponentLifetimeTypeBuilder DefaultComponentLifetimeTypeBuilder;
		private static readonly IComponentLifetimeTypeBuilder s_defaultComponentLifetimeTypeBuilder;
		private static bool s_lifetimeTypeBuilderLoaded = false;
		static ContainerManagerExtensions()
		{
			s_defaultComponentLifetimeTypeBuilder = new DefaultComponentLifetimeTypeBuilder();
		}

		private static IComponentLifetimeTypeBuilder LifetimeBuilder
		{
			get
			{
				if (!s_lifetimeTypeBuilderLoaded && DefaultComponentLifetimeTypeBuilder == null)
				{
					LoadLifetimeTypeBuilder();
				}
				return DefaultComponentLifetimeTypeBuilder ?? s_defaultComponentLifetimeTypeBuilder;
			}
		}

		private static void LoadLifetimeTypeBuilder()
		{
			lock (s_defaultComponentLifetimeTypeBuilder)
			{
				if (s_lifetimeTypeBuilderLoaded)
				{
					return;
				}
				var config = EngineContext.LoadConfig();
				ITypeFinder typeFinder = new WebAppTypeFinder(config);// EngineContext.Current.Resolve<ITypeFinder>();
				if (typeFinder != null)
				{
					var types = typeFinder.FindClassesOfType<DefaultComponentLifetimeTypeBuilder>(null,true).Where(t => t != typeof(DefaultComponentLifetimeTypeBuilder));
					foreach (Type type in types)
					{
						try
						{
							DefaultComponentLifetimeTypeBuilder = Activator.CreateInstance(type) as IComponentLifetimeTypeBuilder;
							return;
						}
						catch { };
					}
					types = typeFinder.FindClassesOfType<IComponentLifetimeTypeBuilder>(null,true).Where(t => t != typeof(DefaultComponentLifetimeTypeBuilder));
					foreach (Type type in types)
					{
						try
						{
							DefaultComponentLifetimeTypeBuilder = Activator.CreateInstance(type) as IComponentLifetimeTypeBuilder;
							return;
						}
						catch { };
					}
				}
				s_lifetimeTypeBuilderLoaded = true;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TLimit"></typeparam>
		/// <typeparam name="TActivatorData"></typeparam>
		/// <typeparam name="TRegistrationStyle"></typeparam>
		/// <param name="builder"></param>
		/// <param name="lifeStyle"></param>
		/// <returns></returns>
		public static IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(
							this IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
							ComponentLifeStyle lifeStyle)
		{
			return LifetimeBuilder.PerLifeStyle(builder, lifeStyle);
		}
	}
}
