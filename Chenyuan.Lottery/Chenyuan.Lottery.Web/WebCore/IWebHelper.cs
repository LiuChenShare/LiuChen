using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Chenyuan.Lottery.Web.WebCore
{
    /// <summary>
    /// WebHelper键值类型定义
    /// </summary>
    public enum WebHelperKeyType
    {
        /// <summary>
        /// 应用程序键
        /// </summary>
        Application,
        /// <summary>
        /// 会话状态键
        /// </summary>
        Session,
        /// <summary>
        /// Post方式表单键
        /// </summary>
        Form,
        /// <summary>
        /// Get方式请求键
        /// </summary>
        QueryString,
        /// <summary>
        /// 所有的请求键
        /// </summary>
        Request,
        /// <summary>
        /// Cookie数据
        /// </summary>
        Cookie,
    }
    public partial interface IWebHelper
    {
        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetUrlReferrer();

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        string GetCurrentIpAddress();

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keyType"></param>
        void Clear();
    }
}