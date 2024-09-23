using Microsoft.AspNetCore.Http;

namespace Byte.Core.Common.Web
{
    /// <summary>
    /// 跨域中间件
    /// </summary>
    public class CorsMiddleware
    {
        private readonly RequestDelegate _next;

        /// <summary>
        /// 管道执行到该中间件时候下一个中间件的RequestDelegate请求委托，如果有其它参数，也同样通过注入的方式获得
        /// </summary>
        /// <param name="next">下一个处理者</param>
        public CorsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        /// 自定义中间件要执行的逻辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context)
        {
            if (!context.Response.HasStarted)
            {
             context.Response.Headers.Add("Access-Control-Allow-Credentials", "true");
            context.Response.Headers.Add("Access-Control-Allow-Origin", context.Request.Headers["origin"]);
            //context.Response.Headers.Add("Access-Control-Allow-Origin", "*");
            context.Response.Headers.Add("Access-Control-Allow-Headers", "Origin,X-Requested-With,Content-Type,Accept,WG-App-Version,WG-Device-Id,WG-Network-Type,WG-Vendor,WG-OS-Type,WG-OS-Version,WG-Device-Model,WG-CPU,WG-Sid,WG-App-Id,WG-Token,Authorization,api-version");
            context.Response.Headers.Add("Access-Control-Allow-Methods", "GET,POST,PUT,DELETE,OPTIONS");
            }
            //若为OPTIONS跨域请求则直接返回,不进入后续管道
            if (context.Request.Method.ToUpper() != "OPTIONS")
                await _next(context);//把context传进去执行下一个中间件
        }
    }
}
