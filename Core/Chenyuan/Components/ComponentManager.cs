using System;
using System.Collections.Generic;
using Chenyuan.Extensions;
using Chenyuan.Utilities;
using Chenyuan.Extensions;

namespace Chenyuan.Components
{
    /// <summary>
    /// 组件容器接口代理类
    /// </summary>
    public static class ComponentManager
    {
        private static IComponentContainer s_componentContainer;
        //private static IComponentProvider s_componentProvider;

        private static IComponentContainer s_currentComponentContainer;
        private static Func<IComponentContainer> s_componentContainerHandler;
        //private static IComponentProvider s_currentComponentProvider;
        //private static Func<IComponentProvider> s_componentProviderHandler;
        /// <summary>
        /// 当前组件容器接口实例
        /// </summary>
        public static IComponentContainer Container
        {
            internal get
            {
                if (s_currentComponentContainer == null)
                {
                    s_currentComponentContainer = ContainerInternal;
                }
                //if (s_currentComponentContainer == null)
                //{
                //    s_currentComponentContainer = ProviderInternal as IComponentContainer;
                //}
                return s_currentComponentContainer;
            }
            set
            {
                s_componentContainer = value;
                s_currentComponentContainer = null;
            }
        }

        /// <summary>
        /// 当前组件容器接口实例
        /// </summary>
        private static IComponentContainer ContainerInternal
        {
            get
            {
                if (ContainerHandler != null)
                {
                    return ContainerHandler();
                }
                if (s_componentContainer != null)
                {
                    return s_componentContainer;
                }
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public static IComponentProvider Provider
        {
            get
            {
                return Container;
                //if(s_currentComponentProvider == null)
                //{
                //    if(s_componentProvider != null)
                //    {
                //        s_currentComponentProvider = s_componentProvider;
                //    }
                //    else
                //    {
                //        s_currentComponentProvider = Container;// ProviderInternal;
                //    }
                //}
                //if(s_currentComponentProvider == null)
                //{
                //    s_currentComponentProvider = ContainerInternal as IComponentProvider;
                //}
                //return s_currentComponentProvider;
            }
            //private set
            //{
            //    s_componentProvider = value;
            //    s_currentComponentProvider = null;
            //}
        }

        ///// <summary>
        ///// 
        ///// </summary>
        //private static IComponentProvider ProviderInternal
        //{
        //    get
        //    {
        //        if (ProviderHandler != null)
        //        {
        //            return ProviderHandler();
        //        }
        //        if (s_componentProvider != null)
        //        {
        //            return s_componentProvider;
        //        }
        //        return null;
        //    }
        //}

        /// <summary>
        /// 当前组件容器对象生成委托
        /// </summary>
        public static Func<IComponentContainer> ContainerHandler
        {
            get
            {
                return s_componentContainerHandler;
            }
            set
            {
                s_componentContainerHandler = value;
                s_currentComponentContainer = null;
            }
        }

        ///// <summary>
        ///// 当前组件容器对象生成委托
        ///// </summary>
        //public static Func<IComponentProvider> ProviderHandler
        //{
        //    get
        //    {
        //        return s_componentProviderHandler;
        //    }
        //    set
        //    {
        //        s_componentProviderHandler = value;
        //        s_currentComponentProvider = null;
        //    }
        //}

        #region 组件类型注册

        /// <summary>
        /// 根据组件类型对象注册组件服务
        /// </summary>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="lifeStyle">组件生存周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterType(Type implementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
        {
            Assert.NotNull(implementationType, nameof(implementationType));
            var current = Container;
            current.RegisterType(implementationType, lifeStyle, key, keyedParameters);
            return current;
        }

        /// <summary>
        /// 根据组件实现类型对象与组件服务类型对象注册组件服务
        /// </summary>
        /// <param name="serviceType">组件服务类型对象</param>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterType(Type serviceType, Type implementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            Assert.NotNull(implementationType, nameof(implementationType));
            var current = Container;
            current.RegisterType(serviceType, implementationType, lifeStyle, key, keyedParameters);
            return current;
        }

        /// <summary>
        /// 根据组件服务类型注册组件服务
        /// </summary>
        /// <typeparam name="TService">组件服务类型</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterType<TService>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TService : class
        {
            var current = Container;
            current.RegisterType<TService>(lifeStyle, key, keyedParameters);
            return current;
        }

        /// <summary>
        /// 根据组件实现类型与组件服务类型注册组件服务
        /// </summary>
        /// <typeparam name="TService">组件服务类型</typeparam>
        /// <typeparam name="TImplementer">组件服务类型</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterType<TService, TImplementer>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TService : class
            where TImplementer : class, TService
        {
            var current = Container;
            current.RegisterType<TService, TImplementer>(lifeStyle, key, keyedParameters);
            return current;
        }

        /// <summary>
        /// 组件服务组注册
        /// </summary>
        /// <typeparam name="TImplementer">组件实现类型</typeparam>
        /// <param name="serviceTypes">组件服务组类型对象数组</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterTypes<TImplementer>(Type[] serviceTypes, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
            where TImplementer : class
        {
            Assert.NotEmpty(serviceTypes, nameof(serviceTypes));
            var current = Container;
            current.RegisterTypes<TImplementer>(serviceTypes, lifeStyle, key, keyedParameters);
            return current;
        }

        /// <summary>
        /// 组件服务组注册
        /// </summary>
        /// <param name="implementationType">组件实现类型对象</param>
        /// <param name="serviceTypes">组件服务组类型对象数组</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <param name="keyedParameters">参数键字典</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterTypes(Type implementationType, Type[] serviceTypes, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null, IDictionary<Type, string> keyedParameters = null)
        {
            Assert.NotNull(implementationType, nameof(implementationType));
            Assert.NotEmpty(serviceTypes, nameof(serviceTypes));
            var current = Container;
            current.RegisterTypes(implementationType, serviceTypes, lifeStyle, key, keyedParameters);
            return current;
        }

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
        public static IComponentContainer RegisterInstance<TService, TImplementer>(TImplementer instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService
        {
            Assert.NotNull(instance, nameof(instance));
            var current = Container;
            current.RegisterInstance<TService, TImplementer>(instance, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件实例对象到服务类型实体
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <param name="instance">组件实例对象，必须与服务类型兼容</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterInstance<TService>(TService instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
        {
            Assert.NotNull(instance, nameof(instance));
            var current = Container;
            current.RegisterInstance<TService>(instance, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件实例对象到服务类型实体
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instance">组件实例对象，必须与服务类型兼容</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterInstance(Type serviceType, object instance, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            Assert.NotNull(instance, nameof(instance));
            var current = Container;
            current.RegisterInstance(serviceType, instance, lifeStyle, key);
            return current;
        }

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
        public static IComponentContainer RegisterHandler<TService>(Func<TService> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
        {
            Assert.NotNull(instanceHandler, nameof(instanceHandler));
            var current = Container;
            current.RegisterHandler(instanceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <typeparam name="TImplementer">组件实例服务类型</typeparam>
        /// <param name="instanceHandler">实例获取句柄，调用参数Type类型的对象为组件实例服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterHandler<TService, TImplementer>(Func<Type, TImplementer> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService
        {
            Assert.NotNull(instanceHandler, nameof(instanceHandler));
            var current = Container;
            current.RegisterHandler<TService, TImplementer>(instanceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <param name="instanceHandler">组件实例获取句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterHandler<TService>(Func<IComponentContainer, TService> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
        {
            Assert.NotNull(instanceHandler, nameof(instanceHandler));
            var current = Container;
            current.RegisterHandler<TService>(instanceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <typeparam name="TService">目标服务类型</typeparam>
        /// <typeparam name="TImplementer">组件实例服务类型</typeparam>
        /// <param name="instanceHandler">实例获取句柄，调用参数Type类型的对象为组件实例服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterHandler<TService, TImplementer>(Func<IComponentContainer, Type, TImplementer> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TService : class
            where TImplementer : class, TService
        {
            Assert.NotNull(instanceHandler, nameof(instanceHandler));
            var current = Container;
            current.RegisterHandler<TService, TImplementer>(instanceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instanceHandler">组件实例获取句柄，返回结果必须兼容目标服务类型</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterHandler(Type serviceType, Func<object> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            Assert.NotNull(instanceHandler, nameof(serviceType));
            var current = Container;
            current.RegisterHandler(serviceType, instanceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册组件获取句柄到服务类型
        /// </summary>
        /// <param name="serviceType">目标服务类型对象</param>
        /// <param name="instanceHandler">组件实例获取句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterHandler(Type serviceType, Func<IComponentContainer, object> instanceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            Assert.NotNull(instanceHandler, nameof(instanceHandler));
            var current = Container;
            current.RegisterHandler(serviceType, instanceHandler, lifeStyle, key);
            return current;
        }

        #endregion

        #region 组件模型注册

        ///// <summary>
        ///// 泛型模型注册
        ///// </summary>
        ///// <typeparam name="TGenericService">范服务类型</typeparam>
        ///// <typeparam name="TGenericImplementer">范实例类型</typeparam>
        ///// <param name="lifeStyle">组件生命周期</param>
        ///// <param name="key">组件唯一识别key</param>
        //public static void RegisterGeneric<TGenericService, TGenericImplementer>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        //    where TGenericService : class
        //    where TGenericImplementer : class
        //{
        //    Assert.IsGenericCompatiable<TGenericService, TGenericImplementer>();
        //    Container.RegisterGeneric<TGenericService, TGenericImplementer>(lifeStyle, key);
        //}

        /// <summary>
        /// 泛型模型注册
        /// </summary>
        /// <param name="genericServiceType">范服务类型对象</param>
        /// <param name="genericImplementationType">范实例类型对象</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterGeneric(Type genericServiceType, Type genericImplementationType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(genericServiceType, nameof(genericServiceType));
            Assert.NotNull(genericImplementationType, nameof(genericImplementationType));
            Assert.IsGenericCompatiable(genericServiceType, genericImplementationType);
            var current = Container;
            current.RegisterGeneric(genericServiceType, genericImplementationType, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceServiceType">可注册源数据加载服务类型对象，必须派生自 <see cref="IRegistableSourceService"/></param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource(Type sourceType, Type sourceServiceType, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.IsCompatiable(sourceType, typeof(IRegistableSource));
            Assert.IsCompatiable(sourceServiceType, typeof(IRegistableSourceService));
            var current = Container;
            current.RegisterSource(sourceType, sourceServiceType, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <typeparam name="TSourceService">可注册源数据加载服务类型对象</typeparam>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource<TSource, TSourceService>(ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
            where TSourceService : class, IRegistableSourceService
        {
            var current = Container;
            current.RegisterSource<TSource, TSourceService>(lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource(Type sourceType, Func<IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(sourceType, nameof(sourceType));
            Assert.IsCompatiable(typeof(IRegistableSource), sourceType);
            Assert.NotNull(serviceHandler, nameof(serviceHandler));
            var current = Container;
            current.RegisterSource(sourceType, serviceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource<TSource>(Func<IComponentContainer, IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
        {
            Assert.NotNull(serviceHandler, nameof(serviceHandler));
            var current = Container;
            current.RegisterSource<TSource>(serviceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource(Type sourceType, Func<IComponentContainer, IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(sourceType, nameof(sourceType));
            Assert.IsCompatiable(typeof(IRegistableSource), sourceType);
            Assert.NotNull(serviceHandler, nameof(serviceHandler));
            var current = Container;
            current.RegisterSource(sourceType, serviceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="serviceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource<TSource>(Func<IRegistableSourceService> serviceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
        {
            Assert.NotNull(serviceHandler, nameof(serviceHandler));
            var current = Container;
            current.RegisterSource<TSource>(serviceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource(Type sourceType, Func<IRegistableSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(sourceType, nameof(sourceType));
            Assert.IsCompatiable(typeof(IRegistableSource), sourceType);
            Assert.NotNull(sourceHandler, nameof(sourceHandler));
            var current = Container;
            current.RegisterSource(sourceType, sourceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource<TSource>(Func<IComponentContainer, TSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
        {
            Assert.NotNull(sourceHandler, nameof(sourceHandler));
            var current = Container;
            current.RegisterSource(sourceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <param name="sourceType">可注册源类型对象，必须派生自 <see cref="IRegistableSource"/></param>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource(Type sourceType, Func<IComponentContainer, IRegistableSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
        {
            Assert.NotNull(sourceType, nameof(sourceType));
            Assert.IsCompatiable(typeof(IRegistableSource), sourceType);
            Assert.NotNull(sourceHandler, nameof(sourceHandler));
            var current = Container;
            current.RegisterSource(sourceType, sourceHandler, lifeStyle, key);
            return current;
        }

        /// <summary>
        /// 注册可注册源服务
        /// </summary>
        /// <typeparam name="TSource">可注册源类型对象</typeparam>
        /// <param name="sourceHandler">可注册源数据加载服务句柄</param>
        /// <param name="lifeStyle">组件生命周期</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>返回当前容器对象</returns>
        public static IComponentContainer RegisterSource<TSource>(Func<TSource> sourceHandler, ComponentLifeStyle lifeStyle = ComponentLifeStyle.Singleton, string key = null)
            where TSource : class, IRegistableSource
        {
            Assert.NotNull(sourceHandler, nameof(sourceHandler));
            var current = Container;
            current.RegisterSource(sourceHandler, lifeStyle, key);
            return current;
        }

        #endregion

        /// <summary>
        /// 判断类型是否已经注册
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="key">组件唯一识别key</param>
        /// <returns></returns>
        public static bool IsRegisted<TService>(string key = null)
            where TService : class
        {
            return Container.IsRegisted<TService>(key);
        }

        /// <summary>
        /// 判断类型是否已经注册
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns></returns>
        public static bool IsRegisted(Type serviceType, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            return Container.IsRegisted(serviceType, key);
        }

        #region 组件实例解析

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        public static TService Resolve<TService>(string key = null)
            where TService : class
        {
            return Provider.Resolve<TService>(key);
        }

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        public static object Resolve(Type serviceType, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            return Provider.Resolve(serviceType, key);
        }

        /// <summary>
        /// 解析服务。如服务未注册则返回空对象
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TService ResolveOptional<TService>(string key = null)
            where TService : class
        {
            if (IsRegisted<TService>(key))
            {
                return Resolve<TService>(key);
            }
            return default(TService);
        }

        /// <summary>
        /// 解析服务。如服务未注册则返回空对象
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object ResolveOptional(Type serviceType, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            if (IsRegisted(serviceType, key))
            {
                return Resolve(serviceType, key);
            }
            return null;
        }

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <returns>目标服务组件实体对象</returns>
        public static TService ResolveUnregist<TService>(/*string key = null*/)
            where TService : class
        {
            return Provider.ResolveUnregist<TService>();
        }

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <returns>目标服务组件实体对象</returns>
        public static object ResolveUnregist(Type serviceType)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            return Provider.ResolveUnregist(serviceType);
        }

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <typeparam name="TService">服务类型</typeparam>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        public static TService ResolveOrUnregist<TService>(string key = null)
            where TService : class
        {
            if (Container.IsRegisted<TService>(key))
            {
                return Provider.Resolve<TService>(key);
            }
            return Provider.ResolveUnregist<TService>();
        }

        /// <summary>
        /// 解析一个服务
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="key">组件唯一识别key</param>
        /// <returns>目标服务组件实体对象</returns>
        public static object ResolveOrUnregist(Type serviceType, string key = null)
        {
            Assert.NotNull(serviceType, nameof(serviceType));
            if (Container.IsRegisted(serviceType, key))
            {
                return Provider.Resolve(serviceType, key);
            }
            return Provider.ResolveUnregist(serviceType);
        }

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <typeparam name="TService">服务类型</typeparam>
        ///// <param name="key">组件唯一识别key</param>
        ///// <param name="instance">实体对象</param>
        ///// <returns>目标服务组件实体对象</returns>
        //public static TService ResolveUnregist<TService>(TService instance, string key = null)
        //    where TService : class
        //{
        //    if (!Container.IsRegisted<TService>(key))
        //    {
        //        Assert.NotNull(instance, nameof(instance));
        //        Container.RegisterInstance<TService>(instance, ComponentLifeStyle.PerRequest, key);
        //    }
        //    return Container.Resolve<TService>(key);
        //}

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <param name="serviceType">服务类型对象</param>
        ///// <param name="instance">实体对象</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //public static object ResolveUnregist(Type serviceType, object instance, string key = null)
        //{
        //    Assert.NotNull(serviceType, nameof(serviceType));
        //    if (!Container.IsRegisted(serviceType, key))
        //    {
        //        Assert.NotNull(instance, nameof(instance));
        //        Container.RegisterInstance(serviceType, instance, ComponentLifeStyle.PerRequest, key);
        //    }
        //    return Container.Resolve(serviceType, key);
        //}

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <typeparam name="TService">服务类型</typeparam>
        ///// <param name="instanceHandler">实体创建句柄</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //public static TService ResolveUnregist<TService>(Func<TService> instanceHandler, string key = null)
        //    where TService : class
        //{
        //    if (!Container.IsRegisted<TService>(key))
        //    {
        //        Assert.NotNull(instanceHandler, nameof(instanceHandler));
        //        Container.RegisterHandler<TService>(instanceHandler, ComponentLifeStyle.PerRequest, key);
        //    }
        //    return Container.Resolve<TService>(key);
        //}

        ///// <summary>
        ///// 解析一个服务
        ///// </summary>
        ///// <param name="serviceType">服务类型对象</param>
        ///// <param name="instanceHandler">实体创建句柄</param>
        ///// <param name="key">组件唯一识别key</param>
        ///// <returns>目标服务组件实体对象</returns>
        //public static object ResolveUnregist(Type serviceType, Func<object> instanceHandler, string key = null)
        //{
        //    Assert.NotNull(serviceType, nameof(serviceType));
        //    if(!Container.IsRegisted(serviceType, key))
        //    {
        //        Assert.NotNull(instanceHandler, nameof(instanceHandler));
        //        Container.RegisterHandler(serviceType, instanceHandler, ComponentLifeStyle.PerRequest, key);
        //    }
        //    return Container.Resolve(serviceType, key);
        //}

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TService TryResolve<TService>(TService instance, string key = null)
            where TService : class
        {
            if (Container.IsRegisted<TService>(key))
            {
                return Provider.Resolve<TService>(key);
            }
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="instanceHandler"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static TService TryResolve<TService>(Func<TService> instanceHandler, string key = null)
            where TService : class
        {
            if (Container.IsRegisted<TService>(key))
            {
                return Provider.Resolve<TService>(key);
            }
            return instanceHandler.Try();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="instance"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object TryResolve(Type serviceType, object instance, string key = null)
        {
            if (Container.IsRegisted(serviceType, key))
            {
                return Provider.Resolve(serviceType, key);
            }
            return instance;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="instanceHandler"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object TryResolve(Type serviceType, Func<object> instanceHandler, string key = null)
        {
            if (Container.IsRegisted(serviceType, key))
            {
                return Provider.Resolve(serviceType, key);
            }
            return instanceHandler.Try();
        }
    }
}
