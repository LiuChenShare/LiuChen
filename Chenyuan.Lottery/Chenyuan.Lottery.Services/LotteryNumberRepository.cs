using Chenyuan.Data.Lottery.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.Services
{
    public class LotteryNumberRepository
    {
        #region
        private Chenyuan.DAL.IRepository<LotteryNumber> _lotteryNumberRepository;
        public LotteryNumberRepository(Chenyuan.DAL.IRepository<LotteryNumber> lotteryNumberRepository)
        {
            _lotteryNumberRepository = lotteryNumberRepository;
        }
        #endregion
        
        public LotteryNumber GetById(Guid id)
        {
            var result = _lotteryNumberRepository.Table.Where(x => !x.Deleted && x.Id == id);
            return result.FirstOrDefault();
        }

        #region 
        public void Insert(LotteryNumber info)
        {
            _lotteryNumberRepository.Insert(info);
        }
        public void Update(LotteryNumber info)
        {
            _lotteryNumberRepository.Update(info);
        }
        public void Delete(LotteryNumber info)
        {
            _lotteryNumberRepository.Delete(info);
        }
        #endregion
    }
}
