using Byte.EF.CodeFirst.Common.IoC;
using Byte.EF.CodeFirst.Common.Models;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.EF.CodeFirst.Common.DBHelper
{
    //
    // 摘要:
    //     基于芒果实体业务逻辑基类
    //
    // 类型参数:
    //   TEntity:
    //     业务实体
    //
    //   TRepository:
    //     数据访问接口
    public abstract class MongoLogic<TEntity, TRepository> : IMongoLogic<TEntity> where TEntity : IMongoModel where TRepository : IMongoRepository<TEntity>
    {
        //
        // 摘要:
        //     数据访问接口
        protected readonly TRepository Repository;

        //
        // 摘要:
        //     当前登录用户
        [FromContainer]
        public PrincipalUser CurrentUser { get; set; }

        //
        // 摘要:
        //     构造函数
        //
        // 参数:
        //   repository:
        //     注入数据访问接口
        protected MongoLogic(TRepository repository)
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

        //
        // 摘要:
        //     创建实体对象
        //
        // 参数:
        //   entity:
        //     实体对象
        //
        //   result:
        //     操作状态
        public virtual void Create(TEntity entity, out ExcutedResult result)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "entity为空！");
            }

            if (entity.Id == Guid.Empty)
            {
                entity.Id = Guid.NewGuid();
            }

            TRepository repository = Repository;
            if (!repository.TryValidateEntity(entity, out var errMessage))
            {
                Log4NetHelper.WriteError(GetType(), errMessage);
                throw new ArgumentException("entity 验证失败");
            }

            repository = Repository;
            repository.Insert(entity);
            result = ExcutedResult.SuccessResult("创建成功");
        }

        //
        // 摘要:
        //     插入实体对象集合到数据库
        //
        // 参数:
        //   list:
        //     实体对象集合
        //
        //   result:
        //     操作状态
        public virtual void Create(IList<TEntity> list, out ExcutedResult result)
        {
            if (list == null || list.Count == 0)
            {
                throw new ArgumentNullException("list", "添加实体集合为空！");
            }

            TRepository repository;
            foreach (TEntity item in list)
            {
                TEntity current = item;
                if (current == null)
                {
                    throw new ArgumentNullException("entity", "集合中存在实体为空！");
                }

                if (current.Id == Guid.Empty)
                {
                    current.Id = Guid.NewGuid();
                }

                repository = Repository;
                if (!repository.TryValidateEntity(current, out var errMessage))
                {
                    Log4NetHelper.WriteError(GetType(), errMessage);
                    throw new ArgumentException("entity 验证失败");
                }
            }

            repository = Repository;
            repository.InsertRange(list);
            result = ExcutedResult.SuccessResult("创建成功");
        }

        //
        // 摘要:
        //     修改实体对象
        //
        // 参数:
        //   entity:
        //     实体对象
        //
        //   result:
        //     操作状态
        public virtual void Update(TEntity entity, out ExcutedResult result)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity", "entity为空！");
            }

            if (entity.Id == Guid.Empty)
            {
                throw new ArgumentException("实体的Id为空。");
            }

            TRepository repository = Repository;
            if (!repository.QueryWhere((TEntity p) => ((IMongoEntity<Guid>)p).Id == ((IMongoEntity<Guid>)entity).Id).Any())
            {
                result = ExcutedResult.FailedResult("1010", "原始实体对象并不存在");
                return;
            }

            repository = Repository;
            repository.Update(entity);
            result = ExcutedResult.SuccessResult("更新成功");
        }

        //
        // 摘要:
        //     删除实体对象
        //
        // 参数:
        //   id:
        //     实体对象ID
        //
        //   result:
        //     操作状态
        public virtual void Delete(Guid id, out ExcutedResult result)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("id为空。");
            }

            TEntity byId = GetById(id);
            if (byId == null)
            {
                result = ExcutedResult.FailedResult("1001", $"ID:{id}的实体对象为空");
                return;
            }

            TRepository repository = Repository;
            repository.Delete(byId);
            result = ExcutedResult.SuccessResult("删除成功！");
        }

        //
        // 摘要:
        //     获取所有实体对象
        //
        // 返回结果:
        //     实体对象清单
        public virtual IList<TEntity> GetAll()
        {
            TRepository repository = Repository;
            return repository.GetAll();
        }

        //
        // 摘要:
        //     根据Id获取实体对象
        //
        // 参数:
        //   id:
        //     实体标识
        //
        // 返回结果:
        //     单个泛型对象
        public virtual TEntity GetById(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("id为空");
            }

            TRepository repository = Repository;
            return repository.GetById(id);
        }

        //
        // 摘要:
        //     根据查询参数执行快速查询，并生成分页信息
        //
        // 参数:
        //   queryParam:
        //     查询参数
        //
        // 返回结果:
        //     业务对象查询结果
        public virtual PagedResults<TEntity> QuickQuery(QuickQueryParam queryParam)
        {
            TRepository repository = Repository;
            return repository.QuickQuery(queryParam);
        }

        //
        // 摘要:
        //     根据查询参数执行高级查询，并生成分页信息
        //
        // 参数:
        //   queryParam:
        //     查询参数
        //
        // 返回结果:
        //     业务对象查询结果
        public virtual PagedResults<TEntity> AdvQuery<TQueryParam>(TQueryParam queryParam) where TQueryParam : AdvQueryParam
        {
            TRepository repository = Repository;
            return repository.AdvQuery(queryParam);
        }

        //
        // 摘要:
        //     刷新缓存
        public virtual void RefreshCache()
        {
        }

        //
        // 摘要:
        //     业务实体转化为业务模型
        //
        // 参数:
        //   entity:
        //     业务实体
        //
        // 类型参数:
        //   TModel:
        //     业务模型
        protected virtual TModel ConvertToModel<TModel>(TEntity entity) where TModel : class, new()
        {
            TRepository repository = Repository;
            return repository.ConvertToModel<TModel>(entity);
        }
    }
}
