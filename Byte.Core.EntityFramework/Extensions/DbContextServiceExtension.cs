using Byte.Core.EntityFramework.IDbContext;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.Extensions.DependencyInjection;

namespace Byte.Core.EntityFramework.Extensions
{
    /// <summary>
    /// IServiceCollection扩展
    /// </summary>
    public static class DbContextServiceExtension
    {
     
        public static IServiceCollection AddDbContextFactory(this IServiceCollection services,
            Action<DbContextFactory> action)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            var factory = DbContextFactory.Instance;
            factory.ServiceCollection = services;
            action?.Invoke(factory);

            return factory.ServiceCollection;
        }

        public static IDbContextCore GetDbContext(this IServiceProvider provider, DatabaseType dbContextTagName)
        {
            if (provider == null) throw new ArgumentNullException(nameof(provider));
            return provider.GetServices<IDbContextCore>().FirstOrDefault(m => m.Option.TagName == dbContextTagName);
        }

        public static IServiceCollection AddDbContext<IT, T>(this IServiceCollection services, DatabaseType tag,
            string connectionString) where IT : class, IDbContextCore where T : BaseDbContext, IT
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            return services.AddDbContext<IT, T>(new DbContextOption()
            {
                TagName = tag,
                ConnectionString = connectionString
            });
        }

        public static IServiceCollection AddDbContext<IT, T>(this IServiceCollection services, DbContextOption option) where IT : IDbContextCore where T : BaseDbContext, IT
        {
            if (services == null) throw new ArgumentNullException(nameof(services));
            if (option == null) throw new ArgumentNullException(nameof(option));
            services.Configure<DbContextOption>(options =>
            {
                options.IsOutputSql = option.IsOutputSql;
                options.ConnectionString = option.ConnectionString;
                options.ModelAssemblyName = option.ModelAssemblyName;
                options.TagName = option.TagName;
            });
            return services.AddDbContext<IT, T>();
        }
       
    }
}

