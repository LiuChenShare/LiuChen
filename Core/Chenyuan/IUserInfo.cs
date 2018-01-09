using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan
{
    /// <summary>
    /// 用户数据接口
    /// </summary>
    public interface IUserInfo
    {
        /// <summary>
        /// 通行证id
        /// </summary>
        Guid Id { get; set; }
        /// <summary>
        /// 账户名
        /// </summary>
        string Account { get; set; }
        /// <summary>
        /// 账户id
        /// </summary>
        Guid AccountId { get; set; }

        string Email { get; set; }

        string Mobile { get; set; }
    }
}
