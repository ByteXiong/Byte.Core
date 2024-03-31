namespace Byte.EF.CodeFirst.Common.DBHelper
{
    //
    // 摘要:
    //     Mongodb数据处理业务逻辑接口
    //
    // 类型参数:
    //   T:
    //     业务实体类型
    public interface IMongoLogic<T> where T : IMongoModel
    {
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
        void Create(T entity, out ExcutedResult result);

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
        void Create(IList<T> list, out ExcutedResult result);

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
        void Update(T entity, out ExcutedResult result);

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
        void Delete(Guid id, out ExcutedResult result);

        //
        // 摘要:
        //     获取所有实体对象
        //
        // 返回结果:
        //     实体对象清单
        IList<T> GetAll();

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
        T GetById(Guid id);

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
        PagedResults<T> QuickQuery(QuickQueryParam queryParam);

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
        PagedResults<T> AdvQuery<TQueryParam>(TQueryParam queryParam) where TQueryParam : AdvQueryParam;

        //
        // 摘要:
        //     刷新缓存(子类重写实现)
        void RefreshCache();
    }
}
