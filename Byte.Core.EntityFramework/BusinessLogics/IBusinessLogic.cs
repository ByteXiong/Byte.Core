using Byte.Core.Common.IoC;
using Byte.Core.Common.Pager;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Pager;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.Code.BusinessLogics
{
    public interface IBusinessLogic<T, TKey> : ITransientDependency, IDisposable where T : IBaseModel<TKey>
    {

        #region Insert

        int Add(T entity, bool withTrigger = false);
        Task<int> AddAsync(T entity, bool withTrigger = false);
        int AddRange(ICollection<T> entities, bool withTrigger = false);
        Task<int> AddRangeAsync(ICollection<T> entities, bool withTrigger = false);
        void BulkInsert(IList<T> entities, string destinationTableName = null);


        #endregion

        #region Update
        void AttachUpdate(T entity);
        Task AttachUpdateAsync(T entity);
        int Update(T entity, bool withTrigger = false);
        Task<int> UpdateAsync(T entity, bool withTrigger = false);

        int Update(T model, bool withTrigger = false, params string[] updateColumns);
        Task<int> UpdateAsync(T model, bool withTrigger = false, params string[] updateColumns);

        int Update(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false);
        Task<int> UpdateAsync(Expression<Func<T, bool>> @where, Expression<Func<T, T>> updateFactory, bool withTrigger = false);
        int UpdateRange(ICollection<T> entities, bool withTrigger = false);
        Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false);
        int UpdateRange(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns);
        Task<int> UpdateRangeAsync(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns);

        int UpdateRange(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false);
        Task<int> UpdateRangeAsync(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false);
        #endregion

        #region Delete

        int Delete(TKey key, bool withTrigger = false);
        Task<int> DeleteAsync(TKey key, bool withTrigger = false);
        int Delete(Expression<Func<T, bool>> @where, bool withTrigger = false);
        Task<int> DeleteAsync(Expression<Func<T, bool>> @where, bool withTrigger = false);
        int DeleteRange(ICollection<T> entities, bool withTrigger = false);
        Task<int> DeleteRangeAsync(ICollection<T> entities, bool withTrigger = false);

        #endregion
        #region SaveChangesUpdate
         int SaveChanges() ;
         int SaveChanges(bool acceptAllChangesOnSuccess) ;
         Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
         Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));

        #endregion

        #region Query

        int Count(Expression<Func<T, bool>> @where = null);
        Task<int> CountAsync(Expression<Func<T, bool>> @where = null);
        bool Any(Expression<Func<T, bool>> @where = null);
        Task<bool> AnyAsync(Expression<Func<T, bool>> @where = null);
        IQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false);
        IQueryable<T> Queryable(Expression<Func<T, bool>> where = null, bool asNoTracking = false);
        T GetById(TKey key);
        T GetById(TKey key, Func<IQueryable<T>, IQueryable<T>> includeFunc);
        Task<T> GetByIdAsync(TKey key);
        T GetSingleOrDefault(Expression<Func<T, bool>> @where = null);
        Task<T> GetSingleOrDefaultAsync(Expression<Func<T, bool>> @where = null);
        IList<T> Get(Expression<Func<T, bool>> @where = null);
        Task<List<T>> GetAsync(Expression<Func<T, bool>> @where = null);

        #endregion
        #region sql
        IEnumerable<TView> ExecuteQuery<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new();
        Task<IEnumerable<TView>> ExecuteQueryAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters) where TView : new();

        Task<TView> ExecuteScalarAsync<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        TView ExecuteScalar<TView>(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        DataTable GetDataTable(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        Task<DataTable> GetDataTableAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        int ExecuteNonQuery(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        /// <summary>
        /// 执行SQL语句，返回影响的记录数
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="cmdTimeout"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        Task<int> ExecuteNonQueryAsync(string sql, int cmdTimeout = 30, params DbParameter[] parameters);
        #endregion






        #region 分页

        PagedResults<TOut> PageQuery<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null);
        Task<PagedResults<TOut>> PageQueryAsync<TOut>(PageParam queryParam, Expression<Func<T, bool>> where = null, Func<T, TOut> selectFunc = null);

        #endregion


        #region 事务
        /// <summary>
        /// 事务（同步）
        /// </summary>
        /// <returns></returns>
        IDbContextTransaction BeginTransaction();

        /// <summary>
        /// 事务（异步）
        /// </summary>
        /// <returns></returns>
        Task<IDbContextTransaction> BeginTransactionAsync();
        #endregion
    }
}
