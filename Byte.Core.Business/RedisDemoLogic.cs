using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
namespace Byte.Core.Business
{
    /// <summary>
    /// 测试表
    /// </summary>
    public class RedisDemoLogic : BaseBusinessLogic<Guid, RedisDemo, RedisDemoRepository>
    {
        /// <summary />
        /// <param name="repository"></param>
        public RedisDemoLogic(RedisDemoRepository repository) : base(repository)
        {
        }

        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [RedisInterceptor(ParamConfig.AopRedisKey, 30)]
        //[ServiceInterceptor(typeof(RedisInterceptorAttribute))]
        public virtual  async Task<PagedResults<RedisDemoDTO>> GetPageAsync(RedisDemoParam param)
        {
            //Expression<Func<RedisDemo, bool>> where = x => x.Code != ParamConfig.Admin;
            //if (!string.IsNullOrWhiteSpace(param.KeyWord))
            //{
            //    param.KeyWord = param.KeyWord.Trim();
            //    where = where.And(x => x.Name.Contains(param.KeyWord));
            //}
            var page = await Repository.GetIQueryable().Select<RedisDemoDTO>().ToPagedResultsAsync(param);
            return page;
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
/// <param name="id"></param>
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
/// <param name="code"></param>
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