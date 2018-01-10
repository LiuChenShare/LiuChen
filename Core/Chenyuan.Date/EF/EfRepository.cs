using Autofac;
using Autofac.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Data.Entity;
using Chenyuan.Extensions;
using Chenyuan.Infrastructure;

namespace Chenyuan.Data.EF
{
	/// <summary>
	/// Entity Framework repository
	/// </summary>
	public partial class EfRepository<T> : EfRepositoryBase<T>
		where T : BaseEntity
	{
		private readonly IChenyuanDBContext _context;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="context"></param>
		public EfRepository(IChenyuanDBContext context)
		{
			_context = context;
		}

		/// <summary>
		/// 
		/// </summary>
		protected override IChenyuanDBContext DbContext
		{
			get
			{
				return _context;
			}
		}

	}

	/// <summary>
	/// Entity Framework repository
	/// </summary>
	public partial class MultiEfRepository<T> : EfRepositoryBase<T>
		where T : BaseEntity
	{
		private readonly IChenyuanDBContext _context;

		#region static

		private static EntityKeyedServices s_entityKeyedServices;

		private class EntityKeyedServices : Dictionary<Type, Service>
		{
		}

		
		private static IDictionary<Type, Service> s_EntityKeyedServices
		{
			get
			{
				if (s_entityKeyedServices == null)
				{
					Type t = typeof(EntityKeyedServices);
					lock (t)
					{
						if (s_entityKeyedServices == null)
						{
							if (EngineContext.Current.ContainerManager.IsRegistered(t))
							{
								s_entityKeyedServices = EngineContext.Current.Resolve<EntityKeyedServices>();
							}
							else
							{
								s_entityKeyedServices = new EntityKeyedServices();
								EngineContext.Current.ContainerManager.UpdateContainer(x => x.RegisterInstance<EntityKeyedServices>(s_entityKeyedServices).SingleInstance());
							}
						}
					}
				}
				return s_entityKeyedServices;
			}
		}

		#endregion

		/// <summary>
		/// 
		/// </summary>
		public MultiEfRepository()
		{
			Type t = typeof(T);
			lock (t)
			{
				Service service = null;
				if (s_EntityKeyedServices.ContainsKey(t))
				{
					service = s_EntityKeyedServices[t];
				}
				else
				{
					var contextKeyAttribute = t.GetCustomAttribute<EntityContextKeyAttribute>();
					if (contextKeyAttribute == null || contextKeyAttribute.ContextKey.IsEmpty())
					{
						service = new TypedService(t);
					}
					else
					{
						service = new KeyedService(contextKeyAttribute.ContextKey, t);
					}
					s_EntityKeyedServices.Add(t, service);
				}
				_context = EngineContext.Current.ContainerManager.ResolveService<IChenyuanDBContext>(service);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		protected override IChenyuanDBContext DbContext
		{
			get
			{
				return _context;
			}
		}
	}
}
