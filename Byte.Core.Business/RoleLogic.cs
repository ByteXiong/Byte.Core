using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Mapster;
using System.Linq.Expressions;

namespace Byte.Core.Business
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleLogic : BaseBusinessLogic<int, Role, RoleRepository>
    {
        /// <summary />
        /// <param name="repository"></param>
        public RoleLogic(RoleRepository repository) : base(repository) { }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<RoleDTO>> GetPageAsync(RoleParam param)
        {
            Expression<Func<Role, bool>> where = x => x.Code != ParamConfig.Admin;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Name.Contains(param.KeyWord));
            }
            var page = await Repository.GetIQueryable(where).Select<RoleDTO>().ToPagedResultsAsync(param);
            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<RoleInfo> GetInfoAsync(int id)
        {
            var entity = await Repository.GetIQueryable(x => x.Id == id).Select<RoleInfo>().FirstOrDefaultAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(UpdateRoleParam param)
        {
            Role model = param.Adapt<Role>();
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(UpdateRoleParam param)
        {
            var role = await GetIQueryable(x => x.Id == param.Id).FirstOrDefaultAsync();
            role.Name = param.Name; //角色名称
            role.Type = param.Type; //用户类型
            role.Remark = param.Remark; //描述
            role.Code = param.Code;
            role.Sort = param.Sort;
            await UpdateAsync(role);
            return param.Id;
        }

        /// <summary>
        /// 下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleSelectDTO>> SelectAsync()
        {
            var list = await GetIQueryable(x => x.Code != ParamConfig.Admin).OrderByDescending(x => x.Sort).Select<RoleSelectDTO>().ToListAsync();
            return list;
        }


        /// <summary>
        /// 下拉框
        /// </summary>
        /// <returns></returns>
        //[HasRedis("RoleSelect", "" , 30, true)]
        //[UserInformationFilter]
        //[Interceptor(typeof(MyBllInterceptor))]

        //List xx
        //[ServiceInterceptor(typeof(CustomInterceptorAttribute))]
        [RedisInterceptor("list")]
        public virtual async Task<List<RoleSelectDTO>> SelectAsync(int[] schoolId)
        {
            var list = await GetIQueryable(x => x.Code != ParamConfig.Admin).OrderByDescending(x => x.Sort).Select<RoleSelectDTO>().ToListAsync();
       
            return list;
        }

        /// <summary>
        ///  设置状态
        /// </summary>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool status) => await UpdateAsync(x => id == x.Id, x => new Role { Status = status });
    }
}