using System;
using System.Collections.Specialized;
using System.Net.WebSockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// WebSocket�����ඨ��
    /// </summary>
	[ServiceBehavior(InstanceContextMode = InstanceContextMode.PerSession, ConcurrencyMode = ConcurrencyMode.Multiple)]
	public abstract class WebSocketService : IDisposable, IWebSocket, IWebSocketCallback
	{
		private bool disposed;
		private IContextChannel channel;

        /// <summary>
        /// Ψһ��ʶKey
        /// </summary>
        public Guid Key
        {
            get;
        }

        /// <summary>
        /// ��ȡ��ǰ�ص��ӿڶ���
        /// </summary>
		protected internal IWebSocketCallback Callback
		{
			get;
			private set;
		}

        /// <summary>
        /// ��ȡ��ǰ�����Ķ���
        /// </summary>
		protected WebSocketContext WebSocketContext
		{
			get;
			private set;
		}

        /// <summary>
        /// ��ȡ��ѯ��������
        /// </summary>
		protected NameValueCollection QueryParameters
		{
			get;
			private set;
		}

        /// <summary>
        /// ��ȡ����URI��Ϣ
        /// </summary>
		protected Uri HttpRequestUri
		{
			get
			{
				return this.WebSocketContext.RequestUri;
			}
		}

        /// <summary>
        /// ���캯��
        /// </summary>
		protected WebSocketService()
		{
			this.channel = OperationContext.Current.Channel;
			this.channel.Closing += new EventHandler(this.Channel_Closing);
			this.channel.Faulted += new EventHandler(this.Channel_Faulted);
			this.Callback = OperationContext.Current.GetCallbackChannel<IWebSocketCallback>();
            this.Key = Guid.NewGuid();
		}

        private void Channel_Closing(object sender, EventArgs e) => this.OnClose();

        private void Channel_Faulted(object sender, EventArgs e) => this.OnError();

        /// <summary>
        /// �ر��¼�����
        /// </summary>
		protected virtual void OnClose()
		{
		}

        /// <summary>
        /// �쳣�¼�����
        /// </summary>
		protected virtual void OnError()
		{
		}

		void IWebSocket.OnOpen()
		{
			WebSocketMessageProperty webSocketMessageProperty = (WebSocketMessageProperty)OperationContext.Current.IncomingMessageProperties["WebSocketMessageProperty"];
			this.WebSocketContext = webSocketMessageProperty.WebSocketContext;
            this.QueryParameters = ParseQueryString(this.WebSocketContext.RequestUri.Query);
			this.OnOpen();
		}

        /// <summary>
        /// ������ѯ�ַ���
        /// </summary>
        /// <param name="queryString"></param>
        /// <returns></returns>
        protected abstract NameValueCollection ParseQueryString(string queryString);

        /// <summary>
        /// ���¼�����
        /// </summary>
		public virtual void OnOpen()
		{
		}

        /// <summary>
        /// ��Ϣ�¼�����
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
		public Task OnMessage(Message message)
		{
			if (message == null)
			{
				throw new ArgumentNullException(nameof(message));
			}
			WebSocketMessageProperty webSocketMessageProperty = (WebSocketMessageProperty)message.Properties["WebSocketMessageProperty"];
			byte[] body = message.GetBody<byte[]>();
			if (webSocketMessageProperty.MessageType == WebSocketMessageType.Binary)
			{
				this.OnMessage(body);
			}
			else
			{
				if (webSocketMessageProperty.MessageType != WebSocketMessageType.Text)
				{
					throw new InvalidOperationException("Unknown message type");
				}
				string @string = Encoding.UTF8.GetString(body);
				this.OnMessage(@string);
			}
			return Task.FromResult<int>(0);
		}

        /// <summary>
        /// ��Ϣ�¼�����
        /// </summary>
        /// <param name="message"></param>
		public virtual void OnMessage(string message)
		{
		}

        /// <summary>
        /// ��Ϣ�¼�����
        /// </summary>
        /// <param name="data"></param>
		public virtual void OnMessage(byte[] data)
		{
		}

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public async Task Send(byte[] value)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			Message message = ByteStreamMessage.CreateMessage(new ArraySegment<byte>(value));
			message.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty
			{
				MessageType = WebSocketMessageType.Binary
			};
			await this.Callback.OnMessage(message);
		}

        /// <summary>
        /// ������Ϣ
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
		public async Task Send(string value)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().FullName);
			}
			Message message = ByteStreamMessage.CreateMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(value)));
			message.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty
			{
				MessageType = WebSocketMessageType.Text
			};
			await this.Callback.OnMessage(message);
		}

        /// <summary>
        /// �ر�ͨ��
        /// </summary>
		public void Close() => this.channel.Close();

		~WebSocketService()
		{
			this.Dispose(false);
		}

        /// <summary>
        /// ���ö���
        /// </summary>
        /// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				this.channel.Closing -= new EventHandler(this.Channel_Closing);
				this.channel.Faulted -= new EventHandler(this.Channel_Faulted);
				this.disposed = true;
			}
		}

        /// <summary>
        /// ���ö���
        /// </summary>
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}
	}
}
