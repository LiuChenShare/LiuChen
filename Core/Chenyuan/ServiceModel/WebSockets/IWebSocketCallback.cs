using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// websocket�ص��ӿڶ���
    /// </summary>
    [ServiceContract(CallbackContract = typeof(IWebSocketCallback))]
	public interface IWebSocketCallback
	{
        /// <summary>
        /// ��Ϣ������
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
		[OperationContract(IsOneWay = true, Action = "*")]
		Task OnMessage(Message message);
	}
}
