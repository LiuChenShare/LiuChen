using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Chenyuan.Fakes
{
	/// <summary>
	/// 
	/// </summary>
	public class FakeHttpContext : HttpContextBase
	{
		private readonly HttpCookieCollection _cookies;
		private readonly NameValueCollection _formParams;
		private IPrincipal _principal;
		private readonly NameValueCollection _queryStringParams;
		private readonly string _relativeUrl;
		private readonly string _method;
		private readonly SessionStateItemCollection _sessionItems;
		private readonly NameValueCollection _serverVariables;
		private HttpResponseBase _response;
		private HttpRequestBase _request;
		private readonly Dictionary<object, object> _items;

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public static FakeHttpContext Root()
		{
			return new FakeHttpContext("~/");
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="method"></param>
		public FakeHttpContext(string relativeUrl, string method)
			: this(relativeUrl, method, null, null, null, null, null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		public FakeHttpContext(string relativeUrl)
			: this(relativeUrl, null, null, null, null, null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="principal"></param>
		/// <param name="formParams"></param>
		/// <param name="queryStringParams"></param>
		/// <param name="cookies"></param>
		/// <param name="sessionItems"></param>
		/// <param name="serverVariables"></param>
		public FakeHttpContext(string relativeUrl,
			IPrincipal principal, NameValueCollection formParams,
			NameValueCollection queryStringParams, HttpCookieCollection cookies,
			SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
			: this(relativeUrl, null, principal, formParams, queryStringParams, cookies, sessionItems, serverVariables)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="method"></param>
		/// <param name="principal"></param>
		/// <param name="formParams"></param>
		/// <param name="queryStringParams"></param>
		/// <param name="cookies"></param>
		/// <param name="sessionItems"></param>
		/// <param name="serverVariables"></param>
		public FakeHttpContext(string relativeUrl, string method,
			IPrincipal principal, NameValueCollection formParams,
			NameValueCollection queryStringParams, HttpCookieCollection cookies,
			SessionStateItemCollection sessionItems, NameValueCollection serverVariables)
		{
			_relativeUrl = relativeUrl;
			_method = method;
			_principal = principal;
			_formParams = formParams;
			_queryStringParams = queryStringParams;
			_cookies = cookies;
			_sessionItems = sessionItems;
			_serverVariables = serverVariables;

			_items = new Dictionary<object, object>();
		}

		/// <summary>
		/// 
		/// </summary>
		public override HttpRequestBase Request
		{
			get
			{
				return _request ??
					   new FakeHttpRequest(_relativeUrl, _method, _formParams, _queryStringParams, _cookies, _serverVariables);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="request"></param>
		public void SetRequest(HttpRequestBase request)
		{
			_request = request;
		}

		/// <summary>
		/// 
		/// </summary>
		public override HttpResponseBase Response
		{
			get
			{
				return _response ?? new FakeHttpResponse();
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="response"></param>
		public void SetResponse(HttpResponseBase response)
		{
			_response = response;
		}

		/// <summary>
		/// 
		/// </summary>
		public override IPrincipal User
		{
			get { return _principal; }
			set { _principal = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override HttpSessionStateBase Session
		{
			get { return new FakeHttpSessionState(_sessionItems); }
		}

		/// <summary>
		/// 
		/// </summary>
		public override System.Collections.IDictionary Items
		{
			get
			{
				return _items;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool SkipAuthorization { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="serviceType"></param>
		/// <returns></returns>
		public override object GetService(Type serviceType)
		{
			return null;
		}
	}
}
