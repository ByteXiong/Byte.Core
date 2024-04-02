using Byte.Core.Common.IoC;
using Byte.Core.Common.Pager;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Pager;
using Byte.Core.EntityFramework.Repository;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.Code.BusinessLogics
{
    public abstract class BaseBusinessLogic<T, TKey, TRepository> : IBusinessLogic<T, TKey> where T : class, IBaseModel<TKey>
          where TRepository : IRepository<T, TKey>
    {
        //[FromContainer]
        //public PrincipalUser CurrentUser { get; set; }

        public TRepository Repository { get; set; }
        protected BaseBusinessLogic(TRepository repository)
        {
            if (repository != null)
            {
                Repository = repository;
                return;
            }

            Repository = AutofacContainer.Resolve<TRepository>();
            if (Repository != null)
            {
                return;
            }
            throw new ApplicationException("IRepository cannot be null");
        }
        #region Insert

        public virtual int Add(T entity, bool withTrigger = false) => Repository.Add(entity, withTrigger);

        public virtual Task<int> AddAsync(T entity, bool withTrigger = false) => Repository.AddAsync(entity, withTrigger);

        public virtual int AddRange(ICollection<T> entities, bool withTrigger = false) => Repository.AddRange(entities, withTrigger);


        public virtual Task<int> AddRangeAsync(ICollection<T> entities, bool withTrigger = false) => Repository.AddRangeAsync(entities, withTrigger);


        public virtual void BulkInsert(IList<T> entities, string destinationTableName = null) => Repository.BulkInsert(entities, destinationTableName);


        #endregion

        #region Update

        public virtual void AttachUpdate(T entity) => Repository.AttachUpdate(entity);
        public virtual Task AttachUpdateAsync(T entity) => Repository.AttachUpdateAsync(entity);

        public virtual int Update(T entity, bool withTrigger = false) => Repository.Update(entity, withTrigger);

        public virtual Task<int> UpdateAsync(T entity, bool withTrigger = false) => Repository.UpdateAsync(entity, withTrigger);



        public virtual int Update(T model, bool withTrigger = false, params string[] updateColumns) => Repository.Update(model, withTrigger, updateColumns);


        public virtual Task<int> UpdateAsync(T model, bool withTrigger = false, params string[] updateColumns) => Repository.UpdateAsync(model, withTrigger, updateColumns);



        public virtual int Update(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) => Repository.Update(where, updateFactory);

        public virtual Task<int> UpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) => Repository.UpdateAsync(where, updateFactory);
        public virtual int UpdateRange(ICollection<T> entities, bool withTrigger = false) => Repository.UpdateRange(entities, withTrigger);

        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false) => Repository.UpdateRangeAsync(entities, withTrigger);

        public virtual int UpdateRange(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) => Repository.UpdateRange(entities, withTrigger, updateColumns);
        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) => Repository.UpdateRangeAsync(entities, withTrigger, updateColumns);
        public virtual int UpdateRange(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) => Repository.UpdateRange(entities, updateColumns, withTrigger);
        public virtual Task<int> UpdateRangeAsync(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) => Repository.UpdateRangeAsync(entities, updateColumns, withTrigger);


        #endregion

        #region Delete

        public virtual int Delete(TKey key, bool withTrigger = false) => Repository.Delete(key, withTrigger);

        public virtual Task<int> DeleteAsync(TKey key, bool withTrigger = false) => Repository.DeleteAsync(key, withTrigger);

        public virtual int Delete(Expression<Func<T, bool>> @where, bool withTrigger = false) => Repository.Delete(where, withTrigger);


        public virtual Task<int> DeleteAsync(Expression<Func<T, bool>> @where, bool withTrigger = false) => Repository.DeleteAsync(where, withTrigger);

        public virtual int DeleteRange(ICollection<T> entities, bool withTrigger = false) => Repository.DeleteRange(entities, withTrigger);

        public virtual Task<int> DeleteRangeAsync(ICollection<T> entities, bool withTrigger = false) => Repository.DeleteRangeAsync(entities, withTrigger);



        #endregion

        #region SaveChangesUpdate
        public int SaveChanges() => Repository.SaveChanges();
        public int SaveChanges(bool acceptAllChangesOnSuccess) => Repository.SaveChanges(acceptAllChangesOnSuccess);
        public virtual Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken)) => Repository.SaveChangesAsync(cancellationToken);
        public virtual Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken)) => Repository.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);

        #endregion
        #region Query

        public virtual int Count(Expression<Func<T, bool>> @where = null)
        {
            return Repository.Count(where);
        }

        public virtual Task<int> CountAsync(Expression<Func<T, bool>> @where = null)
        {
            return Repository.CountAsync(where);
        }


        public virtual bool Any(Expression<Func<T, bool>> @where = null)
        {
            return Repository.Any(where);
        }

        public virtual Task<bool> AnyAsync(Expression<Func<T, bool>> @where = null)
        {
            return Repository.AnyAsync(where);
        }
        public virtual IQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false)
        {
            return Repository.GetIQueryable(where, asNoTracking);
        }
        public IQueryable<T> Queryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false)
        { 























            return Repository.Queryable(where, asNoTracking);
        }
        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual T GetById(TKey key)
        {
            return Repository.GetById(key);
        }

        public virtual T GetById(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc)
        {
            return Repository.GetById(key, includeFunc);
        }

        /// <summary>
        /// 根据主键获取实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual Task<T> GetByIdAsync(TKey key)
        {
            return Repository.GetByIdAsync(key);
        }

        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual T GetSingleOrDefault(Expression<Func<T, bool>> @where = null)
        {
            return Repository.GetSingleOrDefault(@where);
        }

        /// <summary>
        /// 获取单个实体。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> @where = null)
        {
            return Repository.GetSingleOrDefaultAsync(where);
        }

        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual IList<T> Get(Expression<Func<T, bool>> @where = null)
        {
            return Repository.Get(where);
        }


        /// <summary>
        /// 获取实体列表。建议：如需使用Include和ThenInclude请重载此方法。
        /// </summary>
        public virtual Task<List<T>> GetAsync(Expression<Func<T, bool>> @where = null)
        {
            return Repository.GetAsync(where);
        }






        #endregion
        #region 分页 






        public virtual PagedResults<TOut> PageQuery<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null)
        {
            return Repository.PageQuery(queryParam, where, selectFunc);
        }
        public virtual Task<PagedResults<TOut>> PageQueryAsync<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null)
        {
            return Repository.PageQueryAsync(queryParam, where, selectFunc);
        }

        #endregion


        #region 事务
        /// <summary>
        /// 事务（同步）
        /// </summary>
        /// <returns></returns>
        public IDbContextTransaction BeginTransaction() => Repository.BeginTransaction();

        /// <summary>
        /// 事务（异步）
        /// </summary>
        /// <returns></returns>
        public Task<IDbContextTransaction> BeginTransactionAsync() => Repository.BeginTransactionAsync();
        #endregion
        #region  sql语句
        public IEnumerable<TView> ExecuteQuery<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new() => Repository.ExecuteQuery<TView>(sql, cmdTimeout, parameters);
        public Task<IEnumerable<TView>> ExecuteQueryAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new() => Repository.ExecuteQueryAsync<TView>(sql, cmdTimeout, parameters);


        public TView ExecuteScalar<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.ExecuteScalar<TView>(sql, cmdTimeout, parameters);
        public Task<TView> ExecuteScalarAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.ExecuteScalarAsync<TView>(sql, cmdTimeout, parameters);

        public DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.GetDataTable(sql, cmdTimeout, parameters);
        public Task<DataTable> GetDataTableAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.GetDataTableAsync(sql, cmdTimeout, parameters);
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public int ExecuteNonQuery(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.ExecuteNonQuery(sql, cmdTimeout, parameters);
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public Task<int> ExecuteNonQueryAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters) => Repository.ExecuteNonQueryAsync(sql, cmdTimeout, parameters);

        #endregion

        public void Dispose()
        {
            Repository?.Dispose();
        }
    }
}
