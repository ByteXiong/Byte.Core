using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;
using Expression = System.Linq.Expressions.Expression;

namespace Byte.Core.EntityFramework.IDbContext
{
    public abstract class BaseDbContext : DbContext, IDbContextCore
    {

        #region 配置
        public DbContextOption Option { get; set; }
        public DatabaseFacade GetDatabase() => Database;

        protected BaseDbContext(DbContextOption option)
        {
            Option = option ?? ServiceLocator.Resolve<IOptions<DbContextOption>>().Value;
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="option"></param>
        protected BaseDbContext(IOptions<DbContextOption> option)
        {
            if (option == null)
                throw new ArgumentNullException(nameof(option));
            if (string.IsNullOrEmpty(option.Value.ConnectionString))
                throw new ArgumentNullException(nameof(option.Value.ConnectionString));
            if (string.IsNullOrEmpty(option.Value.ModelAssemblyName))
                throw new ArgumentNullException(nameof(option.Value.ModelAssemblyName));
            Option = option.Value;
        }

        protected BaseDbContext(DbContextOptions options) : base(options)
        {
            Option = ServiceLocator.Resolve<IOptions<DbContextOption>>().Value;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            MappingEntityTypes(modelBuilder);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// 生成数据库 coedfrist
        /// </summary>
        /// <returns></returns>
        public virtual bool EnsureCreated()
        {
            return Database.EnsureCreated();
        }
        /// <summary>
        /// 生成数据库 coedfrist
        /// </summary>
        /// <returns></returns>
        public virtual async Task<bool> EnsureCreatedAsync()
        {
            return await Database.EnsureCreatedAsync();
        }
        #endregion

        #region 事务
        /// <summary>
        /// 事务（同步）
        /// </summary>
        /// <returns></returns>
        public virtual IDbContextTransaction BeginTransaction() => Database.BeginTransaction();
        /// <summary>
        /// 事务（异步）
        /// </summary>
        /// <returns></returns>
        public virtual Task<IDbContextTransaction> BeginTransactionAsync() => Database.BeginTransactionAsync();
        #endregion




        #region EF 
        #region 添加
        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="withTrigger"></param>
        /// <returns></returns>
        public virtual int Add<T>(T entity, bool withTrigger = false) where T : class
        {
            base.Add(entity);
            return withTrigger ? 0 : SaveChanges();
        }
        public virtual async Task<int> AddAsync<T>(T entity, bool withTrigger = false) where T : class
        {
            await base.AddAsync(entity);
            return withTrigger ? 0 : await SaveChangesAsync();
        }
        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="withTrigger"></param>
        /// <returns></returns>
        public virtual int AddRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            if (entities == null || entities.Count() == 0) return 0;
            base.AddRange(entities);
            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> AddRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            if (entities == null || entities.Count() == 0) return 0;
            await base.AddRangeAsync(entities);
            return withTrigger ? 0 : await SaveChangesAsync();
        }
        #endregion
        #region 总数
        /// <summary>
        /// 总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Count<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            if (where != null)
            {
                return Set<T>().Count(where);
            }

            return Set<T>().Count();
        }

        public virtual Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return ((where == null) ? Set<T>().CountAsync() : Set<T>().CountAsync(where));
        }
        #endregion
        #region 删除
        public virtual int Delete<T, TKey>(TKey key, bool withTrigger = false) where T : class
        {
            T entity = Find<T>(key);
            Remove(entity);
            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> DeleteAsync<T, TKey>(TKey key, bool withTrigger = false) where T : class
        {
            T entity = Find<T>(key);
            Remove(entity);
            return withTrigger ? 0 : await SaveChangesAsync();
        }
        public virtual int DeleteRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            if (entities != null)
            {
                DbSet<T> dbSet = Set<T>();
                if (entities.Count > 0)
                {
                    foreach (T item in entities)
                    {
                        dbSet.Remove(item);
                    }
                }

                return withTrigger ? 0 : SaveChanges();
            }
            return 0;
        }

        public virtual async Task<int> DeleteRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            if (entities != null)
            {
                DbSet<T> dbSet = Set<T>();

                if (entities.Count > 0)
                {
                    foreach (T item in entities)
                    {
                        dbSet.Remove(item);
                    }
                }
                return withTrigger ? 0 : await SaveChangesAsync();
            }

            return 0;
        }
        /// <summary>
        /// 条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        public virtual int Delete<T>(Expression<Func<T, bool>> where, bool withTrigger = false) where T : class
        {

            RemoveRange(Set<T>().Where(where));
            //return Remove(Set<T>().Where(where)).Entity.Count();//改一下
            //RemoveRange(Set<T>().Where(where));
            return withTrigger ? 0 : SaveChanges();


        }

        public virtual async Task<int> DeleteAsync<T>(Expression<Func<T, bool>> where, bool withTrigger = false) where T : class
        {
            RemoveRange(Set<T>().Where(where));
            //Remove(Set<T>().Where(where));
            return withTrigger ? 0 : await SaveChangesAsync();

        }
        #endregion
        #region 修改
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TKey"></typeparam>
        /// <param name="entity"></param>
        public virtual void AttachUpdate<T, TKey>(T entity) where T : class, IBaseModel<TKey>
        {
            T entity2 = Find<T>(entity.Id);
            Entry(entity2).CurrentValues.SetValues(entity);
        }
        public virtual async Task AttachUpdateAsync<T, TKey>(T entity) where T : class, IBaseModel<TKey>
        {
            T entity2 = await FindAsync<T>(entity.Id);
            Entry(entity2).CurrentValues.SetValues(entity);
        }
        public virtual int Update<T, TKey>(T entity, bool withTrigger = false) where T : class, IBaseModel<TKey>
        {
            T entity2 = Find<T>(entity.Id);
            Entry(entity2).CurrentValues.SetValues(entity);
            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> UpdateAsync<T, TKey>(T entity, bool withTrigger = false) where T : class, IBaseModel<TKey>
        {
            T entity2 = await FindAsync<T>(entity.Id);
            Entry(entity2).CurrentValues.SetValues(entity);
            return withTrigger ? 0 : await SaveChangesAsync();
        }







        public virtual int UpdateRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            Set<T>().AttachRange(entities.ToArray());
            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> UpdateRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class
        {
            Set<T>().AttachRange(entities.ToArray());
            return withTrigger ? 0 : await SaveChangesAsync();
        }
        public virtual int UpdateRange<T>(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) where T : class
        {

            foreach (var model in entities)
            {

                if (updateColumns != null && updateColumns.Length != 0)
                {
                    if (Entry(model).State == EntityState.Added || Entry(model).State == EntityState.Detached)
                    {
                        Set<T>().Attach(model);
                    }

                    foreach (string propertyName in updateColumns)
                    {
                        Entry(model).Property(propertyName).IsModified = true;
                    }
                }
                else
                {
                    Entry(model).State = EntityState.Modified;
                }
            }
            return withTrigger ? 0 : SaveChanges();
        }
        public virtual async Task<int> UpdateRangeAsync<T>(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) where T : class
        {
            Set<T>().AttachRange(entities.ToArray());
            foreach (var model in entities)
            {

                if (updateColumns != null && updateColumns.Length != 0)
                {
                    foreach (string propertyName in updateColumns)
                    {
                        Entry(model).Property(propertyName).IsModified = true;
                    }
                }
            }
            return withTrigger ? 0 : await SaveChangesAsync();
        }




        public virtual int UpdateRange<T>(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) where T : class
        {

            Set<T>().AttachRange(entities.ToArray());

            foreach (var entitie in entities)
            {

                if (updateColumns != null)
                {
                    NewExpression expression = (NewExpression)updateColumns.Body;
                    expression.Members.ForEach(aBinding =>
                    {
                        Entry(entitie).Property(aBinding.Name).IsModified = true;
                    });
                }
            }
            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> UpdateRangeAsync<T>(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) where T : class
        {

            Set<T>().AttachRange(entities.ToArray());

            foreach (var entitie in entities)
            {

                if (updateColumns != null)
                {
                    NewExpression expression = (NewExpression)updateColumns.Body;
                    expression.Members.ForEach(aBinding =>
                    {
                        Entry(entitie).Property(aBinding.Name).IsModified = true;
                    });
                }
            }
            return withTrigger ? 0 : await SaveChangesAsync();
        }


        public virtual int Update<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class
        {
            if (updateColumns != null && updateColumns.Length != 0)
            {
                if (Entry(model).State == EntityState.Added || Entry(model).State == EntityState.Detached)
                {
                    Set<T>().Attach(model);
                }

                foreach (string propertyName in updateColumns)
                {
                    Entry(model).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                Entry(model).State = EntityState.Modified;
            }

            return withTrigger ? 0 : SaveChanges();
        }

        public virtual async Task<int> UpdateAsync<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class
        {
            if (updateColumns != null && updateColumns.Length != 0)
            {
                if (Entry(model).State == EntityState.Added || Entry(model).State == EntityState.Detached)
                {
                    Set<T>().Attach(model);
                }

                foreach (string propertyName in updateColumns)
                {
                    Entry(model).Property(propertyName).IsModified = true;
                }
            }
            else
            {
                Entry(model).State = EntityState.Modified;
            }

            return withTrigger ? 0 : await SaveChangesAsync();
        }


        public virtual int Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) where T : class
        {
            // 获取需要更新的字段及其新值
            var updateExpression = updateFactory.Body as MemberInitExpression;
            if (updateExpression == null)
            {
                throw new ArgumentException("updateFactory 必须为形如 u => new T { Prop1 = value1, Prop2 = value2, ... } 的形式", nameof(updateFactory));
            }

            var memberBindings = updateExpression.Bindings;
            var updateValues = new Dictionary<string, object>();
            foreach (var binding in memberBindings)
            {
                var memberAssignment = binding as MemberAssignment;
                if (memberAssignment == null)
                {
                    throw new ArgumentException("updateFactory 必须为形如 u => new T { Prop1 = value1, Prop2 = value2, ... } 的形式", nameof(updateFactory));
                }

                var memberName = memberAssignment.Member.Name;
                var memberValue = GetValueFromExpression(memberAssignment.Expression);

                updateValues.Add(memberName, memberValue);
            }
            IQueryable<T> entities = Set<T>().Where(where);

            foreach (var entity in entities)
            {
                var entry = Entry(entity);
                foreach (var dic in updateValues)
                {
                    entry.Property(dic.Key).CurrentValue = dic.Value;
                    entry.Property(dic.Key).IsModified = true;
                }
            }

         return withTrigger ? 0 : SaveChanges();

        }

        public virtual async Task<int> UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) where T : class
        {
            // 获取需要更新的字段及其新值
            var updateExpression = updateFactory.Body as MemberInitExpression;
            if (updateExpression == null)
            {
                throw new ArgumentException("updateFactory 必须为形如 u => new T { Prop1 = value1, Prop2 = value2, ... } 的形式", nameof(updateFactory));
            }

            var memberBindings = updateExpression.Bindings;
            var updateValues = new Dictionary<string, object>();
            foreach (var binding in memberBindings)
            {
                var memberAssignment = binding as MemberAssignment;
                if (memberAssignment == null)
                {
                    throw new ArgumentException("updateFactory 必须为形如 u => new T { Prop1 = value1, Prop2 = value2, ... } 的形式", nameof(updateFactory));
                }

                var memberName = memberAssignment.Member.Name;
                var memberValue = GetValueFromExpression(memberAssignment.Expression);

                updateValues.Add(memberName, memberValue);
            }
            IQueryable<T> entities = Set<T>().Where(where);
            
                foreach (var entity in entities)
                {
                    var entry = Entry(entity);
                    foreach (var dic in updateValues)
                    {
                        entry.Property(dic.Key).CurrentValue = dic.Value;
                        entry.Property(dic.Key).IsModified = true;
                    }
                }

            return withTrigger ? 0 :await  SaveChangesAsync();
        }


        private static object GetValueFromExpression(Expression expression)
        {
            if (expression is ConstantExpression constantExpression)
            {
                return constantExpression.Value;
            }
            var lambda = Expression.Lambda(expression).Compile();
            return lambda.DynamicInvoke();
        }
        #endregion

        #region 存在
        public virtual bool Any<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return ((where == null) ? Set<T>().Any() : Set<T>().Any(where));
        }
        public virtual Task<bool> AnyAsync<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return ((where == null) ? Set<T>().AnyAsync() : Set<T>().AnyAsync(where));
        }

        #endregion

        #region 查询第一条
        public virtual T Find<T, TKey>(TKey key) where T : class
        {
            return base.Find<T>(new object[1]
            {
                key
            });
        }

        public virtual async Task<T> FindAsync<T, TKey>(TKey key) where T : class
        {
            return await base.FindAsync<T>(new object[1]
            {
                key
            });
        }


        public virtual T Find<T>(object key) where T : class
        {
            return base.Find<T>(key);
        }
        public virtual async Task<T> FindAsync<T>(object key) where T : class
        {
            return await base.FindAsync<T>(key);
        }

        #endregion
        public virtual IQueryable<T> GetIQueryable<T>(Expression<Func<T, bool>> where = null, bool asNoTracking = false) where T : class
        {
            IQueryable<T> queryable = Set<T>().AsQueryable();
            if (where != null)
            {
                queryable = queryable.Where(where);
            }

            if (!asNoTracking)// 实体查询,不能new 先赋值(不追踪)
            {
                queryable = queryable.AsNoTracking();
            }

            return queryable;
        }

        public virtual T GetSingleOrDefault<T>(Expression<Func<T, bool>> where = null) where T : class
        {

            return where == null ? GetDbSet<T>().SingleOrDefault() : GetDbSet<T>().SingleOrDefault(where);
        }

        public virtual async Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> where = null) where T : class
        {
            return await ((where == null) ? Set<T>().SingleOrDefaultAsync() : Set<T>().SingleOrDefaultAsync(where));
        }

        public virtual IQueryable<T> FilterWithInclude<T>(Func<IQueryable<T>, IQueryable<T>> include, Expression<Func<T, bool>> where) where T : class
        {
            IQueryable<T> queryable = GetDbSet<T>().AsQueryable();
            if (where != null)
            {
                queryable = GetDbSet<T>().Where(where);
            }

            if (include != null)
            {
                queryable = include(queryable);
            }

            return queryable;
        }




        public virtual List<IEntityType> GetAllEntityTypes()
        {
            return Model.GetEntityTypes().ToList();
        }


        public virtual DbSet<T> GetDbSet<T>() where T : class
        {
            if (Model.FindEntityType(typeof(T)) != null)
                return Set<T>();
            throw new Exception($"类型{typeof(T).Name}未在数据库上下文中注册，请先在DbContextOption设置ModelAssemblyName以将所有实体类型注册到数据库上下文中。");
        }

        public T GetByCompileQuery<T, TKey>(TKey id) where T : BaseModel<TKey>
        {
            return Microsoft.EntityFrameworkCore.EF.CompileQuery((DbContext context, TKey id) => context.Set<T>().Find(id))(this, id);
        }
        public Task<T> GetByCompileQueryAsync<T, TKey>(TKey id) where T : BaseModel<TKey>
        {
            return Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((DbContext context, TKey id) => context.Set<T>().Find(id))(this, id);
        }


        public IList<T> GetByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileQuery((DbContext context) => context.Set<T>().AsNoTracking().Where(filter).ToList())(this);
        }
        public Task<List<T>> GetByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((DbContext context) => context.Set<T>().AsNoTracking().Where(filter).ToList())(this);
        }

        public T FirstOrDefaultByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileQuery((DbContext context) => context.Set<T>().AsNoTracking().FirstOrDefault(filter))(this);
        }
        public Task<T> FirstOrDefaultByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((DbContext context) => context.Set<T>().AsNoTracking().FirstOrDefault(filter))(this);
        }
        public T FirstOrDefaultWithTrackingByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileQuery((DbContext context) => context.Set<T>().FirstOrDefault(filter))(this);
        }
        public Task<T> FirstOrDefaultWithTrackingByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((DbContext context) => context.Set<T>().FirstOrDefault(filter))(this);
        }
        public int CountByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileQuery((DbContext context) => context.Set<T>().Count(filter))(this);
        }
        public Task<int> CountByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class
        {
            if (filter == null) filter = m => true;
            return Microsoft.EntityFrameworkCore.EF.CompileAsyncQuery((DbContext context) => context.Set<T>().Count(filter))(this);
        }
        #endregion
        #region Sql

        //public virtual PaginationResult ToPagedResultsAsync<TView>(this string sql, string[] orderBys, int pageIndex, int pageSize,
        //Action<TView> eachAction = null)
        //{
        //    var total = SqlQuery<T, int>($"select count(1) from ({sql}) as s").FirstOrDefault();
        //    var jsonResults = SqlQuery<T, TView>(
        //            $"select * from (select *,row_number() over (order by {string.Join(",", orderBys)}) as RowId from ({sql}) as s) as t where RowId between {pageSize * (pageIndex - 1) + 1} and {pageSize * pageIndex} order by {string.Join(",", orderBys)}")
        //        .ToList();
        //    if (eachAction != null)
        //    {
        //        jsonResults = jsonResults.Each(eachAction).ToList();
        //    }

        //    return new PaginationResult(true, string.Empty, jsonResults)
        //    {
        //        pageIndex = pageIndex,
        //        pageSize = pageSize,
        //        total = total
        //    };
        //}




        public virtual void BulkInsert<T>(IList<T> entities, string destinationTableName = null) where T : class
        {
            if (!Database.IsSqlServer() && !Database.IsMySql())
                throw new NotSupportedException("This method only supports for SQL Server or MySql.");
        }

        public virtual IEnumerable<TView> ExecuteQuery<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new()
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.OpenAsync();
            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);
                var reader = dbCommand.ExecuteReader();
                List<TView> list = new List<TView>();
                Type t = typeof(TView);
                PropertyInfo[] properties = t.GetProperties();
                while (reader.Read())
                {
                    TView val = new TView();
                    if (t.IsValueType)
                    {
                        list.Add((TView)reader[0]);
                    }
                    else
                    {
                        PropertyInfo[] array = properties.Where(x => x.CanWrite == true).ToArray();
                        foreach (PropertyInfo propertyInfo in array)
                        {
                            try
                            {
                                object obj = reader[propertyInfo.Name];
                                if (obj == DBNull.Value)
                                {
                                    propertyInfo.SetValue(val, null);
                                }
                                else
                                {
                                    propertyInfo.SetValue(val, obj);
                                }
                            }
                            catch (Exception)
                            {
                                propertyInfo.SetValue(val, null);
                            }
                        }
                        list.Add(val);
                    }

                }
                reader.Close();//关闭DataReader对象  
                dbConnection.Close();
                return list;
            }
        }
        public virtual async Task<IEnumerable<TView>> ExecuteQueryAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new()
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();
            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);
                var reader = await dbCommand.ExecuteReaderAsync();
                List<TView> list = new List<TView>();
                Type t = typeof(TView);
                PropertyInfo[] properties = t.GetProperties();
                while (await reader.ReadAsync())
                {
                    TView val = new TView();

                    if (t.IsValueType)
                    {
                        list.Add((TView)reader[0]);
                    }
                    else
                    {
                        string tempName = "";
                        PropertyInfo[] array = properties.Where(x => x.CanWrite == true).ToArray();
                        foreach (PropertyInfo propertyInfo in array)
                        {


                            try
                            {
                                object obj = reader[propertyInfo.Name];
                            if (obj == DBNull.Value)
                            {
                                propertyInfo.SetValue(val, null);
                            } else if (propertyInfo.PropertyType == typeof(Boolean)) {

                                propertyInfo.SetValue(val, Convert.ToBoolean(obj) );
                            }
                            else
                            {
                                propertyInfo.SetValue(val,obj);
                            }
                            }
                            catch (Exception)
                            {
                                propertyInfo.SetValue(val, null);
                            }
                        }
                        list.Add(val);
                    }

                }
                await reader.CloseAsync();//关闭DataReader对象  
                await dbConnection.CloseAsync();
                return list;
            }
        }

        public virtual async Task<TView> ExecuteScalarAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();
            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);
                var reader = await dbCommand.ExecuteScalarAsync();

                await dbConnection.CloseAsync();
                return (TView)reader;
            }
        }

        public virtual TView ExecuteScalar<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();
            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);
                var reader = dbCommand.ExecuteScalar();

                dbConnection.Close();
                return (TView)reader;
            }
        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);

                //DbDataAdapter sqlAdp =  DbDataAdapter.;
                var reader = dbCommand.ExecuteReader();
                DataTable dt = new DataTable();
                dt.Load(reader);

                reader.Close();//关闭DataReader对象  
                dbConnection.Close();
                return dt;
            }
        }

        public virtual async Task<DataTable> GetDataTableAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();

            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);

                var reader = await dbCommand.ExecuteReaderAsync();
                DataTable dt = new DataTable();
                dt.Load(reader);
                await reader.CloseAsync();//关闭DataReader对象  
                await dbConnection.CloseAsync();
                return dt;
            }
        }






        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual int ExecuteNonQuery(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                dbConnection.Open();

            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);

                var row = dbCommand.ExecuteNonQuery();
                dbConnection.Close();
                return row;
            }
        }
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public virtual async Task<int> ExecuteNonQueryAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters)
        {
            var dbConnection = Database.GetDbConnection();
            if (dbConnection.State != ConnectionState.Open)
                await dbConnection.OpenAsync();
            using (DbCommand dbCommand = dbConnection.CreateCommand())
            {
                dbCommand.CommandText = sql;
                dbCommand.CommandTimeout = cmdTimeout;
                dbCommand.Parameters.AddRange(parameters);

                var row = await dbCommand.ExecuteNonQueryAsync();

                await dbConnection.CloseAsync();
                return row;
            }
        }



        #endregion






        private void MappingEntityTypes(ModelBuilder modelBuilder)
        {
            List<Type> list = (Assembly.Load(Option.ModelAssemblyName)?.GetTypes())?.Where((Type t) => t.IsClass && !t.IsGenericType && !t.IsAbstract && t.GetInterfaces().Any((Type m) => m.IsAssignableFrom(typeof(BaseModel<>)))).ToList();
            if (list != null && list.Any())
            {
                list.ForEach(delegate (Type t)
                {
                    if (modelBuilder.Model.FindEntityType(t) == null)
                    {
                        modelBuilder.Model.AddEntityType(t);
                    }

                    var types = Assembly.GetExecutingAssembly().GetTypes()
                       .Where(t => t.GetInterfaces().Contains(t))
                      .ToList();

                    foreach (var type in types)
                    {
                        var method = typeof(ModelBuilder).GetMethod("Entity", new Type[] { });
                        method = method.MakeGenericMethod(type);
                        var entity = method.Invoke(modelBuilder, new object[] { });

                        var properties = type.GetProperties();
                        foreach (var property in properties)
                        {
                            if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
                            {
                                var genericArguments = property.PropertyType.GetGenericArguments();
                                if (genericArguments.Length == 1 && types.Contains(genericArguments[0]))
                                {
                                    var hasManyMethod = entity.GetType().GetMethod("HasMany", new Type[] { typeof(string) });
                                    var withManyMethod = hasManyMethod.Invoke(entity, new object[] { property.Name }).GetType().GetMethod("WithMany", new Type[] { typeof(string) });

                                    withManyMethod.Invoke(hasManyMethod, new object[] { genericArguments[0].Name });
                                }
                            }
                        }
                    }
                });
            }


         

        }
    }
}