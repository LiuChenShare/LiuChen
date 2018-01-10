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
    /// WebSocket�����ඨ��
    /// </summary>
    /// <typeparam name="T">WebSocket�������ͣ�������WebSocketService������</typeparam>
	public class WebSocketCollection<T> : IEnumerable<T>, IEnumerable where T : WebSocketService
	{
		private ConcurrentDictionary<Guid, T> dictionary = new ConcurrentDictionary<Guid, T>();
		private object broadcastLock = new object();

        /// <summary>
        /// Ԫ������
        /// </summary>
		public int Count
		{
			get
			{
				return this.dictionary.Count;
			}
		}

        /// <summary>
        /// ���һ��WebSocket�������
        /// </summary>
        /// <param name="item"></param>
		public void Add(T item)
		{
			this.dictionary[item.Key] = item;
		}

        /// <summary>
        /// ɾ��WebSocket�������
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
		public bool Remove(T item)
		{
			T t;
			return this.dictionary.TryRemove(item.Key, out t);
		}

        /// <summary>
        /// �㲥��Ϣ
        /// </summary>
        /// <param name="value">��Ϣ����</param>
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
        /// �㲥��Ϣ
        /// </summary>
        /// <param name="data">��Ϣ����</param>
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
        /// ִ����Ϣ�㲥����
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
        /// ��ȡWebSocketö����
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
