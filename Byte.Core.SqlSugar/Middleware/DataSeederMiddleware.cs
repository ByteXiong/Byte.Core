using Byte.Core.SqlSugar.IDbContext;
using Microsoft.AspNetCore.Builder;

namespace Byte.Core.SqlSugar.Middleware;

public static class DataSeederMiddleware
{
    //private static readonly ILogger Logger = SerilogManager.GetLogger(typeof(DataSeederMiddleware));

    public static void UseDataSeederMiddleware(this IApplicationBuilder app, SugarDbContext dataContext)
    {
        if (app == null) throw new ArgumentNullException(nameof(app));

        try
        {
            if (dataContext.Configs.IsInitTable)
            {
                DataSeeder.InitSystemDataAsync(dataContext, dataContext.Configs.IsInitData,
                    dataContext.Configs.IsQuickDebug).Wait();
            }
        }
        catch (Exception e)
        {
            throw new Exception($"创建数据库初始化数据时错误.\n{e.Message}");
        }
    }
}
