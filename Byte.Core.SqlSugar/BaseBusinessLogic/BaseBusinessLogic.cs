using Byte.Core.Common.IoC;
using Byte.Core.Common.Pager;
using Byte.Core.SqlSugar.Repository;
using SqlSugar;
using System.Linq.Expressions;

namespace Byte.Core.SqlSugar.BusinessLogics
{
    public abstract class BaseBusinessLogic<TEntity, TRepository> : IBusinessLogic<TEntity> where TEntity : class, new()
          where TRepository : IRepository<TEntity>
    {
        #region 字段

        /// <summary>
        /// 当前操作对象仓储
        /// </summary>
        public IRepository<TEntity> SugarRepository { get; set; }

        /// <summary>
        /// sugarClient
        /// </summary>
        public ISqlSugarClient SugarClient => SugarRepository.SugarClient;

        #endregion



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

        #region 查询操作

        /// <summary>
        /// 获取IQueryable
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public  ISugarQueryable<TEntity> GetIQueryable(Expression<Func<TEntity, bool>> where = null)=> Repository.GetIQueryable(where);
        #endregion
        #region 新增操作

        /// <summary>
        /// 新增实体
        /// </summary>
        /// <param name="entity">实体对象</param>
        /// <returns>受影响行数</returns>
        public   Task<int> AddAsync(TEntity entity) => Repository.AddAsync(entity);

        #endregion


        #region 更新操作
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="where"></param>
        /// <param name="updateFactory"></param>
        /// <param name="isLock"></param>
        /// <returns></returns>
        public   Task<int> UpdateAsync(Expression<Func<TEntity, bool>> where, Expression<Func<TEntity, TEntity>> updateFactory, bool isLock = true)=> Repository.UpdateAsync(where, updateFactory, isLock);
        #endregion

#region 删除操作

/// <summary>
/// 删除实体
/// </summary>
/// <param name="id">主键ID</param>
/// <param name="isLock">是否加锁</param>
/// <returns>受影响行数</returns>
public Task<int> DeleteAsync<TKey>(TKey id, bool isLock = true) => Repository.DeleteAsync(id, isLock);

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
public Task<int> DeleteAsync(Expression<Func<TEntity, bool>> where, bool isLock = true) => Repository.DeleteAsync(where, isLock);

/// <summary>
/// 删除实体
/// </summary>
/// <param name=",">主键集合</param>
/// <param name="isLock">是否加锁</param>
/// <returns>受影响行数</returns>
public Task<int> DeleteAsync<Tkey>(Tkey[] ids, bool isLock = true) => Repository.DeleteAsync(ids, isLock);

        #endregion
    }
}