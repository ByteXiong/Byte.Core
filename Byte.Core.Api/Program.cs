using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using AspectCore.Extensions.DependencyInjection;
using Autofac;
using Byte.Core.Api.Common;
using Byte.Core.Api.Common.Authorization;
using Byte.Core.Api.Common.Quartz;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.Models;
using Byte.Core.Common.SnowflakeIdHelper;
using Byte.Core.Common.Web;
using Byte.Core.SqlSugar;
using Byte.Core.SqlSugar.ConfigOptions;
using Byte.Core.SqlSugar.IDbContext;
using Byte.Core.Tools;
using Byte.Core.Tools.Extensions;
using Lazy.Captcha.Core;
using Lazy.Captcha.Core.Generator;
using log4net;
using log4net.Repository;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json.Converters;
using Quartz;
using Quartz.Impl;
using SkiaSharp;
using StackExchange.Profiling;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Net.Mime;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);
//雪花Id 
new IdHelperBootstrapper().SetWorkderId(1).Boot();
//Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

//#if DEBUG
builder.WebHost.UseUrls("http://*:3000");
//#endif
#region 获取Config配置
var configuration = builder.Configuration;
#endregion

#region 配置log4net
ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
Log4NetHelper.SetConfig(repository, "log4net.config");
#endregion
#region 配置版本管理
builder.Services.AddApiVersioning(option =>
{
    //版本号以什么形式，什么字段传递
    option.ApiVersionReader = ApiVersionReader.Combine(new HeaderApiVersionReader("api-version"));

    // 在不提供版本号时，默认为1.0  如果不添加此配置，不提供版本号时会报错"message": "An API version is required, but was not specified."
    option.AssumeDefaultVersionWhenUnspecified = true;

    // 可选，为true时API返回支持的版本信息
    option.ReportApiVersions = true;
    // 请求中未指定版本时默认为1.0
    option.DefaultApiVersion = new ApiVersion(0, 0);

    //option.ErrorResponses
    //option.ErrorResponses = new MyErrorResponseProvider();
    //默认以当前最高版本进行访问
    //option.ApiVersionSelector = new CurrentImplementationApiVersionSelector(option);
})
    .AddApiExplorer(opt =>
    {
        //以通知swagger替换控制器路由中的版本并配置api版本
        opt.SubstituteApiVersionInUrl = true;
        // 版本名的格式：v+版本号
        opt.GroupNameFormat = "'v'VVV";
        //是否提供API版本服务
        opt.AssumeDefaultVersionWhenUnspecified = true;

    });

#endregion


#region Swagger ui
string swggerTitle = configuration["SwggerTitle"];
builder.Services.AddSwaggerGen();
//解决上面报ASP0000警告的方案
builder.Services.AddOptions<SwaggerGenOptions>()
        .Configure<IApiVersionDescriptionProvider>((options, service) =>
        {
            options.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            // 添加文档信息
            foreach (var item in service.ApiVersionDescriptions)
            {

                options.SwaggerDoc(item.GroupName, CreateInfoForApiVersion(item));
            }
            OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
            {
                var info = new OpenApiInfo()
                {
                    //标题
                    Title = $"{swggerTitle} {EnumHelper.GetEnumDescription((VersionEnum)description.ApiVersion.MajorVersion)}",
                    //当前版本
                    Version = description.ApiVersion.ToString(),
                    //文档说明
                    Description = @"",

                    ////联系方式
                    //Contact = new OpenApiContact() { Name = "标题", Email = "", Url = null },
                    ////许可证
                    //License = new OpenApiLicense() { Name = "文档", Url = new Uri("") }
                };
                //当有弃用标记时的提示信息
                if (description.IsDeprecated)
                {
                    info.Description += " - 此版本已放弃兼容";
                }
                return info;
            }
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {

                                {
                                    new OpenApiSecurityScheme
                                    {
                                        Reference = new OpenApiReference
                                        {
                                            Type = ReferenceType.SecurityScheme,
                                            Id = "Bearer"
                                        }
                                    },
                                    new string[] { }
                                }
                });
            // JWT认证                                                 
            options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Scheme = "bearer",
                BearerFormat = "JWT",
                Type = SecuritySchemeType.Http,
                Name = "Authorization",
                In = ParameterLocation.Header,
                Description = "Authorization:Bearer {your JWT token}<br/><b>授权地址:/Test/Login</b>",
            });



            //给swagger添加过滤器
            //options.OperationFilter<SwaggerParameterFilter>();
            // 加载XML注释
            // 为 Swagger JSON and UI设置xml文档注释路径
            //获取应用程序所在目录（绝对，不受工作目录影响，建议采用此方法获取路径）
            var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            var xmls = Directory.GetFiles(basePath, "*.xml");
            Array.ForEach(xmls, aXml =>
            {
                options.IncludeXmlComments(aXml, true);
            });
            options.OrderActionsBy(o => o.RelativePath);
        });
#endregion
#region 图像验证码
builder.Services.AddDistributedMemoryCache().AddCaptcha(
    option =>
    {
        option.CaptchaType = CaptchaType.ARITHMETIC; // 验证码类型
        option.CodeLength = 1; // 验证码长度, 要放在CaptchaType设置后
        option.ExpirySeconds = 120; // 验证码过期时间
        option.IgnoreCase = true; // 比较时是否忽略大小写
        option.ImageOption.Animation = false; // 是否启用动画

        option.ImageOption.Width = 130; // 验证码宽度
        option.ImageOption.Height = 48; // 验证码高度
        option.ImageOption.BackgroundColor = SKColors.White; // 验证码背景色

        option.ImageOption.BubbleCount = 2; // 气泡数量
        option.ImageOption.BubbleMinRadius = 5; // 气泡最小半径
        option.ImageOption.BubbleMaxRadius = 15; // 气泡最大半径
        option.ImageOption.BubbleThickness = 1; // 气泡边沿厚度

        option.ImageOption.InterferenceLineCount = 2; // 干扰线数量

        option.ImageOption.FontSize = 35; // 字体大小
        option.ImageOption.FontFamily = DefaultFontFamilys.Instance.Actionj; // 字体，中文使用kaiti，其他字符可根据喜好设置（可能部分转字符会出现绘制不出的情况）。
    }
    );
#endregion

builder.Services.AddSingleton<Byte.Core.Api.Common.WebSocketManager>();


builder.Services.AddOptions();
builder.Services.Configure<FormOptions>(options =>
{
    options.MultipartBodyLengthLimit = long.MaxValue; // In case of multipart
});
#region Redis
var redisConnectionString = configuration["Redis"];
//启用Redis
builder.Services.UseCsRedisClient(redisConnectionString);
////全局设置Redis缓存有效时间为5分钟。
builder.Services.Configure<DistributedCacheEntryOptions>(option =>
option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5));
#endregion 
#region Session
builder.Services.AddSession(options =>
{
    options.Cookie.Name = "MyApp.Session";
    options.IdleTimeout = TimeSpan.FromMinutes(5);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion
#region MemoryCache

//启用MemoryCache
builder.Services.AddMemoryCache();
//.AddScoped<IDistributedCacheManager, DistributedCacheManager>();// 添加后单机IMemoryCache会报错，暂时不启用
#endregion

#region 配置数据库
builder.Services.Configure<Configs>(configuration)
.AddScoped<IUnitOfWork, UnitOfWork>()
.AddScoped<SugarDbContext>();
builder.Services.AddSqlSugarSetup(configuration);
#endregion
#region 任务调度
IScheduler _scheduler = null;
builder.Services.AddSingleton<IScheduler>(provider =>
{
    StdSchedulerFactory factory = new StdSchedulerFactory();
    _scheduler = factory.GetScheduler().Result; // use Wait to block the execution
    return _scheduler;
});


#region 各种 注入
builder.Services
 .AddAutoServices("Byte.Core.Business")
  .AddAutoServices("Byte.Core.Repository")
//.AddScopedAssembly("Byte.Core.DataAccess", "Byte.Core.DataAccess")//注入仓储
.AddSingleton<IHttpContextAccessor, HttpContextAccessor>()
.AddSingleton<WebSocketServer>();
#endregion

#region MiniProfiler性能分析
builder.Services.AddMiniProfiler(options =>
{
    options.RouteBasePath = "/profiler";
    //(options.Storage as MemoryCacheStorage).CacheDuration = TimeSpan.FromMinutes(10);
    options.PopupRenderPosition = RenderPosition.Left;
    options.PopupShowTimeWithChildren = true;

    // 可以增加权限
    //options.ResultsAuthorize = request => request.HttpContext.User.IsInRole("Admin");
    //options.UserIdProvider = request => request.HttpContext.User.Identity.Name;
}
);
#endregion
builder.Services.Configure<KestrelServerOptions>(options =>
{
    options.AllowSynchronousIO = true;
});


//builder.Services.AddHttpContextAccessor();
builder.Services.AddControllersWithViews();
//builder.Services.AddMvc(option =>
//{
//    option.Filters.Add<ValidFilterAttribute>();
//    //  option.Filters.Add(new GlobalExceptionFilter());
//});

//builder.Services.AddTransient<RedisInterceptorAttribute>(provider =>
//    new RedisInterceptorAttribute("YourStringValue"));
#region 过滤器
builder.Services.AddControllers(options =>
{

    options.Filters.Add<ApiLogAttribute>();
    //options.Filters.Add<ValidFilterAttribute>();
    //全局异常过滤器
    options.Filters.Add<GlobalExceptionFilter>();

})
    .ConfigureApiBehaviorOptions(options =>
    {  // 规定模型校验错误时返回统一的结构
        options.InvalidModelStateResponseFactory = context =>
        {


            //获取验证失败的模型字段 
            var errors = context.ModelState
        .Where(e1 => e1.Value.Errors.Count > 0)
        .Select(e1 => e1.Key)
        .ToList();
            var str = string.Join(">", errors) ?? errors.ToJson();

            // 创建项目定义的统一的返回结构ApiResult
            var apiResult = ExcutedResult.FailedResult(str);
            // 向apiResult记录参数校验错误
            // ...
            // 参数校验错误详情可在context.ModelState中获取
            // 统一返回400的Json  
            var result = new BadRequestObjectResult(apiResult);
            result.ContentTypes.Add(MediaTypeNames.Application.Json);
            return result;
        };
    })
    .AddJsonOptions(options =>
    {
        //你可以通过配置序列化行为来处理导航属性的循环引用
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve;

        //    //格式化日期时间格式，需要自己创建指定的转换类DatetimeJsonConverter
        //    options.JsonSerializerOptions.Converters.Add(new DatetimeJsonConverter());
        //数据格式首字母小写
        options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        //    //JsonNamingPolicy.CamelCase首字母小写（默认）,null则为不改变大小写
        //   // options.JsonSerializerOptions.PropertyNamingPolicy = null;
        //    //取消Unicode编码
        //    options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);
        //    //忽略空值
        //options.JsonSerializerOptions.IgnoreNullValues = true;
        //    //允许额外符号
        //    options.JsonSerializerOptions.AllowTrailingCommas = true;
        //    //反序列化过程中属性名称是否使用不区分大小写的比较
        //    options.JsonSerializerOptions.PropertyNameCaseInsensitive = false;
    })
.AddNewtonsoftJson(options =>
{
    ////修改属性名称的序列化方式，首字母小写，即驼峰样式 处理null =""
    options.SerializerSettings.ContractResolver = new NullToEmptyStringToCamelCaseResolver();
    ////修改属性名称的序列化方式，首字母小写，即驼峰样式
    //options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
    //日期类型默认格式化处理 方式1
    options.SerializerSettings.Converters.Add(new IsoDateTimeConverter() { DateTimeFormat = "yyyy-MM-dd HH:mm:ss" });

    //////日期类型默认格式化处理 方式2
    ////options.SerializerSettings.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
    ////options.SerializerSettings.DateFormatString = "yyyy/MM/dd HH:mm:ss";

    //忽略循环引用
    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

    ////解决命名不一致问题 
    //options.SerializerSettings.ContractResolver = new DefaultContractResolver();

    ////空值处理
    //options.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore;

});

#endregion

//builder.Register(c => new CallLogger(Console.Out))
//       .Named<IInterceptor>("log-calls");
//builder.Services.ConfigureDynamicProxy();
builder.Services.BuildAspectCoreWithAutofacServiceProvider(config =>
{
    //config.Interceptors.AddTyped<RedisInterceptorAttribute>();
    //config.Interceptors.AddServiced<RedisInterceptorAttribute>();
    //config.Interceptors.AddDelegate(async (content, next) =>
    //{
    //    Console.WriteLine("delegate interceptor"); await content.Invoke(next);
    //});
    //config.Interceptors.AddTyped<RedisInterceptorAttribute>(method => method.DeclaringType.Name.EndsWith("GetPageAsync"));
    //config.Interceptors.AddTyped<RedisInterceptorAttribute>();
}); //接入AspectCore.Injector 属性注入




//Console.WriteLine("Quartz:定时器启动" + DateTime.Now.ToString());


//builder.Services.AddSingleton<IJobFactory, JobFactory>();
//builder.Services.AddSingleton<ISchedulerFactory, StdSchedulerFactory>();
//builder.Services.AddHostedService<QuartzHostedService>();

//builder.Services.AddSingleton<HelloQuartzJob>();
//builder.Services.AddSingleton(new JobSchedule(jobType: typeof(HelloQuartzJob), cronExpression: "0/5 * * * * ?"));
#endregion


builder.Host.UseServiceProviderFactory(new DynamicProxyServiceProviderFactory());

//builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => { 
//                containerBuilder.RegisterType<RedisInterceptorAttribute>();
//                containerBuilder.RegisterType<RedisDemoLogic>()
//               .EnableClassInterceptors()
//               .InterceptedBy(typeof(RedisInterceptorAttribute));

//});

//#region 添加微信配置（一行代码）
////Senparc.Weixin 注册（必须）
//builder.Services.AddSenparcWeixinServices(builder.Configuration);
//#endregion
var app = builder.Build();


#region 跨域
app.UseMiddleware<CorsMiddleware>();
#endregion



//app.UseDeveloperExceptionPage();

//检测https 转发
app.UseHttpsRedirection();

#region 性能监控 /profiler/results-index
app.UseMiniProfiler();
#endregion

#region WebSocket
//app.UseWebSockets(new WebSocketOptions
//{
//    KeepAliveInterval = TimeSpan.FromMinutes(2)
//});
//app.UseMiddleware<WebsocketHandlerMiddleware>();
#endregion

app.UseRouting();
app.UseSession();
//身份验证
//这里注意 一定要在 UseMvc前面，顺序不可改变
//app.UseMvc被app.UseRouting和app.UseEndpoints替代，所以app.UseAuthentication和app.UseAuthorization，要放在app.UseRouting、app.UseCors之后，并且在app.UseEndpoints之前
app.UseAuthentication();
app.UseAuthorization();
#region Swagger
app.UseSwagger(opt =>
{
    //路由模板，默认值是/swagger/{documentName}/swagger.json，这个属性很重要！而且这个属性中必须包含{documentName}参数。
    //opt.RouteTemplate= "/swagger/{documentName}/swagger.json";
    // 表示按Swagger2.0格式序列化生成swagger.json，这个不推荐使用，尽可能的使用新版本的就可以了
    //opt.SerializeAsV2
});
//添加SwaggerUI中间件，主要用于拦截swagger / index.html页面请求，返回页面给前端

app.UseSwaggerUI(options =>
{
    var provider = builder.Services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>();
    // 为每个版本创建一个JSON
    foreach (var description in provider.ApiVersionDescriptions)
    {
        //这个属性是往SwaggerUI页面head标签中添加我们自己的代码，比如引入一些样式文件，或者执行自己的一些脚本代码
        //options.HeadContent += $"<script type='text/javascript'>alert('欢迎来到SwaggerUI页面')</script>";

        //展示默认头部显示的下拉版本信息
        //options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
        //自由指定头部显示的下拉版本内容
        options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", EnumHelper.GetEnumDescription((VersionEnum)description.ApiVersion.MajorVersion));
        //options.IndexStream = () => GetType().GetTypeInfo().Assembly.GetManifestResourceStream("MiniProfilerSample.index.html");
        //如果是为空 访问路径就为 根域名/index.html,注意localhost:8001/swagger是访问不到的
        //options.RoutePrefix = string.Empty;
        // 如果你想换一个路径，直接写名字即可，比如直接写c.RoutePrefix = "swagger"; 则访问路径为 根域名/swagger/index.html

        //{
        //    url = "/swagger/logo.png" // 添加 logo
        //};
    }
    options.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
    //options.ConfigObject.AdditionalItems["logo"] = new
    //{
    //    url = "/swagger/logo.png" // 添加 logo
    //};
    //options.RoutePrefix = "Byte.CoreApi";
    //options.InjectStylesheet("/swagger/custom.css");
    //options.InjectJavascript("/swagger/custom.js");
});

#endregion

app.UseEndpoints(endpoints =>
{
    endpoints.MapDefaultControllerRoute();
});

#region 启用微信配置（一句代码）

//手动获取配置信息可使用以下方法
//var senparcWeixinSetting = app.Services.GetService<IOptions<SenparcWeixinSetting>>()!.Value;

//启用微信配置（必须）
//var registerService = app.UseSenparcWeixin(app.Environment,
//    null /* 不为 null 则覆盖 appsettings  中的 SenpacSetting 配置*/,
//    null /* 不为 null 则覆盖 appsettings  中的 SenpacWeixinSetting 配置*/,
//    register => { /* CO2NET 全局配置 */ },
//    (register, weixinSetting) =>
//    {
//        //注册公众号信息（可以执行多次，注册多个公众号）
//        register.RegisterMpUserName(weixinSetting, "【盛派网络小助手】公众号");
//    });
#endregion
#region 使用 MessageHadler 中间件，用于取代创建独立的 Controller

////MessageHandler 中间件介绍：https://www.cnblogs.com/szw/p/Wechat-MessageHandler-Middleware.html
////使用公众号的 MessageHandler 中间件（不再需要创建 Controller）
//app.UseMessageHandlerForMp("/WeixinAsync", CustomMessageHandler.GenerateMessageHandler, options =>
//{
//    //获取默认微信配置
//    var weixinSetting = Senparc.Weixin.Config.SenparcWeixinSetting;

//    //[必须] 设置微信配置
//    options.UserNameSettingFunc = context => weixinSetting;

//    //[可选] 设置最大文本长度回复限制（超长后会调用客服接口分批次回复）
//    options.TextResponseLimitOptions = new TextResponseLimitOptions(2048, weixinSetting.WeixinAppId);
//});

#endregion




#region wwwroot目录开启和虚拟目录设置
app.UseDefaultFiles(); // 添加默认文件中间件
//if (!Directory.Exists(baseDirectory + "/UploadFiles"))
//{
//    Directory.CreateDirectory(baseDirectory + "/UploadFiles");
//}
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
    DefaultContentType = "application/json;charset=UTF-8"
});
//app.UseStaticFiles(new StaticFileOptions
//{
//    FileProvider = new PhysicalFileProvider(baseDirectory + "/UploadFiles"),
//    RequestPath = "/UploadFiles"
//});
#endregion
//#region 初始数据库
//await DataSeederMiddleware.Init("Byte.Core.Entity");
//#endregion
//#region  任务调度
//await QuartzJobMiddleware.Init();
//#endregion

// 在 app.Run() 之后执行一个方法
#region 初始数据库
await app.UseDataSeederMiddlewareAsync("Byte.Core.Entity");
#endregion
//app.Services.GetService<IHostApplicationLifetime>().ApplicationStarted.Register(async () =>
//{

//    #region  任务调度
//    await QuartzJobMiddleware.Init(_scheduler);
//    #endregion
//});
#region  任务调度
await app.QuartzJobInit();
#endregion
app.Run();





