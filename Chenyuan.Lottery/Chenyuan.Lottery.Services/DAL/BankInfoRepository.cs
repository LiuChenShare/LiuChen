using Chenyuan.Data.Lottery.Entity;
using Chenyuan.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.Services
{
    /// <summary>
    /// 用户的彩票银行仓储服务
    /// </summary>
    public class BankInfoRepository
    {
        #region
        private Chenyuan.DAL.IRepository<BankInfo> _bankInfoRepository;
        public BankInfoRepository(Chenyuan.DAL.IRepository<BankInfo> bankInfoRepository)
        {
            _bankInfoRepository = bankInfoRepository;
        }
        #endregion

        /// <summary>
        /// 根据用户账号查询该用户的银行信息
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public BankInfo GetByAccountId(Guid accountId)
        {
            var result = _bankInfoRepository.Table.Where(x => !x.Deleted && x.AccountId == accountId);
            return result.FirstOrDefault();
        }

        #region 
        public void Insert(BankInfo info)
        {
            _bankInfoRepository.Insert(info);
        }
        public void Update(BankInfo info)
        {
            _bankInfoRepository.Update(info);
        }
        public void Delete(BankInfo info)
        {
            _bankInfoRepository.Delete(info);
        }
        #endregion
    }
}
