using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Fakes
{
	/// <summary>
	/// 
	/// </summary>
	public class FakeHttpRequest : HttpRequestBase
	{
		private readonly HttpCookieCollection _cookies;
		private readonly NameValueCollection _formParams;
		private readonly NameValueCollection _queryStringParams;
		private readonly NameValueCollection _serverVariables;
		private readonly string _relativeUrl;
		private readonly Uri _url;
		private readonly Uri _urlReferrer;
		private readonly string _httpMethod;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="method"></param>
		/// <param name="formParams"></param>
		/// <param name="queryStringParams"></param>
		/// <param name="cookies"></param>
		/// <param name="serverVariables"></param>
		public FakeHttpRequest(string relativeUrl, string method,
			NameValueCollection formParams, NameValueCollection queryStringParams,
			HttpCookieCollection cookies, NameValueCollection serverVariables)
		{
			_httpMethod = method;
			_relativeUrl = relativeUrl;
			_formParams = formParams;
			_queryStringParams = queryStringParams;
			_cookies = cookies;
			_serverVariables = serverVariables;
			//ensure collections are not null
			if (_formParams == null)
				_formParams = new NameValueCollection();
			if (_queryStringParams == null)
				_queryStringParams = new NameValueCollection();
			if (_cookies == null)
				_cookies = new HttpCookieCollection();
			if (_serverVariables == null)
				_serverVariables = new NameValueCollection();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="method"></param>
		/// <param name="url"></param>
		/// <param name="urlReferrer"></param>
		/// <param name="formParams"></param>
		/// <param name="queryStringParams"></param>
		/// <param name="cookies"></param>
		/// <param name="serverVariables"></param>
		public FakeHttpRequest(string relativeUrl, string method, Uri url, Uri urlReferrer,
			NameValueCollection formParams, NameValueCollection queryStringParams,
			HttpCookieCollection cookies, NameValueCollection serverVariables)
			: this(relativeUrl, method, formParams, queryStringParams, cookies, serverVariables)
		{
			_url = url;
			_urlReferrer = urlReferrer;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="relativeUrl"></param>
		/// <param name="method"></param>
		/// <param name="url"></param>
		/// <param name="urlReferrer"></param>
		public FakeHttpRequest(string relativeUrl, string method, Uri url, Uri urlReferrer)
			: this(relativeUrl, method, url, urlReferrer, null, null, null, null)
		{
		}

		/// <summary>
		/// 
		/// </summary>
		public override NameValueCollection ServerVariables
		{
			get
			{
				return _serverVariables;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override NameValueCollection Form
		{
			get { return _formParams; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override NameValueCollection QueryString
		{
			get { return _queryStringParams; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override HttpCookieCollection Cookies
		{
			get { return _cookies; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override string AppRelativeCurrentExecutionFilePath
		{
			get { return _relativeUrl; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override Uri Url
		{
			get
			{
				return _url;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override Uri UrlReferrer
		{
			get
			{
				return _urlReferrer;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override string PathInfo
		{
			get { return ""; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override string ApplicationPath
		{
			get
			{
				//we know that relative paths always start with ~/
				//ApplicationPath should start with /
				if (_relativeUrl != null && _relativeUrl.StartsWith("~/"))
					return _relativeUrl.Remove(0, 1);
				return null;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override string HttpMethod
		{
			get
			{
				return _httpMethod;
			}
		}

		/// <summary>
		/// 
		/// </summary>
		public override string UserHostAddress
		{
			get { return null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override string RawUrl
		{
			get { return null; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsSecureConnection
		{
			get { return false; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override bool IsAuthenticated
		{
			get
			{
				return false;
			}
		}

		// codehint: sm-add
		/// <summary>
		/// 
		/// </summary>
		public override string[] UserLanguages
		{
			get
			{
				return new string[] { };
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public override string this[string key]
		{
			get
			{
				try
				{
					return base[key];
				}
				catch
				{
					return key;
				}
			}
		}
	}
}
