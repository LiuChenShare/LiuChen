using Autofac;
using Chenyuan.Autofac;
using Chenyuan.Data.Base;
using Chenyuan.Data.Base.Entity;
using Chenyuan.Date.V2;
using System.Web.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Autofac.Integration.WebApi;
using System.Reflection;
using Autofac.Integration.Mvc;

namespace Chenyuan.Lottery.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //在Global.asax的Application_Start中注册定时任务  
            Timedtask.CensusdemoTask censusdemoTask = new Timedtask.CensusdemoTask();


            ////依赖注入
            //var injection = DependencyInjection.Current;
            ////register datacontext
            //injection.Builder.Register<BaseDataContext>(c => new BaseDataContext(new ChenyuanDatabase())).Keyed<DataContext>(typeof(ChenyuanDatabase)).InstancePerRequest();
            ////injection.Builder.Register<HRDataContext>(c => new HRDataContext(new HRDatabase())).Keyed<DataContext>(typeof(HRDatabase)).InstancePerRequest();
            ////injection.Builder.Register<OperateDataContext>(c => new OperateDataContext(new OperateDatabase())).Keyed<DataContext>(typeof(OperateDatabase)).InstancePerRequest();
            ////injection.Builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //injection.Builder.RegisterControllers(Assembly.GetExecutingAssembly());
            //DependencyInjection.Current.ScopeContainer = injection.Builder.Build();
            //GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(DependencyInjection.Current.ScopeContainer);
            ////AutofacWebApiDependencyResolver
            ///*JSON序列化loop处理*/
            //var json = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            //json.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //启动auto
            AutoFacConfig.Register();
        }
    }
}
