using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.Lottery.Model
{
    /// <summary>
    /// 用户注册模型
    /// </summary>
    public class UserRegisterModel
    {
        /// <summary>
        /// 账户名
        /// </summary>
        [DisplayName("账户名")]
        [Required(ErrorMessage = "不能为空")]
        public string Account { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        [DisplayName("密码")]
        [Required(ErrorMessage = "密码不能为空")]
        public string Password { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        [DisplayName("确认密码")]
        [Compare("Password", ErrorMessage = "两次密码输入不一致")]
        public string PasswordPalt { get; set; }
    }
}
