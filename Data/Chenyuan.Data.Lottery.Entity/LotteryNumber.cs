using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Lottery.Entity
{
    /// <summary>
    /// 彩票开奖记录
    /// </summary>
    public class LotteryNumber : EntityBase<Guid>
    {
        /// <summary>
        /// 开奖时间
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 万位
        /// </summary>
        public virtual int Wan { get; set; }

        /// <summary>
        /// 千位
        /// </summary>
        public virtual int Qian { get; set; }

        /// <summary>
        /// 百位
        /// </summary>
        public virtual int Bai { get; set; }

        /// <summary>
        /// 十位
        /// </summary>
        public virtual int Shi { get; set; }

        /// <summary>
        /// 个位
        /// </summary>
        public virtual int Ge { get; set; }
    }
}
