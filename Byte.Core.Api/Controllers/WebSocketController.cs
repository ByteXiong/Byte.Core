using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// WebSocket
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class WebSocketController : BaseApiController
    {
        [HttpGet]
        [NoCheckJWT]
        public async Task GetNotReadCountAsync()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await EchoAsync(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
             async Task EchoAsync(WebSocket webSocket)
            {
                var buffer = new byte[1024 * 4];
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {
                    var serverMsg = Encoding.UTF8.GetBytes($"服务端返回: {Encoding.UTF8.GetString(buffer)}");
                    await webSocket.SendAsync(new ArraySegment<byte>(serverMsg, 0, serverMsg.Length), result.MessageType, result.EndOfMessage, CancellationToken.None);
                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        }

     
    }
}
