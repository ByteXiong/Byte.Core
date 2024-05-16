using AspectCore.DynamicProxy;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Byte.Core.Common.Filters
{

    public class GlobalExceptionFilter : BaseActionFilter, IAsyncExceptionFilter
    {
        public GlobalExceptionFilter()
        {

        }

        public async Task OnExceptionAsync(ExceptionContext context)
        {
            Exception ex = context.Exception;
            if (ex is AspectInvocationException aspectEx)
                ex = aspectEx.InnerException;

            if (ex is BusException busEx)
            {

                Log4NetHelper.WriteWarn(typeof(GlobalExceptionFilter), busEx.Message);
                context.Result = Error(busEx.Message, busEx.ErrorCode);
            }
            else
            {
                Log4NetHelper.WriteError(typeof(GlobalExceptionFilter), ex);
                context.Result = Error(ex.Message);
            }

            await Task.CompletedTask;
        }
    }
}
