using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web.WebCore
{
    /// <summary>
    /// 共享的工作上下文接口定义
    /// </summary>
    public interface ISharedWorkContext
    {
        /// <summary>
        /// 判断当前访问是否有登录用户
        /// </summary>
        bool IsCustomerLogined { get; }

        /// <summary>
        /// 获取当前工作账户
        /// </summary>
        IUserAccountIdentity CurrentAccount
        {
            get;
        }

        /// <summary>
        /// 用户登陆
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <param name="createPersistentCookie"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        bool Login(string account, string password, bool createPersistentCookie, string code);

        /// <summary>
        /// 登出
        /// </summary>
        void Logout();
    }
}