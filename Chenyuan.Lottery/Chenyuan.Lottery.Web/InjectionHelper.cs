using Autofac;
using Chenyuan.Autofac;
using Chenyuan.Lottery.IServices;
using Chenyuan.Lottery.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web
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
            //请求上下文注册
            Builder.RegisterType<Chenyuan.Lottery.Web.WebCore.WorkContext>().As<Chenyuan.Lottery.Web.WebCore.IWorkContext>().InstancePerRequest();
            Builder.RegisterType<Chenyuan.Lottery.Web.WebCore.WebHelperBase>().As<Chenyuan.Lottery.Web.WebCore.IWebHelper>().InstancePerRequest();
            //Builder.RegisterType<WebRequestInfo>().As<IRequestInfo>().InstancePerRequest();
            //Builder.RegisterType<WorkContext>().As<IWorkContext>().InstancePerRequest();
            //资源注册
            //Builder.RegisterType<LocaleStringResourceService>().As<ILocaleStringResourceService>();
            //Builder.RegisterType<SettingService>().As<ISettingService>();
            //仓储
            Builder.RegisterType<AccountInfoRepository>();
            //Builder.RegisterType<KaoqinInfoRepository>();
            //Builder.RegisterType<MonthKaoqinInfoRepository>();
            //Builder.RegisterType<VacationInfoRepository>();
            //服务注册
            Builder.RegisterType<AccountService>().As<IAccountService>();
            //Builder.RegisterType<CommonService>();
            //Builder.RegisterType<WeiXinService>().As<IWeiXinService>();
            //Builder.RegisterType<KaoqinService>().As<IKaoqinService>();
            //Builder.RegisterType<LeaveService>().As<ILeaveService>();

            //Builder.RegisterType<FlowProcessBL>().As<IFlowProcess>();
            //Builder.RegisterType<WorkFlowBL>().As<IWorkFlow>();
            //Builder.RegisterType<FlowProcessDL>();
            //Builder.RegisterType<WorkFlowDL>();
        }
    }
}