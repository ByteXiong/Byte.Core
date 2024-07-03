using SqlSugar;
using System.Linq.Expressions;

namespace Byte.Core.SqlSugar;

/// <summary>
/// sqlSugar接口
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IRepository<T> where T : class
{
    ISqlSugarClient SugarClient { get; }

    #region 查询操作

    /// <summary>
    /// 获取IQueryable
    /// </summary>
    /// <param name="where"></param>
    /// <param name="needAuth">是否鉴权</param>
    /// <returns></returns>
    ISugarQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null, bool needAuth = true);

    #endregion

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

    //#region 新增操作

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <returns>受影响行数</returns>
    //Task<int> AddAsync(T entity);

    ///// <summary>
    ///// 批量新增实体
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <returns>受影响行数</returns>
    //Task<int> AddAsync(List<T> entitys);

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="keyValues">键：字段名称，值：字段值</param>
    ///// <returns>受影响行数</returns>
    //Task<int> AddAsync(Dictionary<string, object> keyValues);

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <returns>返回当前实体</returns>
    //Task<T> AddReturnEntityAsync(T entity);

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <returns>自增ID</returns>
    //Task<int> AddReturnIdentityAsync(T entity);


    //#endregion

    //#region 更新操作

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateAsync(T entity, List<string> lstIgnoreColumns = null, bool isLock = true);

    ///// <summary>
    ///// 批量更新实体
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateAsync(List<T> entitys, List<string> lstIgnoreColumns = null, bool isLock = true);

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateAsync(T entity, Expression<Func<T, bool>> where,
    //    List<string> lstIgnoreColumns = null,
    //    bool isLock = true);

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="update">实体对象</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateAsync(Expression<Func<T, T>> update, Expression<Func<T, bool>> where = null,
    //    bool isLock = true);

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="keyValues">键:字段名称 值：值</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateAsync(Dictionary<string, object> keyValues, Expression<Func<T, bool>> where = null,
    //    bool isLock = true);

    ///// <summary>
    ///// 批量更新实体列
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <param name="updateColumns">要更新的列</param>
    ///// <param name="wherecolumns">条件列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateColumnsAsync(List<T> entitys, Expression<Func<T, object>> updateColumns,
    //    Expression<Func<T, object>> wherecolumns = null, bool isLock = true);

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateRowVerAsync(T entity, List<string> lstIgnoreColumns = null, bool isLock = true);

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="update">实体对象</param>
    ///// <param name="where">键:字段名称 值:值</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> UpdateRowVerAsync(Expression<Func<T, T>> update, Dictionary<string, object> where,
    //    bool isLock = true);

    //#endregion

    //#region 删除操作

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="id">主键ID</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<bool> DeleteByPrimaryAsync(object id, bool isLock = true);

    ///// <summary>
    ///// 批量删除实体
    ///// </summary>
    ///// <param name="primaryKeyValues">主键ID集合</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> DeleteByPrimaryAsync(List<object> primaryKeyValues, bool isLock = true);

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> DeleteAsync(T entity, bool isLock = true);

    ///// <summary>
    ///// 批量删除实体
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> DeleteAsync(List<T> entitys, bool isLock = true);

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> DeleteAsync(Expression<Func<T, bool>> whereLambda, bool isLock = true);

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="inValues">主键集合</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //Task<int> DeleteInAsync(List<dynamic> inValues, bool isLock = true);

    //#endregion

    //#region 单表查询

    ///// <summary>
    ///// 查询单个
    ///// </summary>
    ///// <param name="expression">返回表达式</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <typeparam name="TResult">返回对象</typeparam>
    ///// <returns>自定义数据</returns>
    //Task<TResult> QueryAsync<TResult>(Expression<Func<T, TResult>> expression,
    //    Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="expression">返回表达式</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <typeparam name="TResult">返回对象</typeparam>
    ///// <returns>自定义数据</returns>
    //Task<List<TResult>> QueryListExpAsync<TResult>(Expression<Func<T, TResult>> expression,
    //    Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 查询单个
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>实体对象</returns>
    //Task<T> QueryFirstAsync(Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="orderFileds"></param>
    ///// <param name="orderByType"></param>
    ///// <returns>实体列表</returns>
    //Task<List<T>> QueryListAsync(Expression<Func<T, bool>> whereLambda = null,
    //    Expression<Func<T, object>> orderFileds = null, OrderByType orderByType = OrderByType.Desc);


    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="sql">SQL</param>
    ///// <returns>实体列表</returns>
    //Task<List<T>> QuerySqlListAsync(string sql);

    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression">查询表达式</param>
    ///// /// <param name="isSplitTable">是否分表</param>
    ///// <returns></returns>
    //Task<List<T>> QueryPageListAsync(Expression<Func<T, bool>> whereLambda, Pagination pagination,
    //    Expression<Func<T, T>> selectExpression = null, bool isSplitTable = false);

    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression"></param>
    ///// <param name="navigationExpression">导航属性</param>
    ///// <param name="navigationExpression2"></param>
    ///// <param name="navigationExpression3"></param>
    ///// <returns></returns>
    //Task<List<T>> QueryPageListAsync<T, T2, T3>(Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination,
    //    Expression<Func<T, T>> selectExpression = null,
    //    Expression<Func<T, T>> navigationExpression = null,
    //    Expression<Func<T, List<T2>>> navigationExpression2 = null,
    //    Expression<Func<T, List<T3>>> navigationExpression3 = null);


    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression"></param>
    ///// <param name="navigationExpression">导航属性</param>
    ///// <param name="navigationExpression2"></param>
    ///// <param name="navigationExpression3"></param>
    ///// <param name="navigationExpression4"></param>
    ///// <returns></returns>
    //Task<List<T>> QueryPageListAsync<T, T2, T3, T4>(Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination,
    //    Expression<Func<T, T>> selectExpression = null,
    //    Expression<Func<T, T>> navigationExpression = null,
    //    Expression<Func<T, List<T2>>> navigationExpression2 = null,
    //    Expression<Func<T, List<T3>>> navigationExpression3 = null,
    //    Expression<Func<T, List<T4>>> navigationExpression4 = null);

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="inFieldName">指定字段名</param>
    ///// <param name="inValues">值</param>
    ///// <returns>实体列表</returns>
    //Task<List<T>> QueryListInAsync(string inFieldName, List<dynamic> inValues);

    ///// <summary>
    ///// 查询单个对象
    ///// </summary>
    ///// <param name="key">列值</param>
    ///// <param name="columnName">列名 默认ID</param>
    ///// <returns>实体对象</returns>
    //Task<T> QuerySingleAsync(object key, string columnName = "id");

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="values">主键集合</param>
    ///// <param name="columnName">列名 默认ID</param>
    ///// <returns>实体列表</returns>
    //Task<List<T>> QueryListInAsync(List<long> values, string columnName = "id");

    ///// <summary>
    ///// DataTable数据源
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>DataTable</returns>
    //Task<DataTable> QueryDataTableAsync(Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// DataTable数据源
    ///// </summary>
    ///// <param name="sql">SQL</param>
    ///// <returns>DataTable</returns>
    //Task<DataTable> QueryDataTableAsync(string sql);

    ///// <summary>
    ///// Object
    ///// </summary> 
    ///// <param name="sql">SQL</param> 
    ///// <returns>Object</returns>
    //Task<object> QuerySqlScalarAsync(string sql);

    ///// <summary>
    ///// 查询单个对象
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>对象.json</returns>
    //Task<string> QueryJsonAsync(Expression<Func<T, bool>> whereLambda = null);

    //#endregion

    //#region 多表联查 最大支持16个表

    //Task<List<TResult>> QueryMuchAsync<T, T2, TResult>(
    //    Expression<Func<T, T2, object[]>> joinExpression,
    //    Expression<Func<T, T2, TResult>> selectExpression,
    //    Expression<Func<T, T2, bool>> whereLambda = null, Expression<Func<T, T2, object>> groupExpression = null,
    //    string sortField = "");


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, TResult>(
    //    Expression<Func<T, T2, T3, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, object>> groupExpression = null);


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, TResult>(
    //    Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, object>> groupExpression = null);


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null);


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null);


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null);


    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null);

    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object>> groupExpression = null);

    //Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> groupExpression = null);

    //#endregion

    //#region 多表联查分页  最大支持16个表

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, object[]>> joinExpression,
    //    Expression<Func<T, T2, TResult>> selectExpression,
    //    Expression<Func<T, T2, bool>> whereLambda = null,
    //    Expression<Func<T, T2, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null) where T : class, new();

    //Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null) where T : class, new();

    //#endregion

    //#region 一对一 一对多查询

    ///// <summary>
    ///// 一对一 一对多查询
    ///// </summary>
    ///// <param name="mapperAction">委托方法体</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> QueryMapperAsync(Action<T> mapperAction,
    //    Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 一对一 一对多查询
    ///// </summary>
    ///// <param name="mapperAction">委托方法体</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination"></param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> QueryMapperPageListAsync(Action<T> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, Pagination pagination);

    ///// <summary>
    ///// 一对一 一对多查询
    ///// </summary>
    ///// <param name="mapperField"></param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="mapperObject"></param>
    ///// <param name="pagination"></param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> QueryMapperPageListAsync<TObject>(Expression<Func<T, List<TObject>>> mapperObject,
    //    Expression<Func<T, object>> mapperField,
    //    Expression<Func<T, bool>> whereLambda, Pagination pagination);

    ///// <summary>
    ///// 一对一 一对多查询
    ///// </summary>
    ///// <param name="mapperAction">委托方法体</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="sortField"></param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> QueryMapperAsync(Action<T, MapperCache<T>> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, string sortField = "");

    ///// <summary>
    ///// 一对一 一对多查询
    ///// </summary>
    ///// <param name="mapperAction">委托方法体</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> QueryMapperPageListAsync(Action<T, MapperCache<T>> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, Pagination pagination);

    //#endregion

    //#region 存储过程

    ///// <summary>
    ///// 执行存储过程DataSet
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>DataSet</returns>
    //Task<DataSet> QueryProcedureDataSetAsync(string procedureName, List<SugarParameter> parameters);

    ///// <summary>
    ///// 执行存储过程DataTable
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>DataTable</returns>
    //Task<DataTable> QueryProcedureAsync(string procedureName, List<SugarParameter> parameters);

    ///// <summary>
    ///// 执行存储过程Object
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>Object</returns>
    //Task<object> QueryProcedureScalarAsync(string procedureName, List<SugarParameter> parameters);

    //#endregion

    //#region 常用函数

    ///// <summary>
    ///// 查询前面几条
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="topNum">要多少条</param>
    ///// <returns>泛型对象集合</returns>
    //Task<List<T>> TakeAsync(int topNum, Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 对象是否存在
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>True or False</returns>
    //Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda = null);

    ///// <summary>
    ///// 总和
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <returns>总和</returns>
    //Task<int> SumAsync(string field);

    ///// <summary>
    ///// 最大值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <typeparam name="TResult">泛型结果</typeparam>
    ///// <returns>最大值</returns>
    //Task<TResult> MaxAsync<TResult>(string field);

    ///// <summary>
    ///// 最小值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <typeparam name="TResult">泛型结果</typeparam>
    ///// <returns>最小值</returns>
    //Task<TResult> MinAsync<TResult>(string field);

    ///// <summary>
    ///// 平均值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <returns>平均值</returns>
    //Task<int> AvgAsync(string field);

    //#endregion

    //#region 流水号

    ///// <summary>
    ///// 生成流水号
    ///// </summary>
    ///// <param name="key">列名</param>
    ///// <param name="prefix">前缀</param>
    ///// <param name="fixedLength">流水号长度</param>
    ///// <param name="dateFomart">日期格式(yyyyMMdd) 为空前缀后不加日期,反之加</param>
    ///// <returns></returns>
    //Task<string> CustomNumberAsync(string key, string prefix = "", int fixedLength = 4, string dateFomart = "");

    ///// <summary>
    ///// 生成流水号
    ///// </summary>
    ///// <param name="key">列名</param>
    ///// <param name="num">数量</param>
    ///// <param name="prefix">前缀</param>
    ///// <param name="fixedLength">流水号长度</param>
    ///// <param name="dateFomart">日期格式(yyyyMMdd) 为空前缀后不加日期,反之加</param>
    ///// <returns></returns>
    //Task<List<string>> CustomNumberAsync(string key, int num, string prefix = "", int fixedLength = 4,
    //    string dateFomart = "");

    //#endregion
}
