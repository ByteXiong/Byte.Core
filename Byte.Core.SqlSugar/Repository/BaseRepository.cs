using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Common.Pager;
using Byte.Core.SqlSugar.Base;
using Byte.Core.SqlSugar;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Byte.Core.SqlSugar;

/// <summary>
/// SqlSugar仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class BaseRepository<T> : IRepository<T> where T : class, new()
{
    public BaseRepository()
    {
       var  unitOfWork = AutofacContainer.Resolve<IUnitOfWork>();
        var sqlSugarScope = unitOfWork.GetDbClient();
        var tenantAttribute = typeof(T).GetCustomAttribute<TenantAttribute>();
        if (tenantAttribute == null)
        {
            SugarClient = sqlSugarScope;
            return;
        }

        SugarClient = sqlSugarScope.GetConnectionScope(tenantAttribute.configId.ToString());
    }

    public ISqlSugarClient SugarClient { get; }


    #region 查询操作

    /// <summary>
    /// 获取IQueryable
    /// </summary>
    /// <param name="where"></param>
    /// <returns></returns>
    public virtual ISugarQueryable<T> GetIQueryable(Expression<Func<T, bool>> where = null)
    {
        if (where == null)
        { 
            return SugarClient.Queryable<T>();
        }
        return SugarClient.Queryable<T>().Where(where);
    }


    #endregion


    #region 新增操作

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> AddAsync(T entity)
    {
        var insert = SugarClient.Insertable(entity);
        return await insert.ExecuteCommandAsync();
    }

    /// <summary>
    /// 新增实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> AddRangeAsync(List<T> entitys)
    {
        var insert = SugarClient.Insertable(entitys);
        return await insert.ExecuteCommandAsync();
    }

    #endregion

    #region 更新操作

    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="entity">实体对象</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> UpdateAsync(T entity, List<string> lstIgnoreColumns = null, bool isLock = true)
    {
        IUpdateable<T> up = SugarClient.Updateable(entity);
        if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }

    /// <summary>
    /// 批量更新实体
    /// </summary>
    /// <param name="entitys">实体集合</param>
    /// <param name="lstIgnoreColumns">忽略列</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> UpdateRangeAsync(List<T> entitys, List<string> lstIgnoreColumns = null,
        bool isLock = true)
    {
        IUpdateable<T> up = SugarClient.Updateable(entitys);
        if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
        {
            up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;
    }


    /// <summary>
    /// 更新实体
    /// </summary>
    /// <param name="where"></param>
    /// <param name="updateFactory"></param>
    /// <param name="isLock"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<T, bool>> where, Expression<Func<T, T>> updateFactory, bool isLock = true) {

        IUpdateable<T> up = SugarClient.Updateable<T>().SetColumns(updateFactory) ;
        if (where != null)
        {
            up = up.Where(where);
        }

        if (isLock)
        {
            up = up.With(SqlWith.UpdLock);
        }

        var result = await up.ExecuteCommandAsync();
        return result;


    }
    #endregion


    #region 删除操作

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name="id">主键ID</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> DeleteAsync<TKey>(TKey id, bool isLock = true)
    {
        //return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();

        var del = SugarClient.Deleteable<T>(id);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync() ;
    }

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
    public virtual async Task<int> DeleteAsync(Expression<Func<T, bool>> where, bool isLock = true)
    {
        var del = SugarClient.Deleteable<T>().Where(where);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    /// <summary>
    /// 删除实体
    /// </summary>
    /// <param name=",">主键集合</param>
    /// <param name="isLock">是否加锁</param>
    /// <returns>受影响行数</returns>
    public virtual async Task<int> DeleteAsync<Tkey>(Tkey[] ids, bool isLock = true)
    {
        var del = SugarClient.Deleteable<T>().In(ids);
        if (isLock)
        {
            del = del.With(SqlWith.RowLock);
        }

        return await del.ExecuteCommandAsync();
    }

    #endregion

    #region 事务
    #endregion
    //#region 新增操作

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> AddAsync(T entity)
    //{
    //    var insert = SugarClient.Insertable(entity);
    //    return await insert.ExecuteCommandAsync();
    //}

    ///// <summary>
    ///// 批量新增实体
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> AddRange(List<T> entitys)
    //{
    //    return await SugarClient.Insertable(entitys.ToArray()).ExecuteCommandAsync();
    //}

    ///// <summary>
    ///// 新增实体
    ///// </summary>
    ///// <param name="keyValues">键：字段名称，值：字段值</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> AddAsync(Dictionary<string, object> keyValues)
    //{
    //    var result = await SugarClient.Insertable(keyValues).ExecuteCommandAsync();
    //    return result;
    //}

    //#endregion

    //#region 更新操作

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateAsync(T entity, List<string> lstIgnoreColumns = null, bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable(entity);
    //    if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
    //    {
    //        up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
    //    }

    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 批量更新实体
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateAsync(List<T> entitys, List<string> lstIgnoreColumns = null,
    //    bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable(entitys);
    //    if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
    //    {
    //        up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
    //    }

    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateAsync(T entity, Expression<Func<T, bool>> where,
    //    List<string> lstIgnoreColumns = null, bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable(entity);
    //    if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
    //    {
    //        up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
    //    }

    //    up = up.Where(where);
    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="update">实体对象</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateAsync(Expression<Func<T, T>> update,
    //    Expression<Func<T, bool>> where = null, bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable<T>().SetColumns(update);
    //    if (where != null)
    //    {
    //        up = up.Where(where);
    //    }

    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="keyValues">键:字段名称 值：值</param>
    ///// <param name="where">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateAsync(Dictionary<string, object> keyValues,
    //    Expression<Func<T, bool>> where = null, bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable<T>(keyValues);
    //    if (where != null)
    //    {
    //        up = up.Where(where);
    //    }

    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 批量更新实体列
    ///// </summary>
    ///// <param name="entitys">实体集合</param>
    ///// <param name="updateColumns">要更新的列</param>
    ///// <param name="wherecolumns">条件列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateColumnsAsync(List<T> entitys,
    //    Expression<Func<T, object>> updateColumns, Expression<Func<T, object>> wherecolumns = null,
    //    bool isLock = true)
    //{
    //    IUpdateable<T> up = SugarClient.Updateable(entitys).UpdateColumns(updateColumns);
    //    if (wherecolumns != null)
    //    {
    //        up = up.WhereColumns(wherecolumns);
    //    }

    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="entity">实体对象</param>
    ///// <param name="lstIgnoreColumns">忽略列</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateRowVerAsync(T entity, List<string> lstIgnoreColumns = null,
    //    bool isLock = true)
    //{
    //    Type ts = entity.GetType();
    //    var rowVerProperty = ts.GetProperty("RowVer");
    //    if (rowVerProperty == null)
    //    {
    //        throw new Exception("Column RowVer Not Exist");
    //    }

    //    if (rowVerProperty.GetValue(entity, null) == null)
    //    {
    //        throw new Exception("RowVer Value Is Null");
    //    }

    //    var codeProperty = ts.GetProperty("Code");
    //    if (codeProperty == null)
    //    {
    //        throw new Exception("Column Code Not Exist");
    //    }

    //    if (codeProperty.GetValue(entity, null) == null)
    //    {
    //        throw new Exception("Code Value Is Null");
    //    }

    //    var rowVerValue = int.Parse(rowVerProperty.GetValue(entity, null).ToString());
    //    var codeValue = codeProperty.GetValue(entity, null).ToString();
    //    var sqlWhere = $" RowVer={rowVerValue} AND Code='{codeValue}'";
    //    rowVerProperty.SetValue(entity, rowVerValue + 1, null);
    //    IUpdateable<T> up = SugarClient.Updateable(entity);
    //    if (lstIgnoreColumns != null && lstIgnoreColumns.Count > 0)
    //    {
    //        up = up.IgnoreColumns(lstIgnoreColumns.ToArray());
    //    }

    //    up = up.Where(sqlWhere);
    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    ///// <summary>
    ///// 更新实体
    ///// </summary>
    ///// <param name="update">实体对象</param>
    ///// <param name="where">键:字段名称 值:值</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> UpdateRowVerAsync(Expression<Func<T, T>> update,
    //    Dictionary<string, object> where, bool isLock = true)
    //{
    //    if (!where.ContainsKey("RowVer"))
    //    {
    //        throw new Exception("Column RowVer Not Exist");
    //    }

    //    if (where["RowVer"] == null)
    //    {
    //        throw new Exception("RowVer Value Is Null");
    //    }

    //    if (update.Body.ToString().IndexOf("RowVer", StringComparison.Ordinal) == -1)
    //    {
    //        throw new Exception("Column RowVer Update Is Null");
    //    }

    //    var sqlWhere = "";
    //    foreach (var item in where)
    //    {
    //        sqlWhere += string.IsNullOrWhiteSpace(sqlWhere)
    //            ? $" {item.Key}='{item.Value}'"
    //            : $" and {item.Key}='{item.Value}'";
    //    }

    //    IUpdateable<T> up = SugarClient.Updateable<T>().SetColumns(update).Where(sqlWhere);
    //    if (isLock)
    //    {
    //        up = up.With(SqlWith.UpdLock);
    //    }

    //    var result = await up.ExecuteCommandAsync();
    //    return result;
    //}

    //#endregion

    //#region 删除操作

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="id">主键ID</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<bool> DeleteByPrimaryAsync(object id, bool isLock = true)
    //{
    //    //return await _db.Deleteable<T>(id).ExecuteCommandHasChangeAsync();

    //    var del = SugarClient.Deleteable<T>(id);
    //    if (isLock)
    //    {
    //        del = del.With(SqlWith.RowLock);
    //    }

    //    return await del.ExecuteCommandAsync() > 0;
    //}

    ///// <summary>
    ///// 批量删除实体
    ///// </summary>
    ///// <param name="primaryKeyValues">主键ID集合</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> DeleteByPrimaryAsync(List<object> primaryKeyValues, bool isLock = true)
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
    //public async Task<int> DeleteAsync(T entity, bool isLock = true)
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
    //public async Task<int> DeleteAsync(List<T> entitys, bool isLock = true)
    //{
    //    var del = SugarClient.Deleteable(entitys);
    //    if (isLock)
    //    {
    //        del = del.With(SqlWith.RowLock);
    //    }

    //    return await del.ExecuteCommandAsync();
    //}

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> DeleteAsync(Expression<Func<T, bool>> whereLambda, bool isLock = true)
    //{
    //    var del = SugarClient.Deleteable<T>().Where(whereLambda);
    //    if (isLock)
    //    {
    //        del = del.With(SqlWith.RowLock);
    //    }

    //    return await del.ExecuteCommandAsync();
    //}

    ///// <summary>
    ///// 删除实体
    ///// </summary>
    ///// <param name="inValues">主键集合</param>
    ///// <param name="isLock">是否加锁</param>
    ///// <returns>受影响行数</returns>
    //public async Task<int> DeleteInAsync(List<dynamic> inValues, bool isLock = true)
    //{
    //    var del = SugarClient.Deleteable<T>().In(inValues);
    //    if (isLock)
    //    {
    //        del = del.With(SqlWith.RowLock);
    //    }

    //    return await del.ExecuteCommandAsync();
    //}

    //#endregion

    //#region 单表查询

    ///// <summary>
    ///// 查询单个
    ///// </summary>
    ///// <param name="expression">返回表达式</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <typeparam name="TResult">返回对象</typeparam>
    ///// <returns>自定义数据</returns>
    //public async Task<TResult> QueryAsync<TResult>(Expression<Func<T, TResult>> expression,
    //    Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(expression)
    //        .FirstAsync();
    //}

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="expression">返回表达式</param>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <typeparam name="TResult">返回对象</typeparam>
    ///// <returns>自定义数据</returns>
    //public async Task<List<TResult>> QueryListExpAsync<TResult>(Expression<Func<T, TResult>> expression,
    //    Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(expression)
    //        .ToListAsync();
    //}

    ///// <summary>
    ///// 查询单个
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>实体对象</returns>
    //public async Task<T> QueryFirstAsync(Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .FirstAsync();
    //}

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="orderFileds"></param>
    ///// <param name="orderByType"></param>
    ///// <returns>实体列表</returns>
    //public async Task<List<T>> QueryListAsync(Expression<Func<T, bool>> whereLambda = null,
    //    Expression<Func<T, object>> orderFileds = null, OrderByType orderByType = OrderByType.Desc)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(whereLambda.IsNotNull(), whereLambda)
    //        .OrderByIF(orderFileds.IsNotNull(), orderFileds, orderByType)
    //        .ToListAsync();
    //}

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="sql">SQL</param>
    ///// <returns>实体列表</returns>
    //public async Task<List<T>> QuerySqlListAsync(string sql)
    //{
    //    return await SugarClient.SqlQueryable<T>(sql).ToListAsync();
    //}

    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression"></param>
    ///// <param name="isSplitTable">分表查询</param>
    ///// <returns></returns>
    //public async Task<List<T>> QueryPageListAsync(Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination, Expression<Func<T, T>> selectExpression = null, bool isSplitTable = false)
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable<T>();
    //    query = query.WhereIF(whereLambda != null, whereLambda);
    //    if (selectExpression != null)
    //    {
    //        query = query.Select(selectExpression);
    //    }

    //    if (isSplitTable)
    //    {
    //        query = query.SplitTable();
    //    }

    //    query = query.OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields));
    //    var list = await query.ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression"></param>
    ///// <param name="navigationExpression"></param>
    ///// <param name="navigationExpression2"></param>
    ///// <param name="navigationExpression3"></param>
    ///// <returns></returns>
    //public async Task<List<T>> QueryPageListAsync<T, T2, T3>(Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination, Expression<Func<T, T>> selectExpression = null,
    //    Expression<Func<T, T>> navigationExpression = null,
    //    Expression<Func<T, List<T2>>> navigationExpression2 = null,
    //    Expression<Func<T, List<T3>>> navigationExpression3 = null)
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable<T>();

    //    if (navigationExpression != null)
    //    {
    //        query = query.Includes(navigationExpression);
    //    }

    //    if (navigationExpression2 != null)
    //    {
    //        query = query.Includes(navigationExpression2);
    //    }

    //    if (navigationExpression3 != null)
    //    {
    //        query = query.Includes(navigationExpression3);
    //    }


    //    query = query.WhereIF(whereLambda != null, whereLambda);
    //    if (selectExpression != null)
    //    {
    //        query = query.Select(selectExpression);
    //    }

    //    query = query.OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields));
    //    var list = await query.ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}


    ///// <summary>
    ///// 实体列表 分页查询
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="pagination">分页对象</param>
    ///// <param name="selectExpression"></param>
    ///// <param name="navigationExpression"></param>
    ///// <param name="navigationExpression2"></param>
    ///// <param name="navigationExpression3"></param>
    ///// <param name="navigationExpression4"></param>
    ///// <returns></returns>
    //public async Task<List<T>> QueryPageListAsync<T, T2, T3, T4>(Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination, Expression<Func<T, T>> selectExpression = null,
    //    Expression<Func<T, T>> navigationExpression = null,
    //    Expression<Func<T, List<T2>>> navigationExpression2 = null,
    //    Expression<Func<T, List<T3>>> navigationExpression3 = null,
    //    Expression<Func<T, List<T4>>> navigationExpression4 = null)
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable<T>();

    //    if (navigationExpression != null)
    //    {
    //        query = query.Includes(navigationExpression);
    //    }

    //    if (navigationExpression2 != null)
    //    {
    //        query = query.Includes(navigationExpression2);
    //    }

    //    if (navigationExpression3 != null)
    //    {
    //        query = query.Includes(navigationExpression3);
    //    }

    //    if (navigationExpression4 != null)
    //    {
    //        query = query.Includes(navigationExpression4);
    //    }

    //    query = query.WhereIF(whereLambda != null, whereLambda);
    //    if (selectExpression != null)
    //    {
    //        query = query.Select(selectExpression);
    //    }

    //    query = query.OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields));
    //    var list = await query.ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="inFieldName">指定字段名</param>
    ///// <param name="inValues">值</param>
    ///// <returns>实体列表</returns>
    //public async Task<List<T>> QueryListInAsync(string inFieldName, List<dynamic> inValues)
    //{
    //    return await SugarClient.Queryable<T>().In(inFieldName, inValues).ToListAsync();
    //}

    ///// <summary>
    ///// 查询单个对象
    ///// </summary>
    ///// <param name="id">列值</param>
    ///// <param name="columnName">列名 默认ID</param>
    ///// <returns>实体对象</returns>
    //public async Task<T> QuerySingleAsync(object id, string columnName = "id")
    //{
    //    if (id.IsNull())
    //    {
    //        throw new BusException("请传入id");
    //    }

    //    var conModels = new List<IConditionalModel>
    //    {
    //        new ConditionalModel
    //        {
    //            FieldName = columnName, ConditionalType = ConditionalType.Equal, FieldValue = id.ToString()
    //        }
    //        // new ConditionalModel
    //        // {
    //        //     FieldName = "is_deleted", ConditionalType = ConditionalType.Equal, FieldValue = "0"
    //        // }
    //    };
    //    return await SugarClient.Queryable<T>().Where(conModels).SingleAsync();
    //    // 这种方式不适合软删除模式
    //    // return await _db.Queryable<T>().InSingleAsync(id);
    //}

    ///// <summary>
    ///// 实体列表
    ///// </summary>
    ///// <param name="values">列值集合</param>
    ///// <param name="columnName">列名 默认ID</param>
    ///// <returns>实体列表</returns>
    //public async Task<List<T>> QueryListInAsync(List<long> values, string columnName = "id")
    //{
    //    var conModels = new List<IConditionalModel>
    //    {
    //        new ConditionalModel
    //        {
    //            FieldName = columnName, ConditionalType = ConditionalType.In,
    //            FieldValue = string.Join(",", values)
    //        }
    //    };
    //    return await SugarClient.Queryable<T>().Where(conModels).ToListAsync();
    //    //return await _db.Queryable<T>().In(values).ToListAsync();
    //}

    ///// <summary>
    ///// DataTable数据源
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>DataTable</returns>
    //public async Task<DataTable> QueryDataTableAsync(Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .ToDataTableAsync();
    //}

    ///// <summary>
    ///// DataTable数据源
    ///// </summary>
    ///// <param name="sql">SQL</param>
    ///// <returns>DataTable</returns>
    //public async Task<DataTable> QueryDataTableAsync(string sql)
    //{
    //    return await SugarClient.Ado.GetDataTableAsync(sql);
    //}

    ///// <summary>
    ///// Object
    ///// </summary> 
    ///// <param name="sql">SQL</param> 
    ///// <returns>Object</returns>
    //public async Task<object> QuerySqlScalarAsync(string sql)
    //{
    //    return await SugarClient.Ado.GetScalarAsync(sql);
    //}

    ///// <summary>
    ///// 查询单个对象
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>对象.json</returns>
    //public async Task<string> QueryJsonAsync(Expression<Func<T, bool>> whereLambda = null)
    //{
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (whereLambda != null)
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    return await up.ToJsonAsync();
    //}

    //#endregion

    //#region 多表联查，最大支持16个表

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, TResult>(
    //    Expression<Func<T, T2, object[]>> joinExpression, Expression<Func<T, T2, TResult>> selectExpression,
    //    Expression<Func<T, T2, bool>> whereLambda = null,
    //    Expression<Func<T, T2, object>> groupExpression = null,
    //    string sortField = "")
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    if (!sortField.IsNullOrEmpty())
    //    {
    //        query = query.OrderBy(sortField);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, TResult>(
    //    Expression<Func<T, T2, T3, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, TResult>(
    //    Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //public async Task<List<TResult>> QueryMuchAsync<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, T9, T10, object>> groupExpression = null)
    //{
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (!groupExpression.IsNullOrEmpty())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    return await query.WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression)
    //        .ToListAsync();
    //}

    //#endregion

    //#region 多表联查分页 最大支持16个表

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, TResult>(Pagination pagination,
    //    Expression<Func<T, T2, object[]>> joinExpression, Expression<Func<T, T2, TResult>> selectExpression,
    //    Expression<Func<T, T2, bool>> whereLambda = null,
    //    Expression<Func<T, T2, object>> groupExpression = null)
    //    where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, TResult>(Pagination pagination,
    //    Expression<Func<T, T2, T3, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, TResult>(Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, TResult>(Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, TResult>(Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, TResult>(
    //    Pagination pagination,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //public async Task<List<TResult>> QueryMuchPageAsync<T, T2, T3, T4, T5, T6, T7, T8, TResult>(
    //    Pagination pagination, Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object[]>> joinExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, TResult>> selectExpression,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, bool>> whereLambda = null,
    //    Expression<Func<T, T2, T3, T4, T5, T6, T7, T8, object>> groupExpression = null) where T : class, new()
    //{
    //    RefAsync<int> totalCount = 0;
    //    var query = SugarClient.Queryable(joinExpression);
    //    if (groupExpression.IsNotNull())
    //    {
    //        query = query.GroupBy(groupExpression);
    //    }

    //    var list = await query
    //        .OrderByIF(pagination.SortFields.Count > 0, string.Join(",", pagination.SortFields))
    //        .WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda)
    //        .Select(selectExpression).ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return list;
    //}

    //#endregion

    //#region 一对一 一对多查询

    //public async Task<List<T>> QueryMapperAsync(Action<T> mapperAction,
    //    Expression<Func<T, bool>> whereLambda = null)
    //{
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (whereLambda != null)
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    var datas = await up.Mapper(mapperAction).ToListAsync();
    //    return datas;
    //}

    //public async Task<List<T>> QueryMapperPageListAsync(Action<T> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, Pagination pagination)
    //{
    //    RefAsync<int> totalCount = 0;
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (!whereLambda.IsNullOrEmpty())
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    if (pagination.SortFields.Count > 0)
    //    {
    //        up = up.OrderBy(string.Join(",", pagination.SortFields));
    //    }

    //    var datas = await up.Mapper(mapperAction)
    //        .ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return datas;
    //}

    //public async Task<List<T>> QueryMapperPageListAsync<TObject>(
    //    Expression<Func<T, List<TObject>>> mapperObject,
    //    Expression<Func<T, object>> mapperField, Expression<Func<T, bool>> whereLambda,
    //    Pagination pagination)
    //{
    //    RefAsync<int> totalCount = 0;
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (!whereLambda.IsNullOrEmpty())
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    if (pagination.SortFields.Count > 0)
    //    {
    //        up = up.OrderBy(string.Join("", pagination.SortFields));
    //    }

    //    var datas = await up.Mapper(mapperObject, mapperField)
    //        .ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return datas;
    //}

    //public async Task<List<T>> QueryMapperAsync(Action<T, MapperCache<T>> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, string sortField = "")
    //{
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (!whereLambda.IsNullOrEmpty())
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    if (!sortField.IsNullOrEmpty())
    //    {
    //        up = up.OrderBy(sortField);
    //    }

    //    var datas = await up.Mapper(mapperAction).ToListAsync();
    //    return datas;
    //}

    //public async Task<List<T>> QueryMapperPageListAsync(
    //    Action<T, MapperCache<T>> mapperAction,
    //    Expression<Func<T, bool>> whereLambda, Pagination pagination)
    //{
    //    RefAsync<int> totalCount = 0;
    //    ISugarQueryable<T> up = SugarClient.Queryable<T>();
    //    if (!whereLambda.IsNullOrEmpty())
    //    {
    //        up = up.Where(whereLambda);
    //    }

    //    if (pagination.SortFields.Count > 0)
    //    {
    //        up = up.OrderBy(string.Join(",", pagination.SortFields));
    //    }

    //    var datas = await up.Mapper(mapperAction)
    //        .ToPageListAsync(pagination.PageIndex, pagination.PageSize, totalCount);
    //    pagination.TotalElements = totalCount;
    //    return datas;
    //}

    //#endregion

    //#region 存储过程

    ///// <summary>
    ///// 执行存储过程DataSet
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>DataSet</returns>
    //public async Task<DataSet> QueryProcedureDataSetAsync(string procedureName, List<SugarParameter> parameters)
    //{
    //    var listParams = ConvetParameter(parameters);
    //    var datas = await SugarClient.Ado.UseStoredProcedure().GetDataSetAllAsync(procedureName, listParams);
    //    return datas;
    //}

    ///// <summary>
    ///// 执行存储过程DataTable
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>DataTable</returns>
    //public async Task<DataTable> QueryProcedureAsync(string procedureName, List<SugarParameter> parameters)
    //{
    //    var listParams = ConvetParameter(parameters);
    //    var datas = await SugarClient.Ado.UseStoredProcedure().GetDataTableAsync(procedureName, listParams);
    //    return datas;
    //}

    ///// <summary>
    ///// 执行存储过程Object
    ///// </summary>
    ///// <param name="procedureName">存储过程名称</param>
    ///// <param name="parameters">参数集合</param>
    ///// <returns>Object</returns>
    //public async Task<object> QueryProcedureScalarAsync(string procedureName, List<SugarParameter> parameters)
    //{
    //    var listParams = ConvetParameter(parameters);
    //    var datas = await SugarClient.Ado.UseStoredProcedure().GetScalarAsync(procedureName, listParams);
    //    return datas;
    //}

    //#endregion

    //#region 常用函数

    ///// <summary>
    ///// 查询前面几条
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <param name="topNum">要多少条</param>
    ///// <returns>泛型对象集合</returns>
    //public async Task<List<T>> TakeAsync(int topNum, Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda).Take(topNum)
    //        .ToListAsync();
    //}

    ///// <summary>
    ///// 对象是否存在
    ///// </summary>
    ///// <param name="whereLambda">条件表达式</param>
    ///// <returns>True or False</returns>
    //public async Task<bool> IsExistAsync(Expression<Func<T, bool>> whereLambda = null)
    //{
    //    return await SugarClient.Queryable<T>().WhereIF(!whereLambda.IsNullOrEmpty(), whereLambda).AnyAsync();
    //}

    ///// <summary>
    ///// 总和
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <returns>总和</returns>
    //public async Task<int> SumAsync(string field)
    //{
    //    return await SugarClient.Queryable<T>().SumAsync<int>(field);
    //}

    ///// <summary>
    ///// 最大值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <typeparam name="TResult">泛型结果</typeparam>
    ///// <returns>最大值</returns>
    //public async Task<TResult> MaxAsync<TResult>(string field)
    //{
    //    return await SugarClient.Queryable<T>().MaxAsync<TResult>(field);
    //}

    ///// <summary>
    ///// 最小值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <typeparam name="TResult">泛型结果</typeparam>
    ///// <returns>最小值</returns>
    //public async Task<TResult> MinAsync<TResult>(string field)
    //{
    //    return await SugarClient.Queryable<T>().MinAsync<TResult>(field);
    //}

    ///// <summary>
    ///// 平均值
    ///// </summary>
    ///// <param name="field">字段名</param>
    ///// <returns>平均值</returns>
    //public async Task<int> AvgAsync(string field)
    //{
    //    return await SugarClient.Queryable<T>().AvgAsync<int>(field);
    //}

    //#endregion

    //#region 流水号

    //public async Task<string> CustomNumberAsync(string key, string prefix = "", int fixedLength = 4,
    //    string dateFomart = "")
    //{
    //    var listNumber = await CustomNumberAsync(key, 1, prefix, fixedLength, dateFomart);
    //    return listNumber[0];
    //}

    //public async Task<List<string>> CustomNumberAsync(string key, int num, string prefix = "",
    //    int fixedLength = 4,
    //    string dateFomart = "")
    //{
    //    List<string> numbers = new List<string>();
    //    var dateValue = dateFomart == "" ? "" : DateTime.Now.ToString(dateFomart);
    //    var fix = prefix.ToUpper() + dateValue;
    //    var maxValue = await SugarClient.Queryable<T>()
    //        .Where(key + " LIKE '" + fix + "%' AND LEN(" + key + ")=" + (fix.Length + fixedLength)).Select(key)
    //        .MaxAsync<string>(key);

    //    if (maxValue == null)
    //    {
    //        for (var i = 0; i < num; i++)
    //        {
    //            var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
    //            numbers.Add(tempNumber);
    //        }
    //    }
    //    else
    //    {
    //        if (maxValue.Substring(0, maxValue.Length - fixedLength) == prefix + dateValue)
    //        {
    //            var tempLast = maxValue.Substring(maxValue.Length - fixedLength);
    //            for (var i = 0; i < num; i++)
    //            {
    //                var tempNumber = fix + (int.Parse(tempLast) + i + 1).ToString().PadLeft(fixedLength, '0');
    //                numbers.Add(tempNumber);
    //            }
    //        }
    //        else
    //        {
    //            for (var i = 0; i < num; i++)
    //            {
    //                var tempNumber = fix + (i + 1).ToString().PadLeft(fixedLength, '0');
    //                numbers.Add(tempNumber);
    //            }
    //        }
    //    }

    //    return numbers;
    //}

    //#endregion

    //#region 参数类型转换

    ///// <summary>
    ///// SqlParameter转SugarParameter
    ///// </summary>
    ///// <param name="parameters">????</param>
    ///// <returns></returns>
    //private List<SugarParameter> ConvetParameter(List<SugarParameter> parameters)
    //{
    //    var listParams = new List<SugarParameter>();
    //    foreach (var p in parameters)
    //    {
    //        var par = new SugarParameter(p.ParameterName, p.Value)
    //        {
    //            DbType = p.DbType,
    //            Direction = p.Direction
    //        };
    //        if (!string.IsNullOrWhiteSpace(p.TypeName))
    //        {
    //            par.TypeName = p.TypeName;
    //        }

    //        listParams.Add(par);
    //    }

    //    return listParams;
    //}

    //#endregion
}
