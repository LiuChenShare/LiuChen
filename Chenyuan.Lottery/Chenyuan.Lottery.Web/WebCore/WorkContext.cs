using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web.WebCore
{
    public class WorkContext : IWorkContext
    {
        private IUserInfo _currentUser;

        /// <summary>
        /// 获取当前登陆用户
        /// </summary>
        public IUserInfo CurrentUser
        {
            get
            {
                if (_currentUser == null)
                {
                    Guid accountId;
                    var _session = HttpContext.Current.Session;
                    if (_session["userName"] != null)
                    {
                        //Response.Write(Session["userName"].ToString() + "---点击获取session"); //获取session，并写入页面
                        accountId = Guid.Parse(_session["userName"].ToString());
                        _currentUser.AccountId = accountId;
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
                return _currentUser;
            }
        }
    }
}