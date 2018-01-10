using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// websocket回调接口定义
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IWebSocketCallback))]
	public interface IWebSocketCallback
	{
        /// <summary>
        /// 消息处理函数
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
		[OperationContract(IsOneWay = true, Action = "*")]
		Task OnMessage(Message message);
	}
}
