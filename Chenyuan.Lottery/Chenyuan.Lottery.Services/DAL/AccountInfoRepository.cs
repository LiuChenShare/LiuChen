using Chenyuan.Data.Base.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.Services
{
    public class AccountInfoRepository
    {
        #region
        private Chenyuan.DAL.IRepository<AccountInfo> _accountInfoRepository;
        public AccountInfoRepository(Chenyuan.DAL.IRepository<AccountInfo> accountInfoRepository)
        {
            _accountInfoRepository = accountInfoRepository;
        }
        #endregion

        /// <summary>
        /// 根据用户账号id查询用户账户信息
        /// </summary>
        /// <param name="id">账户id</param>
        /// <returns></returns>
        public AccountInfo GetById(Guid id)
        {
            var result = _accountInfoRepository.Table.Where(x => !x.Deleted && x.Id == id);
            return result.FirstOrDefault();
        }

        /// <summary>
        /// 根据登录密码获取用户账户集合
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public List<AccountInfo> GetListByPassword(string password)
        {
            var result = _accountInfoRepository.Table.Where(x => !x.Deleted && x.Password == password);
            return result.ToList();
        }

        /// <summary>
        /// 根据登录名获取用户账户集合
        /// </summary>
        /// <param name="userName">Account或者Email或者Mobile</param>
        /// <returns></returns>
        public List<AccountInfo> GetListByUserName(string userName)
        {
            var result = _accountInfoRepository.Table.Where(x => !x.Deleted && ( x.Account == userName || x.Email == userName || x.Mobile == userName));
            return result.ToList();
        }

        #region 
        public void Insert(AccountInfo info)
        {
            _accountInfoRepository.Insert(info);
        }
        public void Update(AccountInfo info)
        {
            _accountInfoRepository.Update(info);
        }
        public void Delete(AccountInfo info)
        {
            _accountInfoRepository.Delete(info);
        }
        #endregion
    }
}
