using Byte.Core.Business;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.IoC;
using Byte.Core.SqlSugar;
using Byte.Core.SqlSugar.ConfigOptions;
using Byte.Core.SqlSugar.IDbContext;
using Byte.Core.Tools.Extensions;
using log4net.Repository;
using log4net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Session;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Byte.Core.MSTest
{
    public class Program
    {

        public static void Setup()
        {   //雪花Id 
            //new IdHelperBootstrapper().SetWorkderId(1).Boot();
            //Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var configuration = new ConfigurationBuilder()
                           .AddInMemoryCollection() //将配置文件的数据加载到内存中
                                                    //.SetBasePath(Directory.GetCurrentDirectory()) //指定配置文件所在的目录
                           .AddJsonFile("appsettings.Development.json", optional: true, reloadOnChange: true) //指定加载的配置文件  --划重点..记得始终复制
                           .Build(); //编译成对象  
            #region 配置log4net
            ILoggerRepository repository = LogManager.CreateRepository("NETCoreRepository");
            Log4NetHelper.SetConfig(repository, "log4net.config");
            #endregion

            IServiceCollection services = new ServiceCollection();

            #region 配置数据库
            //var configs = configuration.Get<Configs>();
            services.AddSqlSugarSetup(configuration);
            services.Configure<Configs>(configuration)
            .AddScoped<SugarDbContext>()
            .AddScoped<IUnitOfWork, UnitOfWork>();
            #endregion




            #region 各种注入
            services
            .AddAutoServices("Byte.Core.Business")
            .AddAutoServices("Byte.Core.Repository")
            .AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.AddOptions();

            #endregion


            //#region 添加微信配置（一行代码）
            ////Senparc.Weixin 注册（必须）
            //services.AddSenparcWeixinServices(configuration);
            //#endregion


            #region Redis

            var redisConnectionString = configuration["Redis"];
            //启用Redis
            services.UseCsRedisClient(redisConnectionString);
            ////全局设置Redis缓存有效时间为5分钟。
            services.Configure<DistributedCacheEntryOptions>(option =>
            option.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5));

            #endregion

            services.AddSession(options =>
            {
                options.Cookie.Name = "MyApp.Session";
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            //注入mvc
            services.AddControllersWithViews();
            services.AddControllers(options =>
            {
                //options.Filters.Add<ValidFilterAttribute>();
                //options.Filters.Add<GlobalExceptionFilter>();

            });
            services.AddSingleton<IConfiguration>(configuration);
            services.BuildAspectCoreWithAutofacServiceProvider();

           
            var http = ServiceLocator.Resolve<IHttpContextAccessor>();
            http.HttpContext = new DefaultHttpContext();
            var session = new MockSession();
            http.HttpContext.Features.Set<ISessionFeature>(new SessionFeature { Session = session });
            http.HttpContext.Session = session;
            //SessionExtensions.SetString(http.HttpContext.Session, "CurrentUser_OpenId", "ofw2d6JqR7ssytSNX0a4pvV0IBOc");
            //http.HttpContext=((IHttpContextAccessor)http).HttpContext
            http.HttpContext.Session.SetString("CurrentUser_OpenId", "ofw2d6JqR7ssytSNX0a4pvV0IBOc");
            //new HttpContext() { Session =default(HttpSession) };

            //http.HttpContext?.Session.SetString("CurrentUser_UserName", "admin");
            //http.HttpContext?.Session.SetString("CurrentUser_Phone", "188XXXXXXXX");
            //http.HttpContext?.Session.SetString("CurrentUser_Name", "系统");

        }
    }

    // 实现一个简单的 ISession
    public class MockSession : ISession
    {
        private readonly Dictionary<string, object> _sessionStorage = new Dictionary<string, object>();

        public IEnumerable<string> Keys => _sessionStorage.Keys;

        public string Id => "mockSessionId";

        public bool IsAvailable => true;

        public void Clear()
        {
            _sessionStorage.Clear();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // 模拟异步提交
            return Task.CompletedTask;
        }

        public Task LoadAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // 模拟异步加载
            return Task.CompletedTask;
        }

        public void Remove(string key)
        {
            _sessionStorage.Remove(key);
        }

        public void Set(string key, byte[] value)
        {
            _sessionStorage[key] = value;
        }

        public bool TryGetValue(string key, out byte[] value)
        {
            if (_sessionStorage.TryGetValue(key, out var objValue) && objValue is byte[] bytesValue)
            {
                value = bytesValue;
                return true;
            }

            value = null;
            return false;
        }
    }
}
