using System;
using System.Linq;
using System.Threading.Tasks;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Repository;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Byte.Core.Api.Attributes
{
    /// <summary>
    /// 权限
    /// </summary>
    public class CheckRoleAttribute : BaseActionFilter
    {

        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            var req = context.HttpContext.Request;
            if (context.ContainsFilter<NoCheckRoleAttribute>()|| context.ContainsFilter<NoCheckJWTAttribute>()|| (req.Method == "GET" && !context.ContainsFilter<GetCheckRoleAttribute>()) || CurrentUser.RoleCodes.Contains(ParamConfig.Admin) || req.Headers["api-version"].ToInt() == (int) VersionEnum.App)
                return;

            try
            {

                var _menuRepository = ServiceLocator.Resolve<MenuRepository>();

                var list=await  _menuRepository.GetPermAsync(CurrentUser.RoleCodes);
                var url = req.Path.ToString().ToLower();
                var any= list.Any(x=>"/api/"+x==req.Path);
                if (!any) { 
                
                    context.Result = Error("权限不足", ParamConfig.ErrorRole);
                }
                
            }
            catch (Exception ex)
            {
                context.Result = Error(ex.Message, ParamConfig.ErrorRole);
            }

            await Task.CompletedTask;
        }
    }
}
