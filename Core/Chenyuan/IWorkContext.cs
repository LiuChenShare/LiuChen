using System.Globalization;

namespace Chenyuan
{
    /// <summary>
    /// 工作环境上下文
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 当前用户
        /// </summary>
        IUserInfo CurrentUser { get; }
    }
}
