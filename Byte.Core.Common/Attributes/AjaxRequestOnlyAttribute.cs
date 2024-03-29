using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace Byte.Core.Common.Attributes
{
    public class AjaxRequestOnlyAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.IsAjaxRequest())
            {
                context.Result = new JsonResult(new { success = false, msg = "This method only allows Ajax requests." });
                context.HttpContext.Response.StatusCode = HttpStatusCode.Forbidden.GetHashCode();
            }
            base.OnActionExecuting(context);
        }

        public void OnException(ExceptionContext context)
        {
            var type = System.Reflection.MethodBase.GetCurrentMethod().DeclaringType;
            Log4NetHelper.WriteError(type, context.Exception);

            context.Result = new JsonResult(new { success = false, msg = context.Exception.Message });
            context.HttpContext.Response.StatusCode = HttpStatusCode.InternalServerError.GetHashCode();
            context.ExceptionHandled = true;
        }
    }
}
