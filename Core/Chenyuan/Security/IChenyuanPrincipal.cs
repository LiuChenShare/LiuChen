using System.Collections.Generic;
using System.Security.Principal;

namespace Chenyuan.Security
{
    /// <summary>
    /// 
    /// </summary>
    public interface IChenyuanPrincipal : IPrincipal
    {
        /// <summary>
        /// 判断是否在角色范围内
        /// </summary>
        /// <param name="roles"></param>
        /// <returns></returns>
        bool IsInRole(IEnumerable<string> roles);
    }
}
