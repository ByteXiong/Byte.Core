namespace Byte.Core.Api.Common
{
    public class WebsocketHandlerMiddleware
    {
        private readonly RequestDelegate _next;

        public WebsocketHandlerMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.Request.Path == "/ws")
            {
                //客户端与服务器成功建立连接后，服务器会循环异步接收客户端发送的消息，收到消息后就会执行Handle(WebsocketClient websocketClient)中的do{}while;直到客户端断开连接
                //不同的客户端向服务器发送消息后台执行do{}while;时，websocketClient实参是不同的，它与客户端一一对应
                //同一个客户端向服务器多次发送消息后台执行do{}while;时，websocketClient实参是相同的
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var webSocket = await context.WebSockets.AcceptWebSocketAsync();

                }
                else
                {
                    context.Response.StatusCode = 404;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
