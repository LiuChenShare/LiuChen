using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.WebPages;

namespace Chenyuan.Lottery.Web.WebCore
{
    public abstract partial class WebHelperBase : IWebHelper
    {
        /// <summary>
        /// 当前请求上下文
        /// </summary>
        protected readonly HttpContextBase _httpContext;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="httpContext">HTTP context</param>
        public WebHelperBase(HttpContextBase httpContext)
        {
            _httpContext = httpContext;
        }

        /// <summary>
        /// Get context IP address
        /// </summary>
        /// <returns>URL referrer</returns>
        public string GetCurrentIpAddress()
        {
            if (_httpContext != null &&
                    _httpContext.Request != null &&
                    _httpContext.Request.UserHostAddress != null)
                return _httpContext.Request.UserHostAddress;
            else
                return string.Empty;
        }

        /// <summary>
        /// Get URL referrer
        /// </summary>
        /// <returns>URL referrer</returns>
        public string GetUrlReferrer()
        {
            string referrerUrl = string.Empty;

            if (_httpContext != null &&
                _httpContext.Request != null &&
                _httpContext.Request.UrlReferrer != null)
                referrerUrl = _httpContext.Request.UrlReferrer.ToString();

            return referrerUrl;
        }

        /// <summary>
        /// 清除缓存
        /// </summary>
        /// <param name="keyType"></param>
        public void Clear()
        {
            if (_httpContext.Session != null)
            {
                _httpContext.Session.Abandon();
                _httpContext.Session.Clear();
            }
        }

    }
}