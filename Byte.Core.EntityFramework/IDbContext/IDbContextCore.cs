using Byte.Core.Common.IoC;
using Byte.Core.EntityFramework.Models;
using Byte.Core.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.Core.EntityFramework.IDbContext
{
    public interface IDbContextCore : ITransientDependency, IDisposable
    {
        DbContextOption Option { get; set; }
        DatabaseFacade GetDatabase();

        IDbContextTransaction BeginTransaction();
        Task<IDbContextTransaction> BeginTransactionAsync();

        void BulkInsert<T>(IList<T> entities, string destinationTableName = null) where T : class;

        bool EnsureCreated();
        Task<bool> EnsureCreatedAsync();


        #region EF 
        #region 添加
        /// <summary>
        /// 添加单条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="withTrigger"></param>
        /// <returns></returns>
        int Add<T>(T entity, bool withTrigger = false) where T : class;
        Task<int> AddAsync<T>(T entity, bool withTrigger = false) where T : class;
        /// <summary>
        /// 添加多条数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        /// <param name="withTrigger"></param>
        /// <returns></returns>
        int AddRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class;

        Task<int> AddRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class;
        #endregion
        #region 总数
        /// <summary>
        /// 总数
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        int Count<T>(Expression<Func<T, bool>> where = null) where T : class;

        Task<int> CountAsync<T>(Expression<Func<T, bool>> where = null) where T : class;
        #endregion
        #region 删除
        int Delete<T, TKey>(TKey key, bool withTrigger = false) where T : class;


        Task<int> DeleteAsync<T, TKey>(TKey key, bool withTrigger = false) where T : class;
        int DeleteRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class;
        Task<int> DeleteRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class;
        /// <summary>
        /// 条件删除
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="where"></param>
        /// <returns></returns>
        int Delete<T>(Expression<Func<T, bool>> where, bool withTrigger = false) where T : class;

        Task<int> DeleteAsync<T>(Expression<Func<T, bool>> where, bool withTrigger = false) where T : class;
        #endregion
        #region 修改

        void AttachUpdate<T, TKey>(T entity) where T : class, IBaseModel<TKey>;
        Task AttachUpdateAsync<T, TKey>(T entity) where T : class, IBaseModel<TKey>;
        int Update<T, TKey>(T entity, bool withTrigger = false) where T : class, IBaseModel<TKey>;
        Task<int> UpdateAsync<T, TKey>(T entity, bool withTrigger = false) where T : class, IBaseModel<TKey>;


        int Update<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class;

        Task<int> UpdateAsync<T>(T model, bool withTrigger = false, params string[] updateColumns) where T : class;


        int Update<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) where T : class;
        Task<int> UpdateAsync<T>(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool withTrigger = false) where T : class;
        int UpdateRange<T>(ICollection<T> entities, bool withTrigger = false) where T : class;
        Task<int> UpdateRangeAsync<T>(ICollection<T> entities, bool withTrigger = false) where T : class;
        int UpdateRange<T>(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) where T : class;
        Task<int> UpdateRangeAsync<T>(ICollection<T> entities, bool withTrigger = false, params string[] updateColumns) where T : class;

        int UpdateRange<T>(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) where T : class;
        Task<int> UpdateRangeAsync<T>(ICollection<T> entities, Expression<Func<T, object>> updateColumns, bool withTrigger = false) where T : class;



        #endregion

        #region 存在
        bool Any<T>(Expression<Func<T, bool>> where = null) where T : class;
        Task<bool> AnyAsync<T>(Expression<Func<T, bool>> where = null) where T : class;

        #endregion

        #region 查询第一条
        T Find<T, TKey>(TKey key) where T : class;

        Task<T> FindAsync<T, TKey>(TKey key) where T : class;

        T Find<T>(object key) where T : class;
        Task<T> FindAsync<T>(object key) where T : class;

        #endregion
        IQueryable<T> GetIQueryable<T>(Expression<Func<T, bool>> where = null, bool asNoTracking = false) where T : class;


        IQueryable<T> FilterWithInclude<T>(Func<IQueryable<T>, IQueryable<T>> include, Expression<Func<T, bool>> where) where T : class;


        List<IEntityType> GetAllEntityTypes();
        DbSet<T> GetDbSet<T>() where T : class;
        T GetSingleOrDefault<T>(Expression<Func<T, bool>> where = null) where T : class;
        Task<T> GetSingleOrDefaultAsync<T>(Expression<Func<T, bool>> where = null) where T : class;
        T GetByCompileQuery<T, TKey>(TKey id) where T : BaseModel<TKey>;
        Task<T> GetByCompileQueryAsync<T, TKey>(TKey id) where T : BaseModel<TKey>;


        IList<T> GetByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class;
        Task<List<T>> GetByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        T FirstOrDefaultByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class;
        Task<T> FirstOrDefaultByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        T FirstOrDefaultWithTrackingByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class;
        Task<T> FirstOrDefaultWithTrackingByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        int CountByCompileQuery<T>(Expression<Func<T, bool>> filter) where T : class;
        Task<int> CountByCompileQueryAsync<T>(Expression<Func<T, bool>> filter) where T : class;
        #endregion
        #region Sql


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

        #region  SaveChange
        int SaveChanges();

        int SaveChanges(bool acceptAllChangesOnSuccess);

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken));


        #endregion
    }
}