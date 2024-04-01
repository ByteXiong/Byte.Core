using Byte.Core.Common.IoC;
using SqlSugar;

namespace Byte.Core.SqlSugar.BusinessLogics
{
    public interface ISugarLogic<TEntity>  where TEntity : class,new()
    {



        /// <summary>
        /// sqlSugarClient
        /// </summary>
        ISqlSugarClient SugarClient { get; }

     
    }
}
