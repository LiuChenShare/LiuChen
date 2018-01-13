using Chenyuan.Autofac;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Chenyuan.Lottery.Web.Controllers
{
    /// <summary>
    /// 晨源系统控制器基类定义
    /// </summary>
    public abstract class ChenyuanControllerBase : Controller
    {
        protected readonly IWorkContext _workContext;

        public ChenyuanControllerBase()
        {
            _workContext = DependencyInjection.Current.Resolve<IWorkContext>();
        }

        //private Chenyuan.Lottery.Web.WebCore.IWorkContext _workContext;
        //protected Chenyuan.Lottery.Web.WebCore.IWorkContext WorkContext
        //{
        //    get
        //    {
        //        if (_workContext == null)
        //        {
        //            //_workContext = EngineContext.Current.Resolve<IWorkContext>();
        //        }
        //        return _workContext;
        //    }
        //}
        
    }
}