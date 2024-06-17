using Byte.Core.Common.Attributes;
using Byte.Core.Common.Helpers;
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
            //Exception ex = context.Exception;

            if (context.Exception is BusException busEx)
            {

                Log4NetHelper.WriteWarn( typeof(GlobalExceptionFilter), busEx);
                context.Result = Error(busEx.Message, busEx.ErrorCode);
            }
            else if (context.Exception is SqlException sqlEx)
            {
                Log4NetHelper.WriteError(typeof(GlobalExceptionFilter), sqlEx);
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
                Log4NetHelper.WriteError(typeof(GlobalExceptionFilter), context.Exception);
                context.Result = Error(context.Exception.Message,500);
            }

            await Task.CompletedTask;
        }
    }
}
