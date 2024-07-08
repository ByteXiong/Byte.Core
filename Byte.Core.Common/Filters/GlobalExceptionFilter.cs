using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Data.SqlClient;


namespace Byte.Core.Common.Filters
{

    public class GlobalExceptionFilter : BaseActionFilter, IAsyncExceptionFilter
    {
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
                context.Result = Error(sqlEx.Message, sqlEx.Number);
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
                context.Result = Error(context.Exception.Message,500);
            }

            await Task.CompletedTask;
        }
    }
}
