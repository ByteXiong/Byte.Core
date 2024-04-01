using Byte.Core.Common.Extensions;
using Byte.Core.Common.Model;
using Byte.Core.Common.Web;
using Byte.Core.SqlSugar.Base;
using Byte.Core.SqlSugar.ConfigOptions;
using Byte.Core.SqlSugar.SnowflakeIdHelper;
using Microsoft.Extensions.DependencyInjection;
using SqlSugar;

namespace Byte.Core.SqlSugar.Extensions;

/// <summary>
/// SqlSugar 启动器
/// </summary>
public static class SqlSugarSetup
{
    public static void AddSqlSugarSetup(this IServiceCollection services, Configs configs)
    {
        if (services.IsNull())
            throw new ArgumentNullException(nameof(services));
        var dataConnection = configs.DataConnection;
        if (!dataConnection.ConnectionItem.Any())
        {
            throw new Exception("请确保配置数据库配置DataConnection无误;");
        }

        // var connectionMaster =
        //     dataConnection.ConnectionItem.Where(x => x.ConnId == configs.DefaultDataBase && x.Enabled).ToList();
        var allConnectionItem =
            dataConnection.ConnectionItem.Where(x => x.Enabled).ToList();
        if (!allConnectionItem.Any())
        {
            throw new Exception($"请添加数据库");
        }

        List<ConnectionConfig> allConnectionConfig = new List<ConnectionConfig>();
        ConnectionConfig masterDb = null; //主库
        List<SlaveConnectionConfig> slaveDbs = null; //从库列表

        foreach (var connectionItem in allConnectionItem)
        {
            //if (connectionItem.DbType == (int)DatabaseType.Sqlite)
            //{
            //    connectionItem.ConnectionString = "DataSource=" + Path.Combine(AppSettings.ContentRootPath,
            //        connectionItem.ConnectionString ?? string.Empty);
            //}

            List<ConnectionItem> connectionSlaves = new List<ConnectionItem>();
            if (configs.IsCqrs)
            {
                connectionSlaves = dataConnection.ConnectionItem
                    .Where(x => x.DbType == connectionItem.DbType && x.ConnId != connectionItem.ConnId && x.Enabled)
                    .ToList();
                if (!connectionSlaves.Any())
                {
                    throw new Exception($"请确保数据库ID:{connectionItem.ConnId}对应的从库的Enabled为true;");
                }
            }

            if (configs.IsCqrs)
            {
                slaveDbs = new List<SlaveConnectionConfig>();
                connectionSlaves.ForEach(db =>
                {
                    slaveDbs.Add(new SlaveConnectionConfig
                    {
                        HitRate = db.HitRate,
                        ConnectionString = db.ConnectionString
                    });
                });
            }

            masterDb = new ConnectionConfig
            {
                ConfigId = connectionItem.ConnId,
                ConnectionString = connectionItem.ConnectionString,
                DbType = (DbType)connectionItem.DbType,
                LanguageType = LanguageType.Chinese,
                IsAutoCloseConnection = true,
                //IsShardSameThread = false,
                MoreSettings = new ConnMoreSettings
                {
                    IsAutoRemoveDataCache = true,
                    SqlServerCodeFirstNvarchar = true, //sqlserver默认使用nvarchar
                },
                ConfigureExternalServices = new ConfigureExternalServices
                {
                    EntityService = (c, p) =>
                    {
                        //p.DbColumnName = UtilMethods.ToUnderLine(p.DbColumnName); //字段使用驼峰转下划线，不需要请注释
                        if ((DbType)connectionItem.DbType == DbType.MySql && p.DataType == "varchar(max)")
                        {
                            p.DataType = "longtext";
                        }

                        /***低版本C#写法***/
                        // int?  decimal?这种 isnullable=true 不支持string(下面.NET 7支持)
                        if (p.IsPrimarykey == false && c.PropertyType.IsGenericType &&
                            c.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>))
                        {
                            p.IsNullable = true;
                        }

                        /***高版C#写法***/
                        //支持string?和string  
                        // if (p.IsPrimarykey == false && new NullabilityInfoContext()
                        //         .Create(c).WriteState is NullabilityState.Nullable)
                        // {
                        //     p.IsNullable = true;
                        // }
                    },
                },
                // 从库
                SlaveConnectionConfigs = slaveDbs
            };
            allConnectionConfig.Add(masterDb);
        }

        var sugar = new SqlSugarScope(allConnectionConfig,
            db =>
            {
                allConnectionConfig.ForEach(
                    config =>
                    {
                        var sugarScopeProvider = db.GetConnectionScope((string)config.ConfigId);

                        #region 接口过滤器

                        sugarScopeProvider.QueryFilter.AddTableFilter<ISoftDeletedEntity>(x => x.IsDeleted == false);

                        #endregion

                        #region 读写事件

                        sugarScopeProvider.Aop.DataExecuting = DataExecuting;

                        #endregion

                        #region 日志

                        //sugarScopeProvider.Aop.OnLogExecuting = (sql, pars) => OnLogExecuting(sugarScopeProvider,
                        //    Enum.GetName(typeof(SugarActionType), sugarScopeProvider.SugarActionType), sql, pars,
                        //    configs, config);

                        #endregion

                        #region 耗时

                        //sugarScopeProvider.Aop.OnLogExecuted = (_, _) => OnLogExecuted(configs, sugarScopeProvider.Ado);

                        #endregion
                    });
            });
        services.AddSingleton<ISqlSugarClient>(sugar);
    }


    #region 读写事件

    /// <summary>
    /// 读写事件
    /// </summary>
    /// <param name="value"></param>
    /// <param name="entityInfo"></param>
    private static void DataExecuting(object value, DataFilterModel entityInfo)
    {
        if (entityInfo.EntityValue is RootKey<long> { Id: 0 } rootEntity)
        {
            rootEntity.Id = IdHelper.GetLongId();
        }

        if (entityInfo.EntityValue is BaseEntity baseEntity)
        {
            switch (entityInfo.OperationType)
            {
                case DataFilterType.InsertByObject:
                {
                    if (baseEntity.CreateTime == DateTime.MinValue)
                    {
                        baseEntity.CreateTime = DateTime.Now;
                    }

                    break;
                }
                case DataFilterType.UpdateByObject:
                    baseEntity.UpdateTime = DateTime.Now;
                    break;
            }

         
                switch (entityInfo.OperationType)
                {
                    case DataFilterType.InsertByObject:
                    {
                        if (baseEntity.CreateBy.IsNullOrEmpty())
                        {
                            baseEntity.CreateBy = CurrentUser.UserAccount;
                        }

                        break;
                    }
                    case DataFilterType.UpdateByObject:
                        baseEntity.UpdateBy = CurrentUser.UserAccount;
                        break;
                }
            
        }
    }

    #endregion

    //#region 日志

    //private static void OnLogExecuting(ISqlSugarClient sqlSugar, string operate, string sql,
    //    SugarParameter[] pars, Configs configs, ConnectionConfig connection)
    //{
    //    try
    //    {
    //        if (!configs.SqlLog.Enabled)
    //        {
    //            return;
    //        }

    //        if (configs.IsQuickDebug && configs.Middleware.MiniProfiler.Enabled)
    //        {
    //            MiniProfiler.Current.CustomTiming("SQL",
    //                "【SQL参数】:\n" + GetParams(pars) + "【SQL语句】：\n" + sql);
    //        }

    //        if (configs.SqlLog.ToDb.Enabled || configs.SqlLog.ToFile.Enabled || configs.SqlLog.ToConsole.Enabled)
    //        {
    //            var httpUser = AutofacHelper.GetScopeService<IHttpUser>();
    //            using (LoggerPropertyConfiguration.Create.AddAopSqlProperty(sqlSugar, configs.SqlLog))
    //            {
    //                Log.Information(
    //                    "Executed Sql--> User:[{User}] Operate:[{Operate}] ConnId:[{ConnId}] {Sql}",
    //                    httpUser?.Account, operate, connection.ConfigId, UtilMethods.GetNativeSql(sql, pars));
    //            }
    //        }
    //    }
    //    catch (Exception e)
    //    {
    //        Log.Fatal("Error occured OnLogExecuting:" + e);
    //    }
    //}

    ///// <summary>
    ///// 参数拼接字符串
    ///// </summary>
    ///// <param name="pars"></param>
    ///// <returns></returns>
    //private static string GetParams(SugarParameter[] pars)
    //{
    //    return pars.Aggregate("", (current, p) => current + $"{p.ParameterName}:{p.Value}\n");
    //}

    //private static void OnLogExecuted(Configs configs, IAdo ado)
    //{
    //    if (!configs.SqlLog.Enabled)
    //    {
    //        return;
    //    }

    //    if (configs.IsQuickDebug && configs.Middleware.MiniProfiler.Enabled)
    //    {
    //        MiniProfiler.Current.CustomTiming("SQL",
    //            $"【Sql耗时】:{Math.Round(ado.SqlExecutionTime.TotalMilliseconds / 1000d, 4)}秒\r\n");
    //    }

    //    if (configs.SqlLog.ToConsole.Enabled)
    //    {
    //        if (ado.SqlExecutionTime.TotalMilliseconds > 5000)
    //        {
    //            ConsoleHelper.WriteLine($"[Time]:{Math.Round(ado.SqlExecutionTime.TotalMilliseconds / 1000d, 4)}秒",
    //                ConsoleColor.DarkCyan);
    //            ConsoleHelper.WriteLine($"[提示]:当前sql执行耗时较长,请检查进行优化\r\n",
    //                ConsoleColor.Red);
    //        }
    //        else
    //        {
    //            ConsoleHelper.WriteLine($"[Time]:{Math.Round(ado.SqlExecutionTime.TotalMilliseconds / 1000d, 4)}秒\r\n",
    //                ConsoleColor.DarkCyan);
    //        }
    //    }
    //}

    //#endregion
}
