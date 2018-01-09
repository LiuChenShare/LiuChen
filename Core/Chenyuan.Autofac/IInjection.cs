using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Autofac
{
    public interface IInjection
    {
        /// <summary>
        /// 容器构建对象
        /// </summary>
        ContainerBuilder Builder
        {
            get; set;
        }
        /// <summary>
        /// service injection
        /// </summary>
        void ServiceInjection();
    }
}
