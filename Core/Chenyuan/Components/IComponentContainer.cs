using System;
using System.Collections.Generic;

namespace Chenyuan.Components
{
    /// <summary>
    /// 组件容器接口定义
    /// </summary>
    public interface IComponentContainer : IComponentProvider
    {
        #region 类型注册接口定义

        /// <summary>
        /// 根据组件类型对象注册组件服务
        /// </summary>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="lifeStyle">组件生存周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterType(Type implementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null);

        /// <summary>
        /// 根据组件实现类型对象与组件服务类型对象注册组件服务
        /// </summary>
        /// <param name="serviceType">组件服务类型对象</param>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterType(Type serviceType, Type implementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null);

        /// <summary>
        /// 根据组件服务类型注册组件服务
        /// </summary>
        /// <typeparam name="TService">组件服务类型</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterType<TService>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TService : class;

        /// <summary>
        /// 根据组件实现类型与组件服务类型注册组件服务
        /// </summary>
        /// <typeparam name="TService">组件服务类型</typeparam>
        /// <typeparam name="TImplementer">组件服务类型</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterType<TService, TImplementer>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TService : class
            where TImplementer : class, TService;

        /// <summary>
        /// 组件服务组注册
        /// </summary>
        /// <typeparam name="TImplementer">组件实现类型</typeparam>
        /// <param name="serviceTypes">组件服务组类型对象数组</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterTypes<TImplementer>(Type[] serviceTypes, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TImplementer : class;

        /// <summary>
        /// 组件服务组注册
        /// </summary>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="serviceTypes">组件服务组类型对象数组</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterTypes(Type implementationType, Type[] serviceTypes, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null);

        #endregion

        #region 组件实例注册

        /// <summary>
        /// 根据组件实现类型与组件服务类型使用组件实体对象注册组件服务
        /// </summary>
        /// <remarks>
        /// 该注册强制使用Singleton生命周期
        /// </remarks>
        /// <typeparam name="TService">组件服务类型</typeparam>
        /// <typeparam name="TImplementer">组件实现类型</typeparam>
        /// <param name="instance">组件实体对象</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterInstance<TService, TImplementer>(TImplementer instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService;

        /// <summary>
        /// 注册组件实例对象到服务类型实体
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <param name="instance">组件实例对象，必须与服务类型兼容</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterInstance<TService>(TService instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class;

        /// <summary>
        /// 注册组件实例对象到服务类型实体
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instance">组件实例对象，必须与服务类型兼容</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterInstance(Type serviceType, object instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        #endregion

        #region 组件句柄注册

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <param name="instanceHandler">组件实例获取句柄，返回结果必须兼容目标服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler<TService>(Func<TService> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class;

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <typeparam name="TImplementer">组件实例服务类型</typeparam>
        /// <param name="instanceHandler">实例获取句柄，调用参数Type类型的对象为组件实例服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler<TService, TImplementer>(Func<Type, TImplementer> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService;

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <param name="instanceHandler">组件实例获取句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler<TService>(Func<IComponentContainer, TService> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class;

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <typeparam name="TImplementer">组件实例服务类型</typeparam>
        /// <param name="instanceHandler">实例获取句柄，调用参数Type类型的对象为组件实例服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler<TService, TImplementer>(Func<IComponentContainer, Type, TImplementer> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService;

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instanceHandler">组件实例获取句柄，返回结果必须兼容目标服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler(Type serviceType, Func<object> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instanceHandler">组件实例获取句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterHandler(Type serviceType, Func<IComponentContainer, object> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        #endregion

        #region 组件模型注册

        ///// <summary>
        ///// 泛型模型注册
        ///// </summary>
        ///// <typeparam name="TGenericService">范服务类型</typeparam>
        ///// <typeparam name="TGenericImplementer">范实例类型</typeparam>
        ///// <param name="lifeStyle">组件生命周期</param>
        ///// <param name="key">组件唯一识别key</param>
        //void RegisterGeneric<TGenericService, TGenericImplementer>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        //    where TGenericService : class
        //    where TGenericImplementer : class;

        /// <summary>
        /// 泛型模型注册
        /// </summary>
        /// <param name="genericServiceType">范服务类型对象</param>
        /// <param name="genericImplementationType">范实例类型对象</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterGeneric(Type genericServiceType, Type genericImplementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource(Type sourceType, Func<IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource<TSource>(Func<IComponentContainer, IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource;

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource(Type sourceType, Func<IComponentContainer, IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource<TSource>(Func<IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource;

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource(Type sourceType, Func<IRegistableSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource<TSource>(Func<IComponentContainer, TSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource;

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource(Type sourceType, Func<IComponentContainer, IRegistableSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource<TSource>(Func<TSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource;

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceServiceType">可注册源数据加载服务类型对象，必须派生自 <see cref="IRegistableSourceService"/></param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource(Type sourceType, Type sourceServiceType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null);

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <typeparam name="TSourceService">可注册源数据加载服务类型对象</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        IComponentContainer RegisterSource<TSource, TSourceService>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
            where TSourceService : class, IRegistableSourceService;

        #endregion

        /// <summary>
        /// 判断类型是否已经注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="key">组件唯一识别key</param>
        /// <returns></returns>
        bool IsRegisted<TService>(string key = null)
            where TService : class;

        /// <summary>
        /// 判断类型是否已经注册
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns></returns>
        bool IsRegisted(Type serviceType, string key = null);
    }
}
