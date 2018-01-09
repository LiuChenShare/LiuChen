using Autofac;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Autofac
{
    public class DependencyInjection
    {
        private static DependencyInjection instance;
        private static readonly object syncRoot = new object();
        private ContainerBuilder builder;
        public static readonly object HttpRequestTag = "AutofacWebRequest";
        /// <summary>
        /// injection container
        /// </summary>
        public IContainer ScopeContainer { get; set; }

        public ContainerBuilder Builder
        {
            get
            {
                return builder;
            }
        }

        static ILifetimeScope LifetimeScope
        {
            get
            {
                return (ILifetimeScope)HttpContext.Current.Items[typeof(ILifetimeScope)];
            }
            set
            {
                HttpContext.Current.Items[typeof(ILifetimeScope)] = value;
            }
        }

        public ILifetimeScope Scope()
        {
            try
            {
                if (HttpContext.Current != null)
                {
                    return LifetimeScope ?? (LifetimeScope = ScopeContainer.BeginLifetimeScope(HttpRequestTag));
                }
                else
                {
                    return ScopeContainer.BeginLifetimeScope(HttpRequestTag);
                }
            }
            catch
            {
                return ScopeContainer;
            }
        }
        /// <summary>
        /// 解析服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <returns></returns>
        public TService Resolve<TService>()
        {
            TService service = default(TService);
            try
            {
                service = this.ScopeContainer.Resolve<TService>();
            }
            catch { }
            if (service == null)
            {
                try
                {
                    service = this.Scope().Resolve<TService>();
                }
                catch { }
            }
            return service;
        }


        private DependencyInjection()
        {
            Initialise();
        }

        public static DependencyInjection Current
        {
            get
            {
                return DependencyInjection.GetInstance();
            }
        }

        private static DependencyInjection GetInstance()
        {
            if (instance == null)
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new DependencyInjection();
                    }
                }
            }
            return instance;
        }
        /// <summary>
        /// injection 
        /// </summary>
        private void Initialise()
        {
            string binPath = Assembly.GetExecutingAssembly().CodeBase; //AppContext.BaseDirectory + "bin";
            binPath = binPath.Substring(0, binPath.LastIndexOf("/")).Replace("file:///", "");
            if (Directory.Exists(binPath))
            {
                foreach (string dllPath in Directory.GetFiles(binPath, "Chenyuan.*.dll"))
                {
                    try
                    {
                        var an = AssemblyName.GetAssemblyName(dllPath);
                        AppDomain.CurrentDomain.Load(an);
                    }
                    catch (BadImageFormatException ex)
                    {
                    }
                }
            }
            builder = new ContainerBuilder();
            var assemblys = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.FullName.StartsWith("Chenyuan."));
            foreach (var assembly in assemblys)
            {
                try
                {
                    var types = assembly.GetTypes().Where(t => t.GetInterfaces().Contains(typeof(IInjection)));
                    foreach (var type in types)
                    {
                        IInjection injectionHelper = Activator.CreateInstance(type) as IInjection;
                        injectionHelper.Builder = builder;
                        injectionHelper.ServiceInjection();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            //ScopeContainer = builder.Build();
            //builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            //ScopeContainer = builder.Build();
            //DependencyResolver.SetResolver(new AutofacDependencyResolver(DependencyInjection.Current.ScopeContainer));
            //GlobaChenyuanonfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(DependencyInjection.Current.ScopeContainer);
        }

    }
}
