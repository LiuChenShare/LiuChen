using Chenyuan.Data.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web.WebCore
{
    public interface IWorkContext
    {
        /// <summary>
		/// 用户登陆
		/// </summary>
		/// <param name="account"></param>
		/// <param name="password"></param>
		/// <param name="createPersistentCookie"></param>
		/// <returns></returns>
		bool Login(string account, string password);

        /// <summary>
		/// 登出
		/// </summary>
		void Logout();

        /// <summary>
        /// 获取当前登录账户
        /// </summary>
        AccountInfo CurrentAccount
        {
            get;
        }
    }
}