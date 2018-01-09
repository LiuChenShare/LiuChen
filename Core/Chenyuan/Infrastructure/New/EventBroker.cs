using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Chenyuan.Infrastructure
{
	/// <summary>
	/// A broker for events from the http application. The purpose of this 
	/// class is to reduce the risk of temporary errors during initialization
	/// causing the site to be crippled.
	/// </summary>
	public class EventBroker
	{
		static EventBroker()
		{
			Instance = new EventBroker();
		}

		/// <summary>Accesses the event broker singleton instance.</summary>
		public static EventBroker Instance
		{
			get { return Singleton<EventBroker>.Instance; }
			protected set { Singleton<EventBroker>.Instance = value; }
		}

		/// <summary>Attaches to events from the application instance.</summary>
		public virtual void Attach(HttpApplication application)
		{
			Trace.WriteLine("EventBroker: Attaching to " + application);

			application.BeginRequest += Application_BeginRequest;
			application.AuthorizeRequest += Application_AuthorizeRequest;

			application.PostResolveRequestCache += Application_PostResolveRequestCache;
			application.PostMapRequestHandler += Application_PostMapRequestHandler;

			application.AcquireRequestState += Application_AcquireRequestState;
			application.Error += Application_Error;
			application.EndRequest += Application_EndRequest;

			application.Disposed += Application_Disposed;
		}

		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> BeginRequest;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> AuthorizeRequest;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> PostResolveRequestCache;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> AcquireRequestState;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> PostMapRequestHandler;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> Error;
		/// <summary>
		/// 
		/// </summary>
		public EventHandler<EventArgs> EndRequest;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_BeginRequest(object sender, EventArgs e)
		{
			if (BeginRequest != null)
			{
				Debug.WriteLine("Application_BeginRequest");
				BeginRequest(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_AuthorizeRequest(object sender, EventArgs e)
		{
			if (AuthorizeRequest != null)
			{
				Debug.WriteLine("Application_AuthorizeRequest");
				AuthorizeRequest(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Application_PostResolveRequestCache(object sender, EventArgs e)
		{
			if (PostResolveRequestCache != null)
			{
				Debug.WriteLine("Application_PostResolveRequestCache");
				PostResolveRequestCache(sender, e);
			}
		}

		private void Application_PostMapRequestHandler(object sender, EventArgs e)
		{
			if (PostMapRequestHandler != null)
			{
				Debug.WriteLine("Application_PostMapRequestHandler");
				PostMapRequestHandler(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_AcquireRequestState(object sender, EventArgs e)
		{
			if (AcquireRequestState != null)
			{
				Debug.WriteLine("Application_AcquireRequestState");
				AcquireRequestState(sender, e);
			}
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_Error(object sender, EventArgs e)
		{
			if (Error != null)
				Error(sender, e);
		}

		/// <summary>
		/// 
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		protected void Application_EndRequest(object sender, EventArgs e)
		{
			if (EndRequest != null)
				EndRequest(sender, e);
		}

		/// <summary>Detaches events from the application instance.</summary>
		void Application_Disposed(object sender, EventArgs e)
		{
			Trace.WriteLine("EventBroker: Disposing " + sender);
		}
	}
}
