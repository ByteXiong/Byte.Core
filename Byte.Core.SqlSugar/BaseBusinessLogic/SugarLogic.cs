using Byte.Core.Common.IoC;
using Byte.Core.SqlSugar.Repository;
using SqlSugar;

namespace Byte.Core.SqlSugar.BusinessLogics
{
    public abstract class SugarLogic<TEntity, TRepository> : ISugarLogic<TEntity> where TEntity : class, new()
          where TRepository : ISugarRepository<TEntity>
    {
        #region 字段

        /// <summary>
        /// 当前操作对象仓储
        /// </summary>
        public ISugarRepository<TEntity> SugarRepository { get; set; }

        /// <summary>
        /// sugarClient
        /// </summary>
        public ISqlSugarClient SugarClient => SugarRepository.SugarClient;

        #endregion



        public TRepository Repository { get; set; }
        protected SugarLogic(TRepository repository)
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
    }
}
