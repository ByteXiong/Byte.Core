using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.Core.EntityFramework.IDbContext
{
    public abstract class BaseRepository<T, TKey> : IRepository<T, TKey> where T : BaseModel<TKey>
    {


        protected readonly IDbContextCore DbContext;
        private bool _disposedValue = true;
        protected DbSet<T> DbSet => DbContext.GetDbSet<T>();
        //[FromContainer]
        //public PrincipalUser CurrentUser {get; set;  }


        protected BaseRepository(IDbContextCore dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            DbContext.EnsureCreated();
        }

        #region Insert

        public virtual int Add(T entity, bool withTrigger = false) => DbContext.Add(entity, withTrigger);

        public virtual Task<int> AddAsync(T entity, bool withTrigger = false) => DbContext.AddAsync(entity, withTrigger);

        public virtual int AddRange(ICollection<T> entities, bool withTrigger = false) => DbContext.AddRange(entities, withTrigger);


        public virtual Task<int> AddRangeAsync(ICollection<T> entities, bool withTrigger = false) => DbContext.AddRangeAsync(entities, withTrigger);


        public virtual void BulkInsert(IList<T> entities, string destinationTableName = null) => DbContext.BulkInsert<T>(entities, destinationTableName);




        #endregion

        #region Update

        public virtual void AttachUpdate(T entity) => DbContext.AttachUpdate<T, TKey>(entity);
        public virtual Task AttachUpdateAsync(T entity) => DbContext.AttachUpdateAsync<T, TKey>(entity);

        public virtual int Update(T entity, bool withTrigger = false) => DbContext.Update<T, TKey>(entity, withTrigger);

        public virtual Task<int> UpdateAsync(T entity, bool withTrigger = false) => DbContext.UpdateAsync<T, TKey>(entity, withTrigger);



        public virtual int Update(T model, bool withTrigger = false, params string[] updateColumns) => DbContext.Update(model, withTrigger, updateColumns);


        public virtual Task<int> UpdateAsync(T model, bool withTrigger = false, params string[] updateColumns) => DbContext.UpdateAsync(model, withTrigger, updateColumns);



        public virtual int Update(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) => DbContext.Update(where, updateFactory);

        public virtual Task<int> UpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) => DbContext.UpdateAsync(where, updateFactory);

        public virtual int UpdateRange(ICollection<T> entities, bool withTrigger = false) => DbContext.UpdateRange(entities, withTrigger);

        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false) => DbContext.UpdateRangeAsync(entities, withTrigger);

        public virtual int UpdateRange(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) => DbContext.UpdateRange(entities, withTrigger, updateColumns);
        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) => DbContext.UpdateRangeAsync(entities, withTrigger, updateColumns);

        public virtual int UpdateRange(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) => DbContext.UpdateRange(entities, updateColumns, withTrigger);
        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) => DbContext.UpdateRangeAsync(entities, updateColumns, withTrigger);

        #endregion

        #region Delete

        public virtual int Delete(TKey key, bool withTrigger = false) => DbContext.Delete<T, TKey>(key, withTrigger);

        public virtual Task<int> DeleteAsync(TKey key, bool withTrigger = false) => DbContext.DeleteAsync<T, TKey>(key, withTrigger);

        public virtual int Delete(Expression<Func<T, bool>> @where, bool withTrigger = false) => DbContext.Delete(where, withTrigger);


        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> @where, bool withTrigger = false) => DbContext.DeleteAsync(where, withTrigger);

        public virtual int DeleteRange(ICollection<T> entities, bool withTrigger = false) => DbContext.DeleteRange<T>(entities);

        public virtual Task<int> DeleteRangeAsync(ICollection<T> entities, bool withTrigger = false) => DbContext.DeleteRangeAsync<T>(entities);

        #endregion

        #region SaveChanges
        public int SaveChanges() => DbContext.SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess) => DbContext.SaveChanges(acceptAllChangesOnSuccess);
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => DbContext.SaveChangesAsync(cancellationToken);
        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken)) => DbContext.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        #endregion
        #region Query

        public virtual int Count(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.Count(where);
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.CountAsync(where);
        }


        public virtual bool Any(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.Any(where);
        }
        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.AnyAsync(where);
        }
        public virtual IQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false)
        {
            return DbContext.GetIQueryable(where, asNoTracking);
        }
        public IQueryable<T> Queryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false)
        {
            return DbContext.GetIQueryable(where, asNoTracking);
        }
        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetById(TKey key)
        {
            return DbContext.Find<T, TKey>(key);
        }
        public virtual T GetById(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            if (includeFunc == null) return GetById(key);
            return includeFunc(DbSet.Where(m => m.Id.Equal(key))).AsNoTracking().FirstOrDefault();
        }
        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Task<T> GetByIdAsync(TKey key)
        {
            return DbContext.FindAsync<T, TKey>(key);
        }
        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.GetSingleOrDefault(@where);
        }
        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.GetSingleOrDefaultAsync(where);
        }
        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual IList<T> Get(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.GetByCompileQuery(where);
        }
        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> @where = null)
        {
            return DbContext.GetByCompileQueryAsync(where);
        }
        #endregion
        #region 分页 
        public virtual PagedResults<TOut> PageQuery<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null)
        {
            return GetIQueryable(where).ToPagedResults(queryParam, selectFunc);
        }
        public virtual Task<PagedResults<TOut>> PageQueryAsync<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null)
        {
            return GetIQueryable(where).ToPagedResultsAsync(queryParam, selectFunc);
        }
        #endregion
        #region  sql语句



        public IEnumerable<TView> ExecuteQuery<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new() => DbContext.ExecuteQuery<TView>(sql, cmdTimeout, parameters);

        public Task<IEnumerable<TView>> ExecuteQueryAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new() => DbContext.ExecuteQueryAsync<TView>(sql, cmdTimeout, parameters);

        public TView ExecuteScalar<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.ExecuteScalar<TView>(sql, cmdTimeout, parameters);
        public Task<TView> ExecuteScalarAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.ExecuteScalarAsync<TView>(sql, cmdTimeout, parameters);


        public DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.GetDataTable(sql, cmdTimeout, parameters);
        public Task<DataTable> GetDataTableAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.GetDataTableAsync(sql, cmdTimeout, parameters);

        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.ExecuteNonQuery(sql, cmdTimeout, parameters);
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<int> ExecuteNonQueryAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => DbContext.ExecuteNonQueryAsync(sql, cmdTimeout, parameters);
        #endregion

        public Type ElementType => DbSet.AsQueryable().ElementType;

        public Expression Expression => DbSet.AsQueryable().Expression;


        public IQueryProvider Provider => DbSet.AsQueryable().Provider;




        public virtual TModel ConvertToModel<TModel>(T entity) where TModel : class, new() => MapperHelper<T, TModel>.Map(entity);


        #region 事务
        /// <summary>
        /// 事务（同步）
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction() => DbContext.BeginTransaction();
        /// <summary>
        /// 事务（异步）
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync() => DbContext.BeginTransactionAsync();

        #endregion





        public IEnumerator<T> GetEnumerator()
        {
            return DbSet.AsQueryable().GetEnumerator();
        }

        //IEnumerator<T> IEnumerable.GetEnumerator()
        //{
        //    return GetEnumerator<T>();
        //}

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposedValue)
            {
                if (disposing)
                {
                    DbContext?.Dispose();
                }

                _disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }





    }
}

