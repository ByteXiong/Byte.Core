using Byte.Core.EntityFramework.Extensions;
using Byte.Core.EntityFramework.Helpers;
using Byte.Core.EntityFramework.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;

namespace Byte.Core.EntityFramework.IDbContext
{
    //
    // 摘要:
    //     MongoDb 数据库访问
    //
    // 类型参数:
    //   TEntity:
    public interface IMongoRepository<TEntity> where TEntity : IMongoModel
    {
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
        TEntity GetById(Guid id);

        //
        // 摘要:
        //     返回序列中的第一个对象；如果序列中不包含任何对象，则返回默认值
        //
        // 参数:
        //   filter:
        //     条件表达式
        //
        // 返回结果:
        //     单个泛型对象
        TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter);

        //
        // 摘要:
        //     插入实体对象到数据库
        //
        // 参数:
        //   entity:
        //     实体对象
        void Insert(TEntity entity);

        //
        // 摘要:
        //     插入实体对象集合到数据库
        //
        // 参数:
        //   list:
        //     实体对象集合
        void InsertRange(IList<TEntity> list);

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   entity:
        bool Update(TEntity entity);

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   oldEntity:
        //
        //   newEntity:
        bool Update(TEntity oldEntity, UpdateDefinition<TEntity> newEntity);

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   filter:
        //
        //   field:
        //
        //   value:
        //
        // 类型参数:
        //   TField:
        bool Update<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value);

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   entity:
        //
        //   field:
        //
        //   value:
        //
        // 类型参数:
        //   TField:
        bool Update<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value);

        //
        // 摘要:
        //     从数据库删除实体对象
        //
        // 参数:
        //   entity:
        //     业务实体对象
        long Delete(TEntity entity);

        //
        // 摘要:
        //     从数据库删除实体对象
        //
        // 参数:
        //   filter:
        //     条件表达式
        long Delete(Expression<Func<TEntity, bool>> filter);

        //
        // 摘要:
        //     获取所有实体对象
        //
        // 返回结果:
        //     实体对象清单
        IList<TEntity> GetAll();

        //
        // 摘要:
        //     是否存在
        //
        // 参数:
        //   filter:
        //     过滤条件
        bool Any(Expression<Func<TEntity, bool>> filter);

        //
        // 摘要:
        //     根据过滤条件获取业务实体
        //
        // 参数:
        //   filter:
        //     过滤条件
        IList<TEntity> QueryWhere(Expression<Func<TEntity, bool>> filter);

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
        PagedResults<TEntity> QuickQuery(QuickQueryParam queryParam);

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
        PagedResults<TEntity> AdvQuery<TQueryParam>(TQueryParam queryParam) where TQueryParam : AdvQueryParam;

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
        TModel ConvertToModel<TModel>(TEntity entity) where TModel : class, new();

        //
        // 摘要:
        //     验证实体
        //
        // 参数:
        //   entity:
        //     实体对象
        //
        //   errMessage:
        //     错误信息
        bool TryValidateEntity(TEntity entity, out string errMessage);
    }


}


