using Asp.Versioning;
using Azure;
using Byte.Core.Api.Common;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
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
    [NoFormatResponse]
    [NoCheckJWT]
    public class WebSocketController : BaseApiController
    {
        private static ConcurrentDictionary<string, System.Net.WebSockets.WebSocket> _sockets = new ConcurrentDictionary<string, System.Net.WebSockets.WebSocket>();
        [HttpGet]
        public async Task ConnectAsync(string socketId)

        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {

             
                    using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                     //取消线程
                    CancellationToken ct = HttpContext.RequestAborted;
                //try
                //{
               

                    ////识别用户
                    //string receiveId = HttpContext.Request.Query["rid"].ToString();
                    //判断用户识别(不存在就添加)
                    if (!_sockets.ContainsKey(socketId))
                    {
                        Console.WriteLine($"有人已连接:{socketId}");
                        _sockets.TryAdd(socketId, webSocket);
                    }
                //var buffer = new byte[1024 * 4];
                //WebSocketReceiveResult result    = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), ct);
                //是否关闭
                while (webSocket.State == WebSocketState.Open || webSocket.State == WebSocketState.CloseSent)
                {
               
                    if (ct.IsCancellationRequested)
                    {
                            break;
                    }
                    //数据缓冲
                    WebSocketMsg response = await ReceiveStringAsync(webSocket, ct);
                        //转对象
                        //  MsgTemplate msg = JsonConvert.DeserializeObject<MsgTemplate>(response);
                        //判断数据状态
                        if (response == null)
                        {
                            continue;

                        }

                        Console.WriteLine($"接收消息{response}在线人数:{_sockets.Count} ");
                  
                        switch (response.Type)
                        {
                            case Tools.WebSocketMsgTypeEnum.发送心跳:
                            case Tools.WebSocketMsgTypeEnum.在线用户:
                                var msgData = new WebSocketMsg()
                                {
                                    Type = Tools.WebSocketMsgTypeEnum.在线用户,
                                    Data = _sockets.Select(x => x.Key).ToList()
                                };
                                await SendStringAsync(webSocket, msgData, ct);
                                break;
                            case Tools.WebSocketMsgTypeEnum.单聊:
                                await SendStringAsync(webSocket, response, ct);
                                break;
                            case Tools.WebSocketMsgTypeEnum.群聊:
                                break;
                            default:
                                break;
                        }

                    }
                //}
                //finally {
               
                var any=  _sockets.TryRemove(socketId, out _);
                Console.WriteLine($"在线人数:{_sockets.Count}{any}");
                //服务端关闭
                if (webSocket.State != WebSocketState.Closed)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.InternalServerError, "Error occurred", CancellationToken.None);
                }
                webSocket.Dispose();
                //}
            }
            else
            {
                HttpContext.Response.StatusCode = 400;
            }
        }


        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="data"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static Task SendStringAsync(System.Net.WebSockets.WebSocket socket, WebSocketMsg msg, CancellationToken ct = default(CancellationToken))
        {
            var str = msg.ToJson();
            Console.WriteLine(str);
            var buffer = Encoding.UTF8.GetBytes(str);
            var segment = new ArraySegment<byte>(buffer);
            return socket.SendAsync(segment, WebSocketMessageType.Text, true, ct);
        }
        /// <summary>
        /// 数据缓冲
        /// </summary>
        /// <param name="socket"></param>
        /// <param name="result"></param>
        /// <param name="ct"></param>
        /// <returns></returns>
        private static async Task<WebSocketMsg> ReceiveStringAsync(System.Net.WebSockets.WebSocket webSocket, CancellationToken ct = default(CancellationToken))
        {
            var buffer = new ArraySegment<byte>(new byte[1024 * 4]);
            using (var ms = new MemoryStream())
            {
                WebSocketReceiveResult result;
                do
                {
                    ct.ThrowIfCancellationRequested();

                    result = await webSocket.ReceiveAsync(buffer, ct);
                    ms.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);
             
           
                if (result.MessageType != WebSocketMessageType.Text)
                {
                    return null;
                }
                //客户端发起关闭请求
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    Console.WriteLine("客户端发起关闭请求");
                    // 发送关闭确认帧
                    await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, ct);
                    return null;
                }
                ms.Seek(0, SeekOrigin.Begin);

                try
                {
                    using (var reader = new StreamReader(ms, Encoding.UTF8))
                    {

                        var read = await reader.ReadToEndAsync();
                        Console.WriteLine("打印"+ read);
                        return read.ToObject<WebSocketMsg>();
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine("解析异常");
                    return null;
                }
            
            }
        }

    }
}
