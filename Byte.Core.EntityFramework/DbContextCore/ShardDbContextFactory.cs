using Byte.Core.Common.IoC;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Byte.Core.EntityFramework.IDbContext
{
    public class ShardDbContextFactory<T> : IDesignTimeDbContextFactory<T> where T : DbContext, new()
    {
        public T CreateDbContext(string[] args)
        {
            return ServiceLocator.Resolve<T>();
        }
    }
}
