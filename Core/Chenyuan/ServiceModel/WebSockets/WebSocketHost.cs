using System;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Description;

namespace Chenyuan.ServiceModel.WebSockets
{

    /// <summary>
    /// WebSocket宿主类定义
    /// </summary>
	public class WebSocketHost : ServiceHost
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="baseAddresses">服务监听地址</param>
		public WebSocketHost(Type serviceType, params Uri[] baseAddresses) : this(serviceType, new ServiceThrottlingBehavior
		{
			MaxConcurrentSessions = 2147483647
		}, baseAddresses)
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceType">服务类型对象</param>
        /// <param name="serviceThrottles"></param>
        /// <param name="baseAddresses"></param>
		public WebSocketHost(Type serviceType, ServiceThrottlingBehavior serviceThrottles, params Uri[] baseAddresses) : base(serviceType, WebSocketHost.RewriteToHttp(baseAddresses))
		{
			base.Description.Behaviors.Add(serviceThrottles);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="https"></param>
        /// <param name="sendBufferSize"></param>
        /// <param name="receiveBufferSize"></param>
        /// <param name="subProtocol"></param>
        /// <returns></returns>
		public static Binding CreateWebSocketBinding(bool https, int sendBufferSize = 0, int receiveBufferSize = 0, string subProtocol = null)
		{
			ByteStreamMessageEncodingBindingElement byteStreamMessageEncodingBindingElement = new ByteStreamMessageEncodingBindingElement();
			byteStreamMessageEncodingBindingElement.MessageVersion = MessageVersion.None;
			HttpTransportBindingElement httpTransportBindingElement = https ? new HttpsTransportBindingElement() : new HttpTransportBindingElement();
			httpTransportBindingElement.WebSocketSettings.TransportUsage = WebSocketTransportUsage.Always;
			httpTransportBindingElement.WebSocketSettings.CreateNotificationOnConnection = true;
			if (subProtocol != null)
			{
				httpTransportBindingElement.WebSocketSettings.SubProtocol = subProtocol;
			}
			return new CustomBinding(new BindingElement[]
			{
				byteStreamMessageEncodingBindingElement,
				httpTransportBindingElement
			})
			{
				ReceiveTimeout = TimeSpan.FromHours(24.0)
			};
		}

		private static Uri[] RewriteToHttp(Uri[] uris)
		{
			return uris.Select(new Func<Uri, Uri>(WebSocketHost.RewriteToHttp)).ToArray<Uri>();
		}

		private static Uri RewriteToHttp(Uri uri)
		{
			if (!uri.IsAbsoluteUri)
			{
				return uri;
			}
			string scheme;
			if ((scheme = uri.Scheme) != null)
			{
				if (scheme == "ws")
				{
					return new Uri("http" + uri.AbsoluteUri.Substring(2));
				}
				if (scheme == "wss")
				{
					return new Uri("https" + uri.AbsoluteUri.Substring(3));
				}
				if (scheme == "http" || scheme == "https")
				{
					return uri;
				}
			}
			throw new ArgumentException("Must supply a websocket address (ws:// or wss://) or a http address (http:// or https://).");
		}

        /// <summary>
        /// 全IP监听
        /// </summary>
        /// <returns></returns>
		public ServiceEndpoint AddWebSocketEndpoint()
		{
			return AddServiceEndpoint(typeof(IWebSocket), CreateWebSocketBinding(false, 0, 0, null), string.Empty);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="binding"></param>
        /// <returns></returns>
		public ServiceEndpoint AddWebSocketEndpoint(Binding binding)
		{
			return AddServiceEndpoint(typeof(IWebSocket), binding, string.Empty);
		}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="binding"></param>
        /// <returns></returns>
		public ServiceEndpoint AddWebSocketEndpoint(Uri uri, Binding binding)
		{
			return AddServiceEndpoint(typeof(IWebSocket), binding, RewriteToHttp(uri));
		}

        /// <summary>
        /// 全IP监听
        /// </summary>
        /// <param name="sendBufferSize"></param>
        /// <param name="receiveBufferSize"></param>
        /// <param name="subProtocol"></param>
        /// <returns></returns>
		public ServiceEndpoint AddWebSocketEndpoint(int sendBufferSize, int receiveBufferSize, string subProtocol)
		{
			return AddServiceEndpoint(typeof(IWebSocket), CreateWebSocketBinding(false, sendBufferSize, receiveBufferSize, subProtocol), string.Empty);
		}
	}

    /// <summary>
    /// 基于泛型的WebSocket宿主类
    /// </summary>
    /// <typeparam name="T"></typeparam>
	public class WebSocketHost<T> : WebSocketHost where T : WebSocketService
	{
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="baseAddresses"></param>
		public WebSocketHost(params Uri[] baseAddresses) : base(typeof(T), baseAddresses)
		{
		}

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="serviceThrottles"></param>
        /// <param name="baseAddresses"></param>
		public WebSocketHost(ServiceThrottlingBehavior serviceThrottles, params Uri[] baseAddresses) : base(typeof(T), serviceThrottles, baseAddresses)
		{
		}
	}
}
