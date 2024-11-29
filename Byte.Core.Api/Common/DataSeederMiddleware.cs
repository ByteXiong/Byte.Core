using Byte.Core.Api.db;
using Byte.Core.Common.Converters;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.SqlSugar.IDbContext;
using Newtonsoft.Json;
using SqlSugar;
using System.Reflection;
using System.Text;

namespace Byte.Core.Api.Common
{
    public static class DataSeederMiddleware
    {

        public static async Task UseDataSeederMiddlewareAsync(this IApplicationBuilder app, string entityAssemblyName)
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            try
            {
                var dataContext = ServiceLocator.Resolve<SugarDbContext>();

                if (dataContext.Configs.IsInitTable)
                {
                    Console.WriteLine($"程序正在启动....", ConsoleColor.Green);
                    Console.WriteLine($"是否开发环境: {dataContext.Configs.IsQuickDebug}");
                    Console.WriteLine($"Master Db Id: {dataContext.Db.CurrentConnectionConfig.ConfigId}");
                    Console.WriteLine($"Master Db Type: {dataContext.Db.CurrentConnectionConfig.DbType}");

                    Console.WriteLine(
                         $"Master Db ConnectString: {dataContext.Db.CurrentConnectionConfig.ConnectionString}");
                    Console.WriteLine("初始化主库....");
                    if (dataContext.DbType != DbType.Oracle)
                    {
                       dataContext.Db.DbMaintenance.CreateDatabase();
                    }
                    else
                    {
                        throw new Exception("sqlsugar官方表示Oracle不支持代码建库，请先建库再启动项目");
                    }
                    Console.WriteLine("初始化主库成功。", ConsoleColor.Green);
                    Console.WriteLine("初始化主库数据表....");
                    #region 初始化主库数据表
                    //继承自BaseEntity或者RootKey<>的类型
                    //一些没有继承的需手动维护添加
                    var assembly = RuntimeHelper.GetAssembly(entityAssemblyName);
                    if (assembly == null)
                    {
                        throw new DllNotFoundException($"the dll \"{entityAssemblyName}\" not be found");
                    }

                    //过滤掉非接口及泛型接口
                    var types = assembly.GetTypes().Where(t => t.IsClass).ToArray();

                    dataContext.Db.CodeFirst.InitTables(types);
                    #endregion

                    #region 初始化主库数据

                    if (dataContext.Configs.IsInitData)
                    {
                        Console.WriteLine("初始化主库种子数据....");
                        JsonSerializerSettings setting = new JsonSerializerSettings();
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                        setting.NullValueHandling = NullValueHandling.Ignore;
                        setting.Converters.Add(new IntToBoolConverter());

                        var basePath = System.IO.Path.GetDirectoryName(typeof(Program).Assembly.Location);
                        string seedDataFolder = "/db/{0}.json";
                        seedDataFolder = basePath + seedDataFolder;


                        #region 菜单

                        if (!await dataContext.Db.Queryable<Menu>().AnyAsync())
                        {
                            var attr = typeof(Menu).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<NavicatJson<Menu>>(jsonStr, setting);

                                await dataContext.GetEntityDb<Menu>().InsertRangeAsync(list.RECORDS);

                                Console.WriteLine(
                                    $"Entity:{nameof(Menu)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }

                        #endregion


                        #region 角色-菜单关系

                        if (!await dataContext.Db.Queryable<Role_Menu>().AnyAsync())
                        {
                            var attr = typeof(Role_Menu).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<Role_Menu>>(jsonStr, setting);

                                await dataContext.GetEntityDb<Role_Menu>().InsertRangeAsync(list);

                                Console.WriteLine(
                                    $"Entity:{nameof(Role_Menu)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }
                        #endregion

                        #region 角色

                        if (!await dataContext.Db.Queryable<Role>().AnyAsync())
                        {
                            var attr = typeof(Role).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<Role>>(jsonStr, setting);

                                await dataContext.GetEntityDb<Role>().InsertRangeAsync(list);

                                Console.WriteLine(
                                    $"Entity:{nameof(Role)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }
                        #endregion




                        #region 用户

                        if (!await dataContext.Db.Queryable<User>().AnyAsync())
                        {
                            var attr = typeof(User).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<User>>(jsonStr, setting);
                                list.ForEach(x => x.Password = "123456".ToMD5String());
                                await dataContext.GetEntityDb<User>().InsertRangeAsync(list);

                                Console.WriteLine(
                                    $"Entity:{nameof(User)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }

                        #endregion

                        #region 用户-组织-角色关系

                        if (!await dataContext.Db.Queryable<User_Dept_Role>().AnyAsync())
                        {
                            var attr = typeof(User_Dept_Role).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<User_Dept_Role>>(jsonStr, setting);
                                await dataContext.GetEntityDb<User_Dept_Role>().InsertRangeAsync(list);

                                Console.WriteLine(
                                    $"Entity:{nameof(User_Dept_Role)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }

                        #endregion


                        #region 组织关系

                        if (!await dataContext.Db.Queryable<Dept>().AnyAsync())
                        {
                            var attr = typeof(Dept).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<Dept>>(jsonStr, setting);
                                await dataContext.GetEntityDb<Dept>().InsertRangeAsync(list);

                                Console.WriteLine(
                                    $"Entity:{nameof(Dept)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }

                        #endregion





                        #region 表结构
                        if (!await dataContext.Db.Queryable<TableView>().AnyAsync())
                        {
                            var attr = typeof(TableView).GetCustomAttribute<SugarTable>();
                            if (attr != null)
                            {
                                StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                                var jsonStr = f2.ReadToEnd();
                                f2.Close();
                                f2.Dispose();

                                var list = JsonConvert.DeserializeObject<List<TableView>>(jsonStr, setting);
                                Console.WriteLine(list.Count());
                                await dataContext.Db.InsertNav(list).Include(x => x.TableColumns).ExecuteCommandAsync();

                                Console.WriteLine(
                                    $"Entity:{nameof(TableView)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                                    ConsoleColor.Green);
                            }
                        }

                        //if (!await dataContext.Db.Queryable<TableColumn>().AnyAsync())
                        //{
                        //    var attr = typeof(TableColumn).GetCustomAttribute<SugarTable>();
                        //    if (attr != null)
                        //    {
                        //        StreamReader f2 = new StreamReader(string.Format(seedDataFolder, attr.TableName), Encoding.UTF8);
                        //        var jsonStr = f2.ReadToEnd();
                        //        f2.Close();
                        //        f2.Dispose();

                        //        var list = JsonConvert.DeserializeObject<List<TableColumn>>(jsonStr, setting);
                        //        await dataContext.GetEntityDb<TableColumn>().InsertRangeAsync(list);

                        //        Console.WriteLine(
                        //            $"Entity:{nameof(TableColumn)}-->Table:{attr.TableName}-->Desc:{attr.TableDescription}-->初始数据成功！",
                        //            ConsoleColor.Green);
                        //    }
                        //}

                        #endregion

                    }
                    #endregion
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }


    }
}
