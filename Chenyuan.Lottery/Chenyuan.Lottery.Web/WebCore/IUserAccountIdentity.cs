using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web.WebCore
{
    /// <summary>
    /// 用户账户接口标识定义
    /// </summary>
    public interface IUserAccountIdentity
    {
        /// <summary>
        /// 获取用户Id
        /// </summary>
        Guid Id
        {
            get;
        }

        /// <summary>
        /// 账户最后登录Ip信息
        /// </summary>
        /// <remarks>
        /// 记录账户最后一次登录系统的Ip信息
        /// </remarks>
        //[MaxLength(50)]
        string LastLoginIp
        {
            get;
        }

        /// <summary>
        /// 最后登录日期时间
        /// </summary>
        /// <remarks>
        /// 账户最后一次登录系统的日期和时间
        /// </remarks>
        DateTime? LastLoginedOn
        {
            get;
        }
    }
}