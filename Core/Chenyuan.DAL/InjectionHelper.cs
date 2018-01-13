using Autofac;
using Chenyuan.Autofac;

namespace Chenyuan.DAL
{
    public class InjectionHelper : IInjection
    {
        /// <summary>
        /// 容器构建对象
        /// </summary>
        public ContainerBuilder Builder
        {
            get; set;
        }

        /// <summary>
        /// service injection
        /// </summary>
        public void ServiceInjection()
        {
            Builder.RegisterGeneric(typeof(EntityRepository<>)).As(typeof(IRepository<>)).InstancePerDependency();
        }
    }
}
