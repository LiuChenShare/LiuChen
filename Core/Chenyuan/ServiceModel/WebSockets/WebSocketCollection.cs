using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// WebSocket集合类定义
    /// </summary>
    /// <typeparam name="T">WebSocket对象类型，必须是WebSocketService兼容类</typeparam>
	public class WebSocketCollection<T> : IEnumerable<T>, IEnumerable where T : WebSocketService
	{
		private ConcurrentDictionary<Guid, T> dictionary = new ConcurrentDictionary<Guid, T>();
		private object broadcastLock = new object();

        /// <summary>
        /// 元素数量
        /// </summary>
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

        /// <summary>
        /// 添加一个WebSocket服务对象
        /// </summary>
        /// <param name="item"></param>
		public void Add(T item)
		{
			this.dictionary[item.Key] = item;
		}

        /// <summary>
        /// 删除WebSocket服务对象
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(T item)
		{
			T t;
			return this.dictionary.TryRemove(item.Key, out t);
		}

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="value">消息内容</param>
        /// <returns></returns>
		public Task Broadcast(string value)
		{
			Message message = ByteStreamMessage.CreateMessage(new ArraySegment<byte>(Encoding.UTF8.GetBytes(value)));
			message.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty
			{
				MessageType = WebSocketMessageType.Text
			};
			return this.Broadcast(message);
		}

        /// <summary>
        /// 广播消息
        /// </summary>
        /// <param name="data">消息内容</param>
        /// <returns></returns>
		public Task Broadcast(byte[] data)
		{
			Message message = ByteStreamMessage.CreateMessage(new ArraySegment<byte>(data));
			message.Properties["WebSocketMessageProperty"] = new WebSocketMessageProperty
			{
				MessageType = WebSocketMessageType.Binary
			};
			return this.Broadcast(message);
		}

        /// <summary>
        /// 执行消息广播服务
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
		private Task Broadcast(Message message)
		{
			Task result;
			lock (this.broadcastLock)
			{
				MessageBuffer messageBuffer = message.CreateBufferedCopy(2147483647);
				List<Task> list = new List<Task>();
				foreach (T current in this.dictionary.Values)
				{
					IChannel channel = (IChannel)current.Callback;
					if (channel.State == CommunicationState.Closed || channel.State == CommunicationState.Faulted)
					{
						this.Remove(current);
					}
					else
					{
						try
						{
							Task item = current.Callback.OnMessage(messageBuffer.CreateMessage());
							list.Add(item);
						}
						catch (Exception)
						{
							this.Remove(current);
						}
					}
				}
				result = Task.WhenAll(list.ToArray()).ContinueWith(delegate(Task t)
				{
					message.Close();
				});
			}
			return result;
		}

        /// <summary>
        /// 获取WebSocket枚举器
        /// </summary>
        /// <returns></returns>
		public IEnumerator<T> GetEnumerator()
		{
			return this.dictionary.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}
	}
}
