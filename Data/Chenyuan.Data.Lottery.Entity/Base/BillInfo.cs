using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Lottery.Entity
{
    public class BillInfo : EntityBase<Guid>
    {
        /// <summary>
        /// 账号id
        /// </summary>
        public virtual Guid AccountId { get; set; }

        /// <summary>
        /// 彩票id
        /// </summary>
        public virtual Guid LotteryNumberId { get; set; }

        /// <summary>
        /// 金额基数
        /// </summary>
        public virtual double GoldBase { get; set; }

        /// <summary>
        /// 下单总额
        /// </summary>
        public virtual double GoldSum { get; set; }

        #region 购买的彩票
        /// <summary>
        /// 万位
        /// </summary>
        public virtual int[] Wans { get; set; }

        /// <summary>
        /// 千位
        /// </summary>
        public virtual int[] Qians { get; set; }

        /// <summary>
        /// 百位
        /// </summary>
        public virtual int[] Bais { get; set; }

        /// <summary>
        /// 十位
        /// </summary>
        public virtual int[] Shis { get; set; }

        /// <summary>
        /// 个位
        /// </summary>
        public virtual int[] Ges { get; set; }
        #endregion

        /// <summary>
        /// 奖金
        /// </summary>
        public virtual double Bonus { get; set; }

        /// <summary>
        /// 是否开奖
        /// </summary>
        public virtual bool Open { get; set; }
    }
}
