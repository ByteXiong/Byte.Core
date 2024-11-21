using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System.Linq.Expressions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Cache;
using Byte.Core.Tools;
using System.Drawing.Drawing2D;
using SqlSugar;
using Byte.Core.Common.Extensions;

namespace Byte.Core.Repository
{
    public class MenuRepository : BaseRepository<int, Menu>
    {
        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Task<int> AddAsync(Menu entity)
        {
#if !DEBUG
            throw new BusException("线上环境不允许新增菜单");
#endif
            return base.AddAsync(entity);
        }
        public override Task<int> UpdateAsync(Menu entity, Expression<Func<Menu, object>> lstIgnoreColumns = null, bool isLock = true)
        {
#if !DEBUG
            throw new BusException("线上环境不允许编辑菜单");
#endif

            return base.UpdateAsync(entity, lstIgnoreColumns, isLock);
        }
        public override async Task<int> DeleteAsync(int[] ids, bool isLock = true)
        {
#if !DEBUG
            throw new BusException("线上环境不允许删除菜单");
#endif

            var any = await SugarClient.DeleteNav<Menu>(x => ids.Contains(x.Id)).Include(x => x.Roles, new DeleteNavOptions() { ManyToManyIsDeleteA = true }).ExecuteCommandAsync();
            //清除缓存
            MemoryCacheManager.RemoveCacheRegex(AppConfig.RoleCaChe);
            return any ? 1 : 0;
        }

        #region 缓存管理

        /// <summary>
        /// 获取角色拥有的权限
        /// </summary>
        /// <param name="roleCode"></param>
        /// <returns></returns>
        public async Task<List<string>> GetPermAsync(List<string> roleCodes)
        {
            var arrs = new List<string>();
            roleCodes.ForEach(async roleCode =>
            {
                var key = AppConfig.RoleButtonCaChe + roleCode;
                var any = MemoryCacheManager.Exists(key);
                List<string> arr;
                if (any)
                {
                    arr = MemoryCacheManager.Get<List<string>>(key);
                }
                else
                {
                    Expression<Func<Menu, bool>> where = x => x.MenuType == MenuTypeEnum.按钮 && x.Roles.Any(y => y.Code == roleCode);
                    if (roleCode == AppConfig.Admin)
                    {
                        where = x => x.MenuType == MenuTypeEnum.按钮;
                    }


                    arr = SugarClient.Queryable<Menu>().Where(where).Select(x => x.Path).ToList();
                    MemoryCacheManager.Set(key, arr, 60 * 30);
                    arrs.AddRange(arr);
                }
            });  
            return arrs.Distinct().ToList();
        }
        #endregion
    }
}
