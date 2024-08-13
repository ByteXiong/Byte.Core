using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.Models;
using Mapster;
using Byte.Core.Tools;
using Byte.Core.Common.Attributes.RedisAttribute;
namespace Byte.Core.Business
{
    /// <summary>
    /// 测试表
    /// </summary>
    public class RedisDemoLogic : BaseBusinessLogic<Guid, RedisDemo, RedisDemoRepository>
    {
        public RedisDemoLogic(RedisDemoRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HasRedisInterceptor(ParamConfig.HRedisDemoKey, "", 30,HasRedisRange.Params)]
        public virtual async  Task<List<RedisDemo>> GetByIdsAsync( Guid[] ids ) {

              var  list = await GetIQueryable(x => ids.Contains(x.Id)).ToListAsync();
              return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HasRedisInterceptor(ParamConfig.HRedisDemoKey, "", 30, HasRedisRange.Params)]
        public virtual async Task<List<RedisDemo>> GetByIdAsync( Guid id)
        {

            var list = await GetIQueryable(x => id == x.Id).ToListAsync();
            return list;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HasRedisInterceptor(ParamConfig.HRedisDemoKey, "", 30, HasRedisRange.Params)]
        public virtual async Task<List<RedisDemo>> GetByIdAsync(Guid[] ids,  string code)
        {


            var list = await GetIQueryable(x => ids.Contains(x.Id)&& x.Code==code).ToListAsync();
            return list;
        }

        /// <summary>
        /// 获取所有的数据
        /// </summary>
        /// <returns></returns>
        /// 

        public List<RedisDemo> GetAll()
        {


            return GetIQueryable().ToList();
        }

       

       


    }
}