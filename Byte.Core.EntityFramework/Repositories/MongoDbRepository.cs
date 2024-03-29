using Byte.Core.Common.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using MongoDB.Driver;
using System.Data;
using System.Linq.Expressions;

namespace Byte.Core.EntityFramework.IDbContext
{
    //
    // 摘要:
    //     Mongodb linq模式数据访问
    //
    // 类型参数:
    //   TEntity:
    //     业务实体，必须加[BsonId]标注
    public abstract class MongoDbRepository<TEntity> : IMongoRepository<TEntity> where TEntity : class, IMongoModel
    {
        //
        // 摘要:
        //     The MongoDbContext
        protected IMongoDbContext MongoDbContext { get; set; }

        //
        // 摘要:
        //     获取当前实体集
        protected IMongoCollection<TEntity> DbSet => GetCollection();

        //
        // 摘要:
        //     当前登录用户
        [FromContainer]
        public PrincipalUser CurrentUser { get; set; }

        //
        // 摘要:
        //     The contructor taking a Fisho.Framework.Data.DbContext.Mongo.IMongoDbContext.
        //
        // 参数:
        //   mongoDbContext:
        //     A mongodb context implementing Fisho.Framework.Data.DbContext.Mongo.IMongoDbContext
        protected MongoRepository(IMongoDbContext mongoDbContext)
        {
            MongoDbContext = mongoDbContext;
        }

        //
        // 摘要:
        //     获取符合条件的第一个对象
        //
        // 参数:
        //   filter:
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Find(filter).FirstOrDefault();
        }

        //
        // 摘要:
        //     获取所有对象
        public virtual IList<TEntity> GetAll()
        {
            return DbSet.Find((TEntity p) => true).ToList();
        }

        //
        // 摘要:
        //     根据Id获取对象
        //
        // 参数:
        //   id:
        public virtual TEntity GetById(Guid id)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("Id", id);
            return DbSet.Find(filter).FirstOrDefault();
        }

        //
        // 摘要:
        //     是否存在
        //
        // 参数:
        //   filter:
        public virtual bool Any(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.CountDocuments(filter) > 0;
        }

        //
        // 摘要:
        //     创建/插入
        //
        // 参数:
        //   entity:
        public virtual void Insert(TEntity entity)
        {
            DbSet.InsertOne(entity);
        }

        //
        // 摘要:
        //     创建/插入列表
        //
        // 参数:
        //   list:
        public virtual void InsertRange(IList<TEntity> list)
        {
            if (!list.Any())
            {
                return;
            }

            foreach (TEntity item in list)
            {
                FormaTEntity(item);
            }

            DbSet.InsertMany(list);
        }

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   entity:
        public virtual bool Update(TEntity entity)
        {
            return DbSet.ReplaceOne((TEntity p) => p.Id == entity.Id, entity).ModifiedCount == 1;
        }

        //
        // 摘要:
        //     修改
        //
        // 参数:
        //   oldEntity:
        //
        //   newEntity:
        public virtual bool Update(TEntity oldEntity, UpdateDefinition<TEntity> newEntity)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("Id", oldEntity.Id);
            return DbSet.UpdateOne(filter, newEntity, new UpdateOptions
            {
                IsUpsert = true
            }).ModifiedCount == 1;
        }

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
        public virtual bool Update<TField>(Expression<Func<TEntity, bool>> filter, Expression<Func<TEntity, TField>> field, TField value)
        {
            return DbSet.UpdateOne(filter, Builders<TEntity>.Update.Set(field, value)).ModifiedCount == 1;
        }

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
        public bool Update<TField>(TEntity entity, Expression<Func<TEntity, TField>> field, TField value)
        {
            FilterDefinition<TEntity> filter = Builders<TEntity>.Filter.Eq("Id", entity.Id);
            return DbSet.UpdateOne(filter, Builders<TEntity>.Update.Set(field, value)).ModifiedCount == 1;
        }

        //
        // 摘要:
        //     删除
        //
        // 参数:
        //   filter:
        public virtual long Delete(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.DeleteOne(filter).DeletedCount;
        }

        //
        // 摘要:
        //     删除
        //
        // 参数:
        //   entity:
        public virtual long Delete(TEntity entity)
        {
            return DbSet.DeleteOne((TEntity p) => p.Id == entity.Id).DeletedCount;
        }

        //
        // 摘要:
        //     Deletes a list of entitys.
        //
        // 参数:
        //   entitys:
        //     The list of entitys to delete.
        //
        // 类型参数:
        //   TEntity:
        //     The type representing a entity.
        //
        // 返回结果:
        //     The number of entitys deleted.
        public long DeleteMany(IEnumerable<TEntity> entitys)
        {
            if (!entitys.Any())
            {
                return 0L;
            }

            Guid[] idsTodelete = entitys.Select((TEntity e) => e.Id).ToArray();
            return DbSet.DeleteMany((TEntity x) => idsTodelete.Contains(x.Id)).DeletedCount;
        }

        //
        // 摘要:
        //     Deletes the entitys matching the condition of the LINQ expression filter.
        //
        // 参数:
        //   filter:
        //     A LINQ expression filter.
        //
        // 类型参数:
        //   TEntity:
        //     The type representing a entity.
        //
        // 返回结果:
        //     The number of entitys deleted.
        public long DeleteMany(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.DeleteMany(filter).DeletedCount;
        }

        //
        // 摘要:
        //     根据过滤条件获取业务实体
        //
        // 参数:
        //   filter:
        //     过滤条件
        public virtual IList<TEntity> QueryWhere(Expression<Func<TEntity, bool>> filter)
        {
            return DbSet.Find(filter).ToList();
        }

        //
        // 摘要:
        //     获取快速查询集
        protected Expression<Func<TEntity, bool>> GetQuickQuery(QuickQueryParam queryParam)
        {
            return (TEntity p) => true;
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
            Expression<Func<TEntity, bool>> quickQuery = GetQuickQuery(queryParam);
            return QueryPagedResults(quickQuery, queryParam);
        }

        //
        // 摘要:
        //     获取高级查询集
        protected virtual Expression<Func<TEntity, bool>> GetAdvQuery<TQueryParam>(TQueryParam queryParam) where TQueryParam : AdvQueryParam
        {
            return (TEntity p) => true;
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
            Expression<Func<TEntity, bool>> advQuery = GetAdvQuery(queryParam);
            return QueryPagedResults(advQuery, queryParam);
        }

        //
        // 摘要:
        //     分页查询
        //
        // 参数:
        //   filter:
        //
        //   queryParam:
        //
        // 类型参数:
        //   TQueryParam:
        protected virtual PagedResults<TEntity> QueryPagedResults<TQueryParam>(Expression<Func<TEntity, bool>> filter, TQueryParam queryParam) where TQueryParam : PageParam
        {
            if (queryParam.StartIndex < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于0");
            }

            if (queryParam.PageSize <= 0)
            {
                throw new InvalidOperationException("每页记录数不能小于0");
            }

            if (filter == null)
            {
                filter = (TEntity p) => true;
            }

            long value = DbSet.Find(filter).CountDocuments();
            PagerInfo pagerInfo = new PagerInfo(queryParam)
            {
                TotalRowCount = Convert.ToInt32(value)
            };
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }

            IFindFluent<TEntity, TEntity> findFluent = DbSet.Find(filter);
            if (queryParam.SortList != null && queryParam.SortList.Count > 0)
            {
                Dictionary<string, int> dictionary = queryParam.SortList.ToDictionary((KeyValuePair<string, bool> k) => k.Key, (KeyValuePair<string, bool> v) => (!v.Value) ? 1 : (-1));
                findFluent = findFluent.Sort(new BsonDocument(dictionary));
            }
            else if (!string.IsNullOrEmpty(queryParam.SortName))
            {
                findFluent = findFluent.Sort(new BsonDocument(new BsonElement(queryParam.SortName, (!queryParam.IsSortOrderDesc) ? 1 : (-1))));
            }

            List<TEntity> data = findFluent.Skip(pagerInfo.StartIndex).Limit(queryParam.PageSize).ToList();
            return new PagedResults<TEntity>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
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
        public virtual TModel ConvertToModel<TModel>(TEntity entity) where TModel : class, new()
        {
            return MapperHelper<TEntity, TModel>.Map(entity);
        }

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
        public virtual bool TryValidateEntity(TEntity entity, out string errMessage)
        {
            ValidationContext validationContext = new ValidationContext(entity);
            List<ValidationResult> list = new List<ValidationResult>();
            bool result = Validator.TryValidateObject(entity, validationContext, list);
            StringBuilder stringBuilder = new StringBuilder();
            foreach (ValidationResult item in list)
            {
                stringBuilder.AppendLine(item.ErrorMessage);
            }

            errMessage = stringBuilder.ToString();
            return result;
        }

        private IMongoCollection<TEntity> GetCollection()
        {
            return MongoDbContext.GetCollection<TEntity>();
        }

        private void FormaTEntity(TEntity entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }

            if (entity.Id == default(Guid))
            {
                entity.Id = Guid.NewGuid();
            }
        }
    }
}

