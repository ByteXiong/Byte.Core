using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;


namespace Byte.Core.Common.Filters
{
    /// <summary>
    /// 异常过滤器
    /// </summary>
    public class GlobalExceptionFilter : BaseActionFilter, IAsyncExceptionFilter
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public GlobalExceptionFilter()
        {

        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
          Type type = ((ControllerActionDescriptor)context.ActionDescriptor).ControllerTypeInfo.AsType();
            //Type type = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo.ReflectedType;
            //Exception ex = context.Exception;
            //Type type = new System.Diagnostics.StackTrace(context, true).GetFrame(0).GetMethod().ReflectedType;
            if (context.Exception is BusException busEx)
            {
                type = new System.Diagnostics.StackTrace(busEx, true).GetFrame(0).GetMethod().ReflectedType;

                Log4NetHelper.WriteWarn( type, busEx.Message);
                context.Result = Error(busEx.Message,  busEx.ErrorCode);
            }
            else if (context.Exception is SqlException sqlEx)
            {
                Log4NetHelper.WriteError(type, sqlEx);
                var result = Error(sqlEx.Message, sqlEx.Number);
                result.StatusCode = 500;
                context.Result = result;
                
            }
            //else

            //if (context.Exception is SystemException  systemException)
            //{
            //    Log4NetHelper.WriteError(typeof(GlobalExceptionFilter), systemException);
            //    context.Result = Error(systemException.Message, systemException.HResult);
            //}
            else
            {
                Log4NetHelper.WriteError(type, context.Exception);
                var result = Error(context.Exception.Message, 500);
                result.StatusCode = 500;
                context.Result = result;
            }

            await Task.CompletedTask;
        }
    }
}
