using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.Options;

namespace Byte.Core.EntityFramework.IDbContext
{
    public class OracleDbContext : BaseDbContext, IOracleDbContext
    {
        public OracleDbContext(DbContextOption option) : base(option)
        {

        }
        public OracleDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        public OracleDbContext(DbContextOptions options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseOracle(Option.ConnectionString, b => b.UseOracleSQLCompatibility("11"));
            base.OnConfiguring(optionsBuilder);


        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                foreach (var property in entityType.GetProperties())
                {
                    if (property.ClrType == typeof(Guid))
                    {
                        property.SetValueConverter(new ValueConverter<Guid, string>(v => Convert.ToString(v).ToUpper(), v => Guid.Parse(v)));
                    }

                    if (property.ClrType == typeof(Guid?))
                    {
                        property.SetValueConverter(new ValueConverter<Guid?, string>
                            (v => Convert.ToString(v).ToUpper(),
                              v => v.Length > 0 ? Guid.Parse(v) : null));

                    }
                }
            }
        }

        //public class BoolToIntConverter : ValueConverter<Guid, string>
        //{
        //    public BoolToIntConverter(ConverterMappingHints mappingHints = null)
        //        : base(
        //              v => Convert.ToString(v),
        //              v => Guid.Parse(v),
        //              mappingHints)
        //    {
        //    }

        //    public static ValueConverterInfo DefaultInfo { get; }
        //        = new ValueConverterInfo(typeof(Guid), typeof(string), i => new BoolToIntConverter(i.MappingHints));
        //}

        //public override DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        //{
        //    return GetDataTables(sql, cmdTimeout, parameters).FirstOrDefault();
        //}

        //public override List<DataTable> GetDataTables(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        //{
        //    var dts = new List<DataTable>();
        //    //TODO： connection 不能dispose 或者 用using，否则下次获取connection会报错提示“the connectionstring property has not been initialized。”
        //    var connection = Database.GetDbConnection();
        //    if (connection.State != ConnectionState.Open)
        //        connection.Open();
        //    using (var cmd = new OracleCommand(sql, (OracleConnection)connection))
        //    {
        //        cmd.CommandTimeout = cmdTimeout;
        //        if (parameters != null && parameters.Length > 0)
        //        {
        //            cmd.Parameters.AddRange(parameters);
        //        }

        //        using (var da = new OracleDataAdapter(cmd))
        //        {
        //            using (var ds = new DataSet())
        //            {
        //                da.Fill(ds);
        //                foreach (DataTable table in ds.Tables)
        //                {
        //                    dts.Add(table);
        //                }
        //            }
        //        }
        //    }
        //    connection.Close();

        //    return dts;
        //}

    }
}
