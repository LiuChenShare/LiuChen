using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chenyuan.Components;

namespace Chenyuan.Infrastructure.DependencyManagement
{
	/// <summary>
	/// 依赖注册基类定义
	/// </summary>
	public abstract class DependencyRegistarBase : IDependencyRegistar
	{
		/// <summary>
		/// 启动注册
		/// </summary>
		/// <param name="builder"></param>
		/// <param name="typeFinder"></param>
		public abstract void Register(ContainerBuilder builder, ITypeFinder typeFinder);

		/// <summary>
		/// 注册服务顺序，值高优先
		/// </summary>
		public virtual int Order
		{
			get
			{
				return 0;
			}
		}
	}
}
