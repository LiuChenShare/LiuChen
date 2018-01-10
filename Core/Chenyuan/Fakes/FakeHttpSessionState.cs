using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace Chenyuan.Fakes
{
	/// <summary>
	/// 
	/// </summary>
	public class FakeHttpSessionState : HttpSessionStateBase
	{
		private readonly SessionStateItemCollection _sessionItems;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sessionItems"></param>
		public FakeHttpSessionState(SessionStateItemCollection sessionItems)
		{
			_sessionItems = sessionItems;
		}

		/// <summary>
		/// 
		/// </summary>
		public override int Count
		{
			get { return _sessionItems.Count; }
		}

		/// <summary>
		/// 
		/// </summary>
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get { return _sessionItems.Keys; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public override object this[string name]
		{
			get { return _sessionItems[name]; }
			set { _sessionItems[name] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public bool Exists(string key)
		{
			return _sessionItems[key] != null;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		public override object this[int index]
		{
			get { return _sessionItems[index]; }
			set { _sessionItems[index] = value; }
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		/// <param name="value"></param>
		public override void Add(string name, object value)
		{
			_sessionItems[name] = value;
		}

		/// <summary>
		/// 
		/// </summary>
		/// <returns></returns>
		public override IEnumerator GetEnumerator()
		{
			return _sessionItems.GetEnumerator();
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="name"></param>
		public override void Remove(string name)
		{
			_sessionItems.Remove(name);
		}
	}
}
