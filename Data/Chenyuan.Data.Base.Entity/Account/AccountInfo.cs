﻿using Chenyuan.Date.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Data.Base.Entity
{
    /// <summary>
    /// 晨源系统全局账户
    /// </summary>
    public class AccountInfo : EntityBase<Guid>
    {

        /// <summary>
        /// 登录账号
        /// </summary>
        public virtual string Account { get; set; }

        /// <summary>
        /// 最后登录日期时间
        /// </summary>
        /// <remarks>
        /// 账户最后一次登录系统的日期和时间
        /// </remarks>
        public virtual DateTime? LastLoginedOn { get; set; }

        /// <summary>
        /// 账户最后登录Ip信息
        /// </summary>
        /// <remarks>
        /// 记录账户最后一次登录系统的Ip信息
        /// </remarks>
        //bak[MaxLength(50)]
        public virtual string LastLoginIp
        {
            get;
            set;
        }

        /// <summary>
        /// Email地址
        /// </summary>
        //bak[MaxLength(100)]
        public virtual string Email { get; set; }

        /// <summary>
        /// 移动电话号码
        /// </summary>
        //bak[MaxLength(50)]
        public virtual string Mobile { get; set; }

        /// <summary>
        /// 注册码(邀请码)
        /// </summary>
        public virtual string RegisterCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public virtual string Password { get; set; }
    }
}
