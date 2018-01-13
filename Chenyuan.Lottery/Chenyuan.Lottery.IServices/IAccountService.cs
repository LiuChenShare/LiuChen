using Chenyuan.Data.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.IServices
{
    public interface IAccountService
    {
        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        bool Login(string account, string password);

        /// <summary>
        /// 系统登出
        /// </summary>
        void Logout();

        /// <summary>
        /// 获取用户账户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AccountInfo GetAccountInfo(Guid id);
    }
}
