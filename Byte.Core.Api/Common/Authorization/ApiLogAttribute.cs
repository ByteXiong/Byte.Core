﻿using Byte.Core.Api.Attributes;
using Byte.Core.Api.Controllers;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.Tools;
using Exceptionless.Models.Data;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using NPOI.XWPF.UserModel;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Byte.Core.Api.Common.Authorization
{
    public class ApiLogAttribute : BaseActionFilter
    {

        public override async Task OnActionExecuting(ActionExecutingContext context)
        {

         
            var req = context.HttpContext.Request;


            var xmlCommentHelper = new XmlCommentHelper();
            xmlCommentHelper.LoadAll();

            Assembly assIBll = Assembly.GetExecutingAssembly();
            req.RouteValues.TryGetValue("controller", out var controller);
            Type type = assIBll.GetType($"{assIBll.GetName().Name}.Controllers.{controller}Controller");
            var controllerName =    xmlCommentHelper.GetTypeComment(type);
            req.RouteValues.TryGetValue("action", out var action);
            var method = type.GetMethod(action.ToString()+ "Async");
            var methodName = xmlCommentHelper.GetMethodComment(method);


            StreamReader stream = new StreamReader(context.HttpContext.Request.Body);
            string body = stream.ReadToEnd();
            var _apiLogRepository = ServiceLocator.Resolve<ApiLogRepository>();
            var info = new ApiLog { 
                Url = req.Path,
                Method = req.Method, 
                Ip = req.HttpContext.Connection.RemoteIpAddress.ToString(), 
                Version = (VersionEnum)req.Headers["api-version"].ToInt(),
                Query = req.QueryString.ToString(),
                Body = body,
                Description = controllerName + "." + methodName
            };
            await _apiLogRepository.AddAsync(info);




        }
    }
}