using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.Common.Cache;
using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Mapster;
using NPOI.SS.Formula.Functions;
using Quartz.Util;
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
            Expression<Func<Role, bool>> where = x => x.Code != AppConfig.Root;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Name.Contains(param.KeyWord));
            }
            var page = await Repository.GetIQueryable(where).Select(x=>new RoleDTO {
             DeptName=x.Dept.EasyName,
            }, true).ToPagedResultsAsync(param);
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
            model.DeptId = CurrentUser.DeptId;
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
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstOrDefaultAsync();
            entity.Name = param.Name; //角色名称
            entity.Type = param.Type; //用户类型
            entity.Remark = param.Remark; //描述
            entity.Code = param.Code;
            entity.Sort = param.Sort;
            entity.Status = param.Status;
            entity.DeptId = CurrentUser.DeptId;
            await UpdateAsync(entity);
            return param.Id;
        }

        /// <summary>
        /// 下拉框
        /// </summary>
        /// <returns></returns>
        public async Task<List<RoleSelectDTO>> GetSelectAsync()
        {
            var list = await GetIQueryable(x => x.Code != AppConfig.Root).OrderByDescending(x => x.Sort).Select<RoleSelectDTO>().ToListAsync();
            return list;
        }

        /// <summary>
        ///  设置状态
        /// </summary>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool status) => await UpdateAsync(x => id == x.Id, x => new Role { Status = status });
    }
}