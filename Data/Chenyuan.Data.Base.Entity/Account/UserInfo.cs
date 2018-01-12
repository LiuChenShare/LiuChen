using Chenyuan.Date.Entity;
using Chenyuan.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base.Entity
{
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfo : EntityBase<Guid>
    {
        /// <summary>
        /// 账号id
        /// </summary>
        public virtual Guid AccountId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        public virtual string AccountName { get; set; }

        /// <summary>
        /// 姓名
        /// </summary>
        public virtual string Name { get; set; }

        ///// <summary>
        ///// 性别
        ///// </summary>
        //public virtual Gender Gender
        //{
        //    get;
        //    set;
        //}

        /// <summary>
        /// QQ
        /// </summary>
        public virtual string QQ { get; set; }
        /// <summary>
        /// 微信
        /// </summary>
        public virtual string Weixin { get; set; }
        /// <summary>
        /// 微博
        /// </summary>
        public virtual string Weibo { get; set; }
    }
}
