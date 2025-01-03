﻿using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.Web;
using Microsoft.AspNetCore.Mvc.Filters;
using Byte.Core.Common.Attributes;
namespace Byte.Core.Tools.Attributes
{
    /// <summary>
    /// JWT校验
    /// </summary>
    public class CheckJWTAttribute : BaseActionFilter
    {
        private static readonly int _errorCode = AppConfig.ErrorJWT;

        public override async Task OnActionExecuting(ActionExecutingContext context)
        {
            
            if (context.ContainsFilter<NoCheckJWTAttribute>())
                return;

            try
            {
                var req = context.HttpContext.Request;

                string token = req.GetToken();
                if (token.IsNullOrEmpty())
                {
                    context.Result = Error("缺少token", _errorCode);
                    return;
                }

                if (!JWTHelper.CheckToken(token, JWTHelper.JWTSecret))
                {
                    context.Result = Error("token校验失败!", _errorCode);
                    return;
                }

                var payload = JWTHelper.GetPayload<JWTPayload>(token);
                if (payload.Expire < DateTime.Now)
                {
                    context.Result = Error("token过期!", _errorCode);
                    return;
                }

                CurrentUser.Configure(payload);
            }
            catch (Exception ex)
            {
                context.Result = Error(ex.Message, _errorCode);
            }

            await Task.CompletedTask;
        }
    }
}