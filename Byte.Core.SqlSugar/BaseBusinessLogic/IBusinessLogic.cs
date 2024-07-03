using Byte.Core.Common.IoC;
using SqlSugar;
using System.Linq.Expressions;

namespace Byte.Core.SqlSugar
{
    public interface IBusinessLogic<T>  where T : class, new()
    {



        /// <summary>
        /// sqlSugarClient
        /// </summary>
        ISqlSugarClient SugarClient { get; }

        /// <summary>
        /// 获取IQueryable
        /// </summary>
        /// <param name="where"></param>
        /// <param name="needAuth">是否鉴权</param>
        /// <returns></returns>
        ISugarQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null,bool needAuth=true);
        #region 新增操作

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>受影响行数</returns>
        Task<int> AddAsync(T entity);
        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entitys">实体对象</param>
        /// <returns>受影响行数</returns>
        Task<int> AddRangeAsync(List<T> entitys);
        #endregion



        #region 更新操作
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <param name="lstIgnoreColumns">忽略列</param>
        /// <param name="isLock">是否加锁</param>
        /// <returns>受影响行数</returns>
        Task<int> UpdateAsync(T entity, Expression<Func<T, object>> lstIgnoreColumns = null, bool isLock = true);
        /// <summary>
        /// 批量更新实体
        /// </summary>
        /// <param name="entitys">实体集合</param>
        /// <param name="lstIgnoreColumns">忽略列</param>
        /// <param name="isLock">是否加锁</param>
        /// <returns>受影响行数</returns>
        Task<int> UpdateRangeAsync(List<T> entitys, Expression<Func<T, object>> lstIgnoreColumns = null, bool isLock = true);
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="updateFactory"></param>
        /// <param name="isLock"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool isLock = true);
        #endregion


        #region 删除操作

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="id">主键ID</param>
        /// <param name="isLock">是否加锁</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync<TKey>(TKey id, bool isLock = true);

        ///// <summary>
        ///// 批量删除实体
        ///// </summary>
        ///// <param name="primaryKeyValues">主键ID集合</param>
        ///// <param name="isLock">是否加锁</param>
        ///// <returns>受影响行数</returns>
        //public virtual async Task<int> DeleteByPrimaryAsync(List<object> primaryKeyValues, bool isLock = true)
        //{
        //    var del = SugarClient.Deleteable<T>().In(primaryKeyValues);
        //    if (isLock)
        //    {
        //        del = del.With(SqlWith.RowLock);
        //    }

        //    return await del.ExecuteCommandAsync();
        //}

        ///// <summary>
        ///// 删除实体
        ///// </summary>
        ///// <param name="entity">实体对象</param>
        ///// <param name="isLock">是否加锁</param>
        ///// <returns>受影响行数</returns>
        //public virtual async Task<int> DeleteAsync(T entity, bool isLock = true)
        //{
        //    var del = SugarClient.Deleteable(entity);
        //    if (isLock)
        //    {
        //        del = del.With(SqlWith.RowLock);
        //    }

        //    return await del.ExecuteCommandAsync();
        //}

        ///// <summary>
        ///// 批量删除实体
        ///// </summary>
        ///// <param name="entitys">实体集合</param>
        ///// <param name="isLock">是否加锁</param>
        ///// <returns>受影响行数</returns>
        //public virtual async Task<int> DeleteAsync(List<T> entitys, bool isLock = true)
        //{
        //    var del = SugarClient.Deleteable(entitys);
        //    if (isLock)
        //    {
        //        del = del.With(SqlWith.RowLock);
        //    }

        //    return await del.ExecuteCommandAsync();
        //}

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="where">条件表达式</param>
        /// <param name="isLock">是否加锁</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync(Expression<Func<T, bool>> where, bool isLock = true);

        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name=",">主键集合</param>
        /// <param name="isLock">是否加锁</param>
        /// <returns>受影响行数</returns>
        Task<int> DeleteAsync<Tkey>(Tkey[] ids, bool isLock = true);
        #endregion
    }
}
