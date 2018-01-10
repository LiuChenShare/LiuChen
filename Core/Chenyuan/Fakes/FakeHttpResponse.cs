using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Fakes
{
	/// <summary>
	/// 
	/// </summary>
	public class FakeHttpResponse : HttpResponseBase
	{
		private readonly HttpCookieCollection _cookies;
		/// <summary>
		/// 
		/// </summary>
		public FakeHttpResponse()
		{
			_cookies = new HttpCookieCollection();
		}
		private readonly StringBuilder _outputString = new StringBuilder();

		/// <summary>
		/// 
		/// </summary>
		public string ResponseOutput
		{
			get { return _outputString.ToString(); }
		}

		/// <summary>
		/// 
		/// </summary>
		public override int StatusCode { get; set; }

		/// <summary>
		/// 
		/// </summary>
		public override string RedirectLocation { get; set; }

		/// <summary>
		/// 
		/// </summary>
		/// <param name="s"></param>
		public override void Write(string s)
		{
			_outputString.Append(s);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="virtualPath"></param>
		/// <returns></returns>
		public override string ApplyAppPathModifier(string virtualPath)
		{
			return virtualPath;
		}

		/// <summary>
		/// 
		/// </summary>
		public override HttpCookieCollection Cookies
		{
			get
			{
				return _cookies;
			}
		}
	}
}
