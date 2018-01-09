using System;

namespace Chenyuan.Components
{
    /// <summary>
    /// 组件提供者接口定义
    /// </summary>
    public interface IComponentProvider
    {

        #region 组件实例解析

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        TService Resolve<TService>(string key = null)
            where TService : class;

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        object Resolve(Type serviceType, string key = null);

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <returns>目标服务组件实体对象</returns>
        TService ResolveUnregist<TService>(/*string key = null*/)
            where TService : class;

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <returns>目标服务组件实体对象</returns>
        object ResolveUnregist(Type serviceType/*, string key = null*/);

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <typeparam name="TService">服务类型</typeparam>
        ///// <param name="key">组件唯一识别key</param>
        ///// <param name="instance">实体对象</param>
        ///// <returns>目标服务组件实体对象</returns>
        //TService ResolveUnregist<TService>(TService instance, string key = null)
        //    where TService : class;

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <param name="serviceType">服务类型对象</param>
        ///// <param name="instance">实体对象</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //object ResolveUnregist(Type serviceType, object instance, string key = null);

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <typeparam name="TService">服务类型</typeparam>
        ///// <param name="instanceHandler">实体创建句柄</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //TService ResolveUnregist<TService>(Func<TService> instanceHandler, string key = null)
        //    where TService : class;

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <param name="serviceType">服务类型对象</param>
        ///// <param name="instanceHandler">实体创建句柄</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //object ResolveUnregist(Type serviceType, Func<object> instanceHandler, string key = null);

        /*
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        TService TryResolve<TService>(TService instance, string key = null)
            where TService : class;

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instanceHandler"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        TService TryResolve<TService>(Func<TService> instanceHandler, string key = null)
             where TService : class;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object TryResolve(Type serviceType, object instance, string key = null);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param
        /// <param name="instanceHandler"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        object TryResolve(Type serviceType, Func<object> instanceHandler, string key = null);
        */

        #endregion
    }
}
