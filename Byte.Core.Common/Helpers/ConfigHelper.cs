using Microsoft.Extensions.Configuration;

namespace Byte.Core.Common.Helpers
{
    public class ConfigHelper
    {
        private static IConfiguration _config;
        private static object _lock = new object();
        public static IConfiguration Configuration
        {
            get
            {
                if (_config == null)
                {
                    lock (_lock)
                    {
                        if (_config == null)
                        {
                            var builder = new ConfigurationBuilder()
                                .SetBasePath(AppContext.BaseDirectory)
                             .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                             .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", optional: true, reloadOnChange: true);
                            _config = builder.Build();
                        }
                    }
                }

                return _config;
            }
            set
            {
                _config = value;
            }
        }

        /// <summary>
        /// 从AppSettings获取key的值
        /// </summary>
        /// <param name="key">key</param>
        /// <returns></returns>
        public static string GetValue(string key)
        {
            return Configuration[key];
        }

        /// <summary>
        /// 获取连接字符串
        /// </summary>
        /// <param name="nameOfCon">连接字符串名</param>
        /// <returns></returns>
        public static string GetConnectionString(string nameOfCon)
        {
            return Configuration.GetConnectionString(nameOfCon);
        }
    }
}
