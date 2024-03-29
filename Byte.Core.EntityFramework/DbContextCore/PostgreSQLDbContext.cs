﻿using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Byte.Core.EntityFramework.IDbContext
{
    public class PostgreSQLDbContext : BaseDbContext, IPostgreSQLDbContext
    {
        public PostgreSQLDbContext(DbContextOption option) : base(option)
        {

        }
        public PostgreSQLDbContext(IOptions<DbContextOption> option) : base(option)
        {
        }

        public PostgreSQLDbContext(DbContextOptions options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(Option.ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }

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

        //    using (var cmd = new NpgsqlCommand(sql, (NpgsqlConnection)connection))
        //    {
        //        cmd.CommandTimeout = cmdTimeout;
        //        if (parameters != null && parameters.Length > 0)
        //        {
        //            cmd.Parameters.AddRange(parameters);
        //        }

        //        using (var da = new NpgsqlDataAdapter(cmd))
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
