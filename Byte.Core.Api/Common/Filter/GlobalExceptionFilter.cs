using Byte.Core.Business;
using Byte.Core.Common;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.SnowflakeIdHelper;
using Byte.Core.Entity;
using Byte.Core.Tools;
using IP2Region.Net.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Shyjus.BrowserDetection;
using StackExchange.Profiling;
using System.ComponentModel;
using LogLevel = Byte.Core.Tools.LogLevel;
using MiniProfiler = StackExchange.Profiling.MiniProfiler;

namespace Ape.Volo.Api.Filter;

public class GlobalExceptionFilter : IAsyncExceptionFilter
{
    private readonly ExceptionLogLogic _exceptionLogLogic;
    private readonly SettingLogic _settingLogic;
    private readonly IBrowserDetector _browserDetector;
    private readonly ILogger<GlobalExceptionFilter> _logger;
    private readonly ISearcher _ipSearcher;

    public GlobalExceptionFilter(ExceptionLogLogic exceptionLogLogic, ISearcher searcher,
        SettingLogic settingLogic, IBrowserDetector browserDetector,
        ILogger<GlobalExceptionFilter> logger)
    {
        _exceptionLogLogic = exceptionLogLogic;
        _settingLogic = settingLogic;
        _browserDetector = browserDetector;
        _logger = logger;
        _ipSearcher = searcher;
    }

    public async Task OnExceptionAsync(ExceptionContext context)
    {
        var exceptionType = context.Exception.GetType();
        var statusCode = StatusCodes.Status500InternalServerError;
        //自定义全局异常
        //if (exceptionType == typeof(ApevovoException))
        //{
        //  var ex = (ApevovoException)context.Exception;
        // statusCode = ex.StatusCode;
        // }
        if (exceptionType == typeof(BusException)) //错误请求 无法处理
        {
            statusCode = StatusCodes.Status400BadRequest;
        }

        var remoteIp = context.HttpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
        var ipAddress = _ipSearcher.Search(remoteIp);
        string throwMsg = context.Exception.Message; //错误信息
        context.Result = new ContentResult
        {
            Content = err
            
            
            //new ActionResultVm
            //{
            //    Status = statusCode,
            //    ActionError = actionError,
            //    Message = throwMsg,
            //    Timestamp = DateTime.Now.ToUnixTimeStamp(),
            //    Path = context.HttpContext.Request.Path.Value?.ToLower()
            //}.ToJson(),
            ContentType = "application/json; charset=utf-8",
            StatusCode = statusCode
        };
        if (App.GetOptions<MiddlewareOptions>().MiniProfiler.Enabled)
        {
            MiniProfiler.Current.CustomTiming("Errors：", throwMsg);
        }

        try
        {
            //记录日志
            _logger.LogError(ExceptionLogFormat.WriteLog(context.HttpContext, remoteIp, ipAddress, context.Exception,
                 CurrentUser.UserName,
                _browserDetector.Browser?.OS, _browserDetector.Browser?.DeviceType, _browserDetector.Browser?.Name,
                _browserDetector.Browser?.Version));
        }
        catch (Exception e)
        {
            //_logger.LogCritical("LogError出错:" + e.ToString());
            FileHelper.WriteLog(ExceptionLogFormat.WriteLog(context.HttpContext, remoteIp, ipAddress, e,
              CurrentUser.UserName,
                _browserDetector.Browser?.OS, _browserDetector.Browser?.DeviceType, _browserDetector.Browser?.Name,
                _browserDetector.Browser?.Version));
        }

        var saveDb = await  _settingLogic.GetSettingValue<bool>("IsExceptionLogSaveDB");
        if (saveDb)
        {
            //记录日志到数据库
            try
            {
                var log = CreateLog(context);
                if (log.IsNotNull())
                {
                    await Task.Factory.StartNew(() => _exceptionLogLogic.AddAsync(log))
                        .ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ExceptionLogFormat.WriteLog(context.HttpContext, remoteIp, ipAddress, ex, App.HttpUser.Account,
                    _browserDetector.Browser?.OS, _browserDetector.Browser?.DeviceType, _browserDetector.Browser?.Name,
                    _browserDetector.Browser?.Version));
            }
        }
    }

    /// <summary>
    /// 创建异常对象
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private ExceptionLog CreateLog(ExceptionContext context)
    {
        ExceptionLog log = null;
        try
        {
            var routeValues = context.ActionDescriptor.RouteValues;
            var httpContext = context.HttpContext;
            var remoteIp = httpContext.Connection.RemoteIpAddress?.ToString() ?? "0.0.0.0";
            var arguments = HttpHelper.GetAllRequestParams(httpContext);
            var descriptionAttribute = ((ControllerActionDescriptor)context.ActionDescriptor).MethodInfo
                .GetCustomAttributes(typeof(DescriptionAttribute), true)
                .OfType<DescriptionAttribute>()
                .FirstOrDefault();
            log = new ExceptionLog
            {
                Id = IdHelper.GetLongId(),
                CreateBy = CurrentUser.UserName,
                CreateTime = DateTime.Now.ToUnixTimeStamp(),
                Area = routeValues["area"],
                Controller = routeValues["controller"],
                Action = routeValues["action"],
                Method = httpContext.Request.Method,
                Description = descriptionAttribute?.Description,
                RequestUrl = httpContext.Request.Path,
                RequestParameters = arguments.ToJson(),
                ExceptionMessage = context.Exception.Message,
                ExceptionMessageFull = ExceptionHelper.GetExceptionAllMsg(context.Exception),
                ExceptionStack = context.Exception.StackTrace,
                RequestIp = remoteIp,
                IpAddress = _ipSearcher.Search(remoteIp),
                LogLevel = LogLevel.Error,
                OperatingSystem = _browserDetector.Browser?.OS,
                DeviceType = _browserDetector.Browser?.DeviceType,
                BrowserName = _browserDetector.Browser?.Name,
                Version = _browserDetector.Browser?.Version
            };
        }
        catch
        {
            // ignored
        }

        return log;
    }
}
