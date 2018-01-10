using System.ServiceModel;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// WebSocke�ӿڶ���
    /// </summary>
	[ServiceContract(CallbackContract = typeof(IWebSocketCallback))]
	public interface IWebSocket : IWebSocketCallback
	{
        /// <summary>
        /// WebSocket�¼��򿪴�����
        /// </summary>
		[OperationContract(Action = "http://schemas.microsoft.com/2011/02/session/onopen", IsOneWay = true, IsInitiating = true)]
		void OnOpen();
	}
}
