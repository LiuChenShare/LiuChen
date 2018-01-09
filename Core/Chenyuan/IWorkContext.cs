using System.Globalization;

namespace Chenyuan
{
    /// <summary>
    /// 工作环境上下文
    /// </summary>
    public interface IWorkContext
    {
        /// <summary>
        /// 
        /// </summary>
        CultureInfo CultureInfo
        {
            get;
        }
        /// <summary>
        /// 当前应用
        /// </summary>
        IAppInfo CurrentApp { get; }
        /// <summary>
        /// 当前用户
        /// </summary>
        IUserInfo CurrentUser { get; }

        /// <summary>
        /// 重置CurrentUser
        /// </summary>
        /// <param name="referenceId"></param>
        IUserInfo ResetCurrentUser(System.Guid? referenceId);
    }
}
