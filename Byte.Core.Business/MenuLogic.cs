using Byte.Core.Common.Cache;
using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
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
    /// 角色-菜单
    /// </summary>
    public class MenuLogic : BaseBusinessLogic<Guid, Menu, MenuRepository>
    {
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="repository"></param>
        public MenuLogic(MenuRepository repository) : base(repository)
        {

        }




        /// <summary>
        /// 列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<MenuTreeDTO>> GetTreeAsync()
        {

            var tree = await GetIQueryable().OrderByDescending(x => x.Order).Select<MenuTreeDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, null, it => it.Id);
            return tree;
        }

        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="parentId"></param>
        /// <returns></returns>
        public async Task<List<MenuSelectDTO>> GetTreeSelectAsync(Guid? parentId = null)
        {
            var tree = await GetIQueryable().OrderByDescending(x => x.Order).Select<MenuSelectDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, parentId, it => it.Id);
            return tree;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MenuInfo> GetInfoAsync(Guid id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select(x => new MenuInfo
                {

                //Title = x.Title,
                //Path = null,
                //Component = null,
                //ComponentName = null,
                //ParentId = null,
                //Sort = 0,
                //Icon = null,
                //Type = (MenuTypeEnum)0,
                //KeepAlive = false,
                //Hidden = false,
                //Redirect = null,
                //AlwaysShow = false,
                //State = false,
                //IsDeleted = false,
                //Roles = null,
                //Children = null

                },true).FirstAsync();

            return entity;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(UpdateMenuParam param)
        {

            if (param.ParentId == null && param.Type != MenuTypeEnum.目录)
            {
                throw new BusException("一级必须是目录");
            }

            Menu model = param.Adapt<Menu>();
            model.Component = param.Type == MenuTypeEnum.目录 ? "Layout" : param.Component;
            await AddAsync(model);
            //清除缓存
            MemoryCacheManager.RemoveCacheRegex(ParamConfig.RoleCaChe);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateAsync(UpdateMenuParam param)
        {
            if (param.ParentId == null && param.Type != MenuTypeEnum.目录)
            {
                throw new BusException("一级必须是目录");
            }

            await UpdateAsync(x => x.Id == param.Id, x => new Menu
            {
                Name = param.Name,
                Component = param.Component,
                Path = param.Path,
                Icon = param.Icon,
                LocalIcon = param.LocalIcon,
                IconFontSize = param.IconFontSize,
                Order =  param.Order,
                Href = param.Href,
                HideInMenu = param.HideInMenu,
                ActiveMenu = param.ActiveMenu,
                MultiTab = param.MultiTab,
                FixedIndexInTab = param.FixedIndexInTab,
                Query = param.Query,
                ParentId = param.ParentId,
                Type = param.Type,
                KeepAlive = param.KeepAlive,
                Constant = param.Constant,
                Title = param.Title,
                I18nKey = param.I18nKey,
                Redirect = param.Redirect,
                State = param.State,
            });
            //清除缓存
            MemoryCacheManager.RemoveCacheRegex(ParamConfig.RoleCaChe);
            return param.Id;
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<int> SetStateAsync(Guid id, bool state) => await UpdateAsync(x => id == x.Id, x => new Menu { State = state });





        /// <summary>
        /// 获取当前用户的路由
        /// </summary>
        /// <returns></returns>
        public async Task<List<RouteDTO>> GetRoutesAsync()
        {

            var code = CurrentUser.RoleCode;
            Expression<Func<Menu, bool>> where = x => x.State==true && x.Type != MenuTypeEnum.按钮;
            if (code != ParamConfig.Admin)
            {
                where = where.And(x => x.Roles.Any(y => y.Code == code));
            }

            var db = await GetIQueryable(where)
                         .Includes(x => x.Roles).OrderByDescending(x => x.Order).Select(x =>
                          new RouteDTO()
                          {
                              Id = x.Id,

                              Meta = new RouteMeta
                              {
                                  Icon = x.Icon,
                                  //LocalIcon = x.LocalIcon,
                                  //IconFontSize = x.IconFontSize,
                                  //Order = x.Order,
                                  //Href = x.Href,
                                  //HideInMenu = x.HideInMenu,
                                  //ActiveMenu = x.ActiveMenu,
                                  //MultiTab = x.MultiTab,
                                  //FixedIndexInTab = x.FixedIndexInTab,
                                  //Query = x.Query,
                                  //KeepAlive = x.KeepAlive,
                                  Constant = x.Constant,
                                  Title = x.Title,
                                  I18nKey = x.I18nKey,
                              },
                              Name = x.Name,
                              Component = x.Component,
                              Path = x.Path,
                              ParentId = x.ParentId,
                              //Type = x.Type,
                              Redirect = x.Redirect,
                              //State = x.State,
                            
                          }).ToListAsync();
            List<RouteDTO> list = new List<RouteDTO>();

            db.ForEach(x =>
            {
                x.Params = new Dictionary<string, dynamic>() { { "TableName", "User" } };
                x.Children = db.Where(y => y.ParentId == x.Id).ToList();
                if (x.ParentId == null)
                {
                    list.Add(x);
                }
            });
            return list;
        }

    }
}