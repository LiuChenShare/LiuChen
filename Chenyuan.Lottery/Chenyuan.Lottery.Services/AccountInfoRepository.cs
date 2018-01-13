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
        /// <param name="accountId"></param>
        /// <returns></returns>
        public AccountInfo GetByAccountId(Guid accountId)
        {
            var result = _accountInfoRepository.Table.Where(x => !x.Deleted && x.Id == accountId);
            return result.FirstOrDefault();
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
