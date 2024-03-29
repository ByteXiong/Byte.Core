using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;

namespace Byte.Core.EntityFramework.IDbContext
{
    public class InMemoryDbContext : BaseDbContext, IInMemoryDbContext
    {
        public InMemoryDbContext(DbContextOption option) : base(option)
        {

        }
        public InMemoryDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        public InMemoryDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

        public override DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            throw new System.NotImplementedException();
        }


    }
}
