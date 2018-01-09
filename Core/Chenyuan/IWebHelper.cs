using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan
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
	/// <summary>
	/// Represents a common helper
	/// </summary>
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
		/// Get context User HostName
		/// </summary>
		/// <returns>URL referrer</returns>
		string GetUserHostName();

		/// <summary>
		/// Get context Server Machine name
		/// </summary>
		/// <returns>URL referrer</returns>
		string GetServerMachineName();

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <returns>Page name</returns>
		string GetThisPageUrl(bool includeQueryString);

		/// <summary>
		/// Gets this page name
		/// </summary>
		/// <param name="includeQueryString">Value indicating whether to include query strings</param>
		/// <param name="useSsl">Value indicating whether to get SSL protected page</param>
		/// <returns>Page name</returns>
		string GetThisPageUrl(bool includeQueryString, bool useSsl);

		/// <summary>
		/// Gets a value indicating whether current connection is secured
		/// </summary>
		/// <returns>true - secured, false - not secured</returns>
		bool IsCurrentConnectionSecured();

		/// <summary>
		/// Gets server variable by name
		/// </summary>
		/// <param name="name">Name</param>
		/// <returns>Server variable</returns>
		string ServerVariables(string name);

		/// <summary>
		/// Gets store location
		/// </summary>
		/// <returns>Store location</returns>
		string GetRequestLocation();

		/// <summary>
		/// Gets store location
		/// </summary>
		/// <param name="useSsl">Use SSL</param>
		/// <returns>Store location</returns>
		string GetRequestLocation(bool useSsl);

		/// <summary>
		/// Returns true if the requested resource is one of the typical resources that needn't be processed by the cms engine.
		/// </summary>
		/// <param name="request">HTTP Request</param>
		/// <returns>True if the request targets a static resource file.</returns>
		/// <remarks>
		/// These are the file extensions considered to be static resources:
		/// .css
		///	.gif
		/// .png 
		/// .jpg
		/// .jpeg
		/// .js
		/// .axd
		/// .ashx
		/// </remarks>
		bool IsStaticResource(HttpRequest request);

		/// <summary>
		/// Maps a virtual path to a physical disk path.
		/// </summary>
		/// <param name="path">The path to map. E.g. "~/bin"</param>
		/// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
		string MapPath(string path);

		/// <summary>
		/// 读取或设置Web键值
		/// </summary>
		/// <param name="keyType">键值存储位置</param>
		/// <param name="key">键</param>
		/// <returns>值</returns>
		object this[WebHelperKeyType keyType, string key]
		{
			get;
			set;
		}

		/// <summary>
		/// 尝试获取配置数据
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="keyType"></param>
		/// <param name="key"></param>
		/// <param name="result"></param>
		/// <returns></returns>
		bool TryGet<T>(WebHelperKeyType keyType, string key, out T result);

		/// <summary>
		/// Modifies query string
		/// </summary>
		/// <param name="url">Url to modify</param>
		/// <param name="queryStringModification">Query string modification</param>
		/// <param name="anchor">Anchor</param>
		/// <returns>New url</returns>
		string ModifyQueryString(string url, string queryStringModification, string anchor);

		/// <summary>
		/// Remove query string from url
		/// </summary>
		/// <param name="url">Url to modify</param>
		/// <param name="queryString">Query string to remove</param>
		/// <returns>New url</returns>
		string RemoveQueryString(string url, string queryString);

		/// <summary>
		/// Gets query string value by name
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="name">Parameter name</param>
		/// <returns>Query string value</returns>
		T QueryString<T>(string name);

		/// <summary>
		/// Restart application domain
		/// </summary>
		/// <param name="makeRedirect">A value indicating whether </param>
		/// <param name="redirectUrl">Redirect URL; empty string if you want to redirect to the current page URL</param>
		void RestartAppDomain(bool makeRedirect = false, string redirectUrl = "");

		/// <summary>
		/// Get a value indicating whether the request is made by search engine (web crawler)
		/// </summary>
		/// <param name="context">HTTP context</param>
		/// <returns>Result</returns>
		bool IsSearchEngine(HttpContextBase context);

		/// <summary>
		/// Gets a value that indicates whether the client is being redirected to a new location
		/// </summary>
		bool IsRequestBeingRedirected { get; }

		/// <summary>
		/// Gets or sets a value that indicates whether the client is being redirected to a new location using POST
		/// </summary>
		bool IsPostBeingDone { get; set; }

		/// <summary>
		/// Gets the requested store hostname.
		/// </summary>
		string Host
		{
			get;
		}

		/// <summary>
		/// 是否主题测试状态
		/// </summary>
		bool ThemeTest
		{
			get;
		}

		/// <summary>
		/// 测试主题名称
		/// </summary>
		string TestThemeName
		{
			get;
		}

		///// <summary>
		///// 登录请求信息
		///// </summary>
		//ILoginRequestInfo LoginRequestInfo
		//{
		//	get;
		//}
		/// <summary>
		/// 清除缓存
		/// </summary>
		/// <param name="keyType"></param>
		void Clear(WebHelperKeyType keyType);
	}
}
