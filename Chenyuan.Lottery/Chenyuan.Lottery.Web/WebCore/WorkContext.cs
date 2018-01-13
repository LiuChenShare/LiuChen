using System;
using System.Web;
using System.Web.SessionState;
using Chenyuan.Data.Base.Entity;
using Chenyuan.Lottery.IServices;

namespace Chenyuan.Lottery.Web.WebCore
{
    public class WorkContext : IWorkContext
    {
        private readonly HttpContextBase _httpContext;
        private readonly IWebHelper _webHelper;
        private readonly IAccountService _accountService;
        public WorkContext(
            HttpContextBase httpContext,
            IWebHelper webHelper,
            IAccountService accountService
            )
        {

            _httpContext = httpContext;
            _webHelper = webHelper;
            _accountService = accountService;
        }

        private AccountInfo _currentAccount;

        /// <summary>
        /// 获取当前登陆用户
        /// </summary>
        public AccountInfo CurrentAccount
        {
            get
            {
                if (_currentAccount == null)
                {
                    Guid accountId;
                    var _session = HttpContext.Current.Session;
                    if (_session["userName"] != null)
                    {
                        //Response.Write(Session["userName"].ToString() + "---点击获取session"); //获取session，并写入页面
                        accountId = Guid.Parse(_session["userName"].ToString());
                    }

                    //先读session中的accountid值，有的话就读取用户信息

                    //_currentAccount = _accountInfoService.GetWholeAccountInfoById(accountId);

                    //如果没有，就读取Passport的值，并且把accountId存入session

                    //下面为注释的代码，需要重写

                    //if (_webHelper.TryGet(WebHelperKeyType.Session, "CurrentAccount", out accountId) && accountId != Guid.Empty)
                    //{
                    //    _currentAccount = _accountInfoService.GetWholeAccountInfoById(accountId);
                    //}
                    //else
                    //{
                    //    var passport = this.CurrentPassport;
                    //    if (passport != null)
                    //    {
                    //        _currentAccount = passport.AccountInfo;
                    //        _webHelper[WebHelperKeyType.Session, "CurrentAccount"] = _currentAccount.Id;
                    //        EngineContext.Current.Resolve<ILogForLoginService>().Log(passport); //record login log
                    //    }
                    //}
                }
                return _currentAccount;
            }
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string account, string password)
        {
            _webHelper.Clear();
            bool result = _accountService.Login(account, password);
            //if (result)
            //{
            //    _themeLoaded = false;
            //}
            return result;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public void Logout()
        {
            _webHelper.Clear();
            _accountService.Logout();
        }
    }
}