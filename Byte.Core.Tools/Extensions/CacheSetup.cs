using Byte.Core.Common.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Identity.Client;

namespace Byte.Core.Tools.Extensions;
/// <summary>
/// 缓存启动器
/// </summary>
public static class CacheSetup
{
    public static void AddCacheSetup(this IServiceCollection services)
    {
        if (services.IsNull())
            throw new ArgumentNullException(nameof(services));
        //也可以增加MemoryCache选项，但是MemoryCache不支持异步操作，需要自行实现

        //var options = App.GetOptions<CacheOptions>();
        //if (options.RedisCacheSwitch.Enabled)
        //{
        //    //开启了redis就优先使用redis
        //    services.AddSingleton<ICache, RedisCache>();
        //}
        //else if (options.DistributedCacheSwitch.Enabled)
        //{
        //    services.AddDistributedMemoryCache();
        //    services.AddSingleton<ICache, DistributedCache>();
        //}
        //else
        //{
        //    //都没有默认使用DistributedCache 防止异常
        //    services.AddDistributedMemoryCache();
        //    services.AddSingleton<ICache, DistributedCache>();
        //}
    }
}