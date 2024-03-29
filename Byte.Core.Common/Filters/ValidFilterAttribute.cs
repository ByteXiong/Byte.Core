using Microsoft.AspNetCore.Mvc.Filters;

namespace Byte.Core.Common.Filters
{
    public class ValidFilterAttribute : ActionFilterAttribute, IActionFilter
    {

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            Console.WriteLine("捕捉");
            if (!context.ModelState.IsValid)
            {
                var msgList = context.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);

                //    context.Result = Error(string.Join(",", msgList));
            }

            //    await Task.CompletedTask;
        }
    }
}
