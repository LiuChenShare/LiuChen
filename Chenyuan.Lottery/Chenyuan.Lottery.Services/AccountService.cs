using Chenyuan.Data.Base.Entity;
using Chenyuan.Lottery.IServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Lottery.Services
{
    public class AccountService : IAccountService
    {
        #region 依赖注入
        protected readonly IWorkContext _workContext;
        protected readonly AccountInfoRepository _accountInfoRepository;

        public AccountService(IWorkContext workContext
            , AccountInfoRepository accountInfoRepository)
        {
            _workContext = workContext;
            _accountInfoRepository = accountInfoRepository;
        }
        #endregion

        /// <summary>
        /// 系统登录
        /// </summary>
        /// <param name="account"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public bool Login(string account, string password)
        {
            var userNameList = _accountInfoRepository.GetListByUserName(account);
            var passwordList = _accountInfoRepository.GetListByPassword(password);
            var accountInfo = userNameList.Intersect(passwordList).ToList().FirstOrDefault();
            if (accountInfo != null)
            {
                //存储Session
                var _session = HttpContext.Current.Session;
                _session["userName"] = accountInfo.Id.ToString();  //把用户id保存到session中
                return true;
            }
            return false;
        }

        public void Logout()
        {
            throw new NotImplementedException();
        }

        public AccountInfo GetAccountInfo(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
