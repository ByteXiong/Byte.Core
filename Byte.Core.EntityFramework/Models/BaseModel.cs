using MongoDB.Bson.Serialization.Attributes;

namespace Byte.Core.EntityFramework.Models
{
    public interface IAggregateRoot
    {

    }

    public interface IBaseModel<TKey> : IAggregateRoot
    {
        TKey Id { get; set; }
    }
    /// <summary>
    /// 所有数据表实体类都必须继承此类
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    [Serializable]
    public abstract class BaseModel<TKey> : IBaseModel<TKey>
    {
        [BsonId]//MongoDB使用_id
        public abstract TKey Id { get; set; }
    }

    [Serializable]
    public abstract class MongoDBBaseModel<TKey> : IBaseModel<TKey>
    {
 
        public abstract TKey Id { get; set; }
    }
    /// <summary>
    /// 所有数据库视图对应实体类必须继承此类
    /// </summary>
    [Serializable]
    public abstract class BaseViewModel : IAggregateRoot
    {

    }
}
