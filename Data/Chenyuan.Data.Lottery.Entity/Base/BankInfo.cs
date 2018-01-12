using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Lottery.Entity
{
    /// <summary>
    /// 用户的彩票银行
    /// </summary>
    public class BankInfo : EntityBase<Guid>
    {
        /// <summary>
        /// 账号id
        /// </summary>
        public virtual Guid AccountId { get; set; }

        /// <summary>
        /// 金币
        /// </summary>
        public virtual double Gold { get; set; }

        /// <summary>
        /// 倍率
        /// </summary>
        public virtual double Rate { get; set; }

        /// <summary>
        /// 最大连胜
        /// </summary>
        public virtual double MaxWin { get; set; }

        /// <summary>
        /// 最大连败
        /// </summary>
        public virtual double MaxDefeat { get; set; }
    }
}
