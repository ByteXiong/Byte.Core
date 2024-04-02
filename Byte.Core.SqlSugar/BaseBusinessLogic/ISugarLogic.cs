using Byte.Core.Common.IoC;
using Byte.Core.Common.Pager;
using SqlSugar;
using System.Linq.Expressions;

namespace Byte.Core.SqlSugar.BusinessLogics
{
    public interface ISugarLogic<TEntity> :ITransientDependency where TEntity : class,new()
    {



        /// <summary>
        /// sqlSugarClient
        /// </summary>
        ISqlSugarClient SugarClient { get; }

        /// <summary>
        /// 获取IQueryable
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        ISugarQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>> where = null);
        #region 新增操作

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>受影响行数</returns>
        Task<int> AddAsync(TEntity entity);

        #endregion



        #region 更新操作
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="updateFactory"></param>
        /// <param name="isLock"></param>
        /// <returns></returns>
        Task<int> UpdateAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> updateFactory, bool isLock = true);
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
        //    var del = SugarClient.Deleteable<TEntity>().In(primaryKeyValues);
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
        //public virtual async Task<int> DeleteAsync(TEntity entity, bool isLock = true)
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
        //public virtual async Task<int> DeleteAsync(List<TEntity> entitys, bool isLock = true)
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
        Task<int> DeleteAsync(Expression<Func<TEntity, bool>> where, bool isLock = true);

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
