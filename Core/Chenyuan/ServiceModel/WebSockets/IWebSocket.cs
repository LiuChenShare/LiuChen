using System.ServiceModel;

namespace Chenyuan.ServiceModel.WebSockets
{
    /// <summary>
    /// WebSocke接口定义
    /// </summary>
	[ServiceContract(CallbackContract = typeof(IWebSocketCallback))]
	public interface IWebSocket : IWebSocketCallback
	{
        /// <summary>
        /// WebSocket事件打开处理函数
        /// </summary>
		[OperationContract(Action = "http://schemas.microsoft.com/2011/02/session/onopen", IsOneWay = true, IsInitiating = true)]
		void OnOpen();
	}
}
