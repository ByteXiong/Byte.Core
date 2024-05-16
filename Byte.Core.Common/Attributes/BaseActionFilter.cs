
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;


namespace Byte.Core.Common.Attributes
{
    public abstract class BaseActionFilter : Attribute, IAsyncActionFilter
    {



        /// <summary>
        /// 在行为方法执行前执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async virtual Task OnActionExecuting(ActionExecutingContext context)
        {


            //Console.WriteLine($"在行为方法执行前执行{DateTime.Now.ToString()}");
            await Task.CompletedTask;
        }
        /// <summary>
        /// 在行为方法执行后执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async virtual Task OnActionExecuted(ActionExecutedContext context)
        {
            //Console.WriteLine($"在行为方法执行后执行{DateTime.Now.ToString()}");
            await Task.CompletedTask;
        }

        /// <summary>
        /// 在行为方法返回前执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async virtual Task OnResultExecuting(ResultExecutingContext context)
        {
            //Console.WriteLine("在行为方法返回前执行");
            await Task.CompletedTask;
        }
        /// <summary>
        /// 在行为方法返回后执行
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public async virtual Task OnResultExecuted(ResultExecutingContext context)
        {
            //Console.WriteLine($"在行为方法返回后执行{DateTime.Now.ToString()}");
            await Task.CompletedTask;
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="next"></param>
        /// <returns></returns>
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await OnActionExecuting(context);
            if (context.Result == null)
            {
                var nextContext = await next();
                await OnActionExecuted(nextContext);
            }
        }

        /// <summary>
        /// 返回JSON
        /// </summary>
        /// <param name="json">json字符串</param>
        /// <returns></returns>
        protected ContentResult JsonContent(string json)
        {
            return new ContentResult { Content = json, StatusCode = 200, ContentType = "application/json; charset=utf-8" };
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <returns></returns>
        protected ContentResult Success()
        {
            var res = ExcutedResult.SuccessResult("请求成功！");

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="msg">消息</param>
        /// <returns></returns>
        protected ContentResult Success(string msg)
        {

            var res = ExcutedResult.SuccessResult(msg);
            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回成功
        /// </summary>
        /// <param name="data">返回的数据</param>
        /// <returns></returns>
        protected ContentResult Success<T>(T data)
        {

            var res = ExcutedResult.SuccessResult(data);
            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <returns></returns>
        protected ContentResult Error()
        {
            var res = ExcutedResult.FailedResult("请求失败！", 001);

            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <returns></returns>
        protected ContentResult Error(string msg)
        {

            var res = ExcutedResult.FailedResult(msg);
            return JsonContent(res.ToJson());
        }

        /// <summary>
        /// 返回错误
        /// </summary>
        /// <param name="msg">错误提示</param>
        /// <param name="errorCode">错误代码</param>
        /// <returns></returns>
        protected ContentResult Error(string msg, int errorCode)
        {
            var res = ExcutedResult.FailedResult(msg, errorCode);
            return JsonContent(res.ToJson());
        }
    }
}
