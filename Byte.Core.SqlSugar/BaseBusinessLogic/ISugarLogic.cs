using Byte.Core.Common.IoC;
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

    }
}
