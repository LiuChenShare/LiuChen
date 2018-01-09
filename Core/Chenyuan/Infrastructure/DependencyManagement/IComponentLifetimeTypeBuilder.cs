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
	public interface IComponentLifetimeTypeBuilder
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
		IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> PerLifeStyle<TLimit, TActivatorData, TRegistrationStyle>(
						  IRegistrationBuilder<TLimit, TActivatorData, TRegistrationStyle> builder,
						  ComponentLifeStyle lifeStyle);
	}
}
