using Byte.Core.Common.Helpers;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace Byte.Core.Common.Middlewares
{
    public class ResponseTimeMiddleware
    {
        // Handle to the next Middleware in the pipeline  
        private readonly RequestDelegate _next;
        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public Task InvokeAsync(HttpContext context)
        {
            // Start the Timer using Stopwatch  
            var watch = new Stopwatch();
            watch.Start();
            context.Response.OnStarting(() =>
            {
                // Stop the timer information and calculate the time   
                watch.Stop();
                var responseTimeForCompleteRequest = watch.ElapsedMilliseconds;
                // Add the Response time information in the Response headers.   
                Log4NetHelper.WriteInfo(GetType(), $"\t{context.Request.Path.ToString()}\t{responseTimeForCompleteRequest}");
                return Task.CompletedTask;
            });
            // Call the next delegate/middleware in the pipeline   
            return this._next(context);
        }
    }
}
