using Autofac.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Components;

namespace Chenyuan.Infrastructure.DependencyManagement
{
	/// <summary>
	/// 
	/// </summary>
	public class DefaultComponentLifetimeTypeBuilder : IComponentLifetimeTypeBuilder
	{
		/// <summary>
		/// 
		/// </summary>
		/// <typeparam name="TLimit"></typeparam>
		/// <typeparam name="TActivatorData"></typeparam>
		/// <typeparam name="TRegistrationStyle"></typeparam>
		/// <param name="builder"></param>
		/// <param name="lifeStyle"></param>
		/// <returns></returns>
		public virtual IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(
						   IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
						   ComponentLifeStyle lifeStyle)
		{
			switch (lifeStyle)
			{
				//case ComponentLifeStyle.LifetimeScope:
				//    return HttpContext.Current != null ? builder.InstancePerRequest() : builder.InstancePerLifetimeScope();
				case ComponentLifeStyle.Transident:
					return builder.InstancePerDependency();
				case ComponentLifeStyle.Singleton:
					return builder.SingleInstance();
				default:
					return builder.SingleInstance();
			}
		}
	}
}
