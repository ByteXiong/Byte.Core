using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.Models;
using Byte.Core.Tools;
using System.Linq.Expressions;
using Mapster;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Attributes.RedisAttribute;
using Microsoft.AspNetCore.Mvc;
using AspectCore.DynamicProxy;

namespace Byte.Core.Business
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleLogic : BaseBusinessLogic<Guid, Role, RoleRepository>
    {
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
        public async Task<RoleInfo> GetInfoAsync(Guid id)
        {
            var entity = await Repository.GetIQueryable(x => x.Id == id).Select<RoleInfo>().FirstOrDefaultAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(UpdateRoleParam param)
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
        public async Task<Guid> UpdateAsync(UpdateRoleParam param)
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
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<RoleSelectDTO>> SelectAsync()
        {
            var list = await GetIQueryable(x => x.Code != ParamConfig.Admin).OrderByDescending(x => x.Sort).Select<RoleSelectDTO>().ToListAsync();
            return list;
        }



        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        //[HasRedis("RoleSelect", "" , 30, true)]
        //[UserInformationFilter]
        //[Interceptor(typeof(MyBllInterceptor))]
        [CustomInterceptor("admin")]

        //[ServiceInterceptor(typeof(CustomInterceptorAttribute))]
        public virtual async Task<List<RoleSelectDTO>> SelectAsync(string name)
        {
            var aa = name;
            var list = await GetIQueryable(x => x.Code != ParamConfig.Admin).OrderByDescending(x => x.Sort).Select<RoleSelectDTO>().ToListAsync();
            return list;
        }

        /// <summary>
        ///  设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task<int> SetStateAsync(Guid id, bool state) => await UpdateAsync(x => id == x.Id, x => new Role { State = state });
    }
}