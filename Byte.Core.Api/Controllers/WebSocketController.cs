using Asp.Versioning;
using Azure;
using Byte.Core.Api.Common;
using Byte.Core.Common.Attributes;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Concurrent;
using System.Net.Sockets;
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
        private static ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();
        [HttpGet]
        [NoCheckJWT]
        public async Task ConnectAsync(string socketId)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                //取消线程
                CancellationToken ct = HttpContext.RequestAborted;

                ////识别用户
                //string receiveId = HttpContext.Request.Query["rid"].ToString();
                //判断用户识别(不存在就添加)
                if (!_sockets.ContainsKey(socketId))
                {
                    Console.WriteLine("有人已连接");
                    _sockets.TryAdd(Guid.NewGuid().ToString(), webSocket);
                }
                while (true)
                {
                    if (ct.IsCancellationRequested)
                    {
                        break;
                    }
                    //数据缓冲
                    string response = await ReceiveStringAsync(webSocket, ct);
                    //转对象
                    //  MsgTemplate msg = JsonConvert.DeserializeObject<MsgTemplate>(response);
                    //判断数据状态
                    if (string.IsNullOrEmpty(response))
                    {
                        if (webSocket.State != WebSocketState.Open)
                        {
                            break;
                        }
                        continue;
                    }

                    foreach (var socket in _sockets)
                    {
                        if (socket.Value.State != WebSocketState.Open)
                        {
                            continue;
                        }
                        //给指定用户发送
                        //if (socket.Key == receiveId)
                        //{
                            await SendStringAsync(socket.Value, response, ct);
                        //}
                    }
                }

                //_sockets.TryRemove(socketId, out dummy);

                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", ct);
                webSocket.Dispose();


            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
            async Task EchoAsync(WebSocket webSocket)
            {
                var buffer = new byte[1024 * 4];
                WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                while (!result.CloseStatus.HasValue)
                {

                    await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);

                    result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                }
                await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static Task SendStringAsync(System.Net.WebSockets.WebSocket socket, string data, CancellationToken ct = default(CancellationToken))
        {
            var buffer = Encoding.UTF8.GetBytes(data);
            var segment = new ArraySegment<byte>(buffer);

            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
        /// <summary>
        /// 数据缓冲
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static async Task<string> ReceiveStringAsync(System.Net.WebSockets.WebSocket socket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await socket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                ms.Seek(0, SeekOrigin.Begin);
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }

                using (var reader = new StreamReader(ms, Encoding.UTF8))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }

    }
}
