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
    public class MenuLogic : BaseBusinessLogic<int, Menu, MenuRepository>
    {
        private readonly IUnitOfWork _unitOfWork;
        public readonly Role_MenuRepository   _role_MenuRepository;
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="repository"></param>
        public MenuLogic(MenuRepository repository, IUnitOfWork unitOfWork, Role_MenuRepository  role_MenuRepository) : base(repository)
        {
            _unitOfWork = unitOfWork;
            _role_MenuRepository = role_MenuRepository;
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
        public async Task<List<MenuSelectDTO>> GetTreeSelectAsync(int? parentId = null)
        {
            var tree = await GetIQueryable().OrderByDescending(x => x.Order).Select<MenuSelectDTO>().ToTreeAsync(it => it.Children, it => it.ParentId, parentId, it => it.Id);
            return tree;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<MenuInfo> GetInfoAsync(int id)
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
                //Status = false,
                //IsDeleted = false,
                //Roles = null,
                //Children = null

                },true).FirstAsync();

            entity.Buttons = await GetIQueryable(x => x.ParentId == id).Select(x => new MenuButton {
              Id = x.Id,
              Code = x.Path,
              Desc = x.Title,
              I18nKey=x.I18nKey,
              State= StateEnum.normal,
              ParentId=x.ParentId,
              Status=x.Status
            }).ToListAsync();

            return entity;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(UpdateMenuParam param)
        {

            if (param.ParentId == null && param.MenuType != MenuTypeEnum.目录)
            {
                throw new BusException("一级必须是目录");
            }
            param.Path =$"/{param.Name}" ;
            Menu model = param.Adapt<Menu>();
            model.Layout = "layout.base";
            try
            {
                _unitOfWork.BeginTran();
         
                await AddAsync(model);

                var addButtons = param.Buttons.Where(x => x.State != StateEnum.del).Select(x => new Menu
                {
                    MenuType = MenuTypeEnum.按钮,
                    Title = x.Desc,
                    Path = x.Code,
                    ParentId = model.Id,
                    I18nKey=x.I18nKey,
                    Status = x.Status
                }).ToList();
                await AddRangeAsync(addButtons);

            
                _unitOfWork.CommitTran();
         
            }
            catch (Exception ex) { 
                _unitOfWork.RollbackTran();
                throw ex;
            }finally
            {

                //清除缓存
                MemoryCacheManager.RemoveCacheRegex(ParamConfig.RoleCaChe);
            }
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(UpdateMenuParam param)
        {
            if (param.ParentId == null && param.MenuType != MenuTypeEnum.目录)
            {
                throw new BusException("一级必须是目录");
            }
            try
            {
                _unitOfWork.BeginTran();
                await UpdateAsync(x => x.Id == param.Id, x => new Menu
                {
                    Name = param.Name,
                    Component = param.Component,
                    Path = param.Path,
                    Icon = param.Icon,
                    LocalIcon = param.LocalIcon,
                    IconFontSize = param.IconFontSize,
                    Order = param.Order,
                    Href = param.Href,
                    HideInMenu = param.HideInMenu,
                    ActiveMenu = param.ActiveMenu,
                    MultiTab = param.MultiTab,
                    FixedIndexInTab = param.FixedIndexInTab,
                    PathParam = param.PathParam,
                    ParentId = param.ParentId,
                    MenuType = param.MenuType,
                    KeepAlive = param.KeepAlive,
                    Constant = param.Constant,
                    Title = param.Title,
                    I18nKey = param.I18nKey,
                    Redirect = param.Redirect,
                    Status = param.Status,
                });
               //新增/编辑
             var buttons = param.Buttons.Where(x => x.State != StateEnum.del).Select(x => new Menu
                {
                    Id = x.Id,
                    MenuType = MenuTypeEnum.按钮,
                    Title = x.Desc,
                    Path = x.Code,
                    ParentId = param.Id,
                }).ToList();
                await   _unitOfWork.GetDbClient().Storageable(buttons).ExecuteCommandAsync();
                //删除
                await DeleteAsync(param.Buttons.Where(x => x.State == StateEnum.del).Select(x => x.Id).ToArray());

                _unitOfWork.CommitTran();
            }
            catch (Exception ex)
            {
                _unitOfWork.RollbackTran();
                throw ex;
            }
            finally {
                //清除缓存
                MemoryCacheManager.RemoveCacheRegex(ParamConfig.RoleCaChe);
            }
            
            return param.Id;
           
        }


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool status) => await UpdateAsync(x => id == x.Id, x => new Menu { Status = status });




        /// <summary>
        /// 菜单下拉
        /// </summary>
        /// <returns></returns>
        public async Task<List<RouteSelectDTO>> SelectAsync()
        {

            var db = await GetIQueryable(x => x.Status).OrderByDescending(x => x.Order).Select(x => new RouteSelectDTO
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Title = x.Title,
            }).ToListAsync();
            var list = new List<RouteSelectDTO>();
            //递归
            db.ForEach(x =>
            {
                x.Children = db.Where(y => y.ParentId == x.Id).ToList();
                if (x.ParentId == default)
                {
                    list.Add(x);
                }
            });

            return list;
        }

        /// <summary>
        /// 通过角色Id获取菜单数组
        /// </summary>
        /// <param name="roleId"></param>
        /// <returns></returns>
        public async Task<int[]> GetByRoleIdAsync(int roleId)
        {
            var ids = await  _role_MenuRepository.GetIQueryable(x => x.RoleId == roleId).Select(x => x.MenuId).ToArrayAsync();
            return ids;
        }

        /// <summary>
        /// 通过角色Id添加菜单数组
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SetByRoleIdAsync(SetByRoleIdDTO param)
        {
            try
            {
                _unitOfWork.BeginTran();


                await _role_MenuRepository.DeleteAsync(x => x.RoleId == param.RoleId);
                var list = param.MenuIds.Select(x => new Role_Menu
                {
                    RoleId = param.RoleId,
                    MenuId = x
                }).ToList();
                await _role_MenuRepository.AddRangeAsync(list);
                //清除缓存
                MemoryCacheManager.RemoveCacheRegex(ParamConfig.RoleCaChe);
                _unitOfWork.CommitTran();
            }
            catch
            {

                _unitOfWork.RollbackTran();
                throw new BusException("更新失败", 301);
            }
        }







        /// <summary>
        /// 获取当前用户的路由
        /// </summary>
        /// <returns></returns>
        public async Task<List<RouteDTO>> GetRoutesAsync()
        {

            var code = CurrentUser.RoleCode;
            Expression<Func<Menu, bool>> where = x => x.Status;
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
                              //Status = x.Status,
                            
                          }).ToListAsync();
            List<RouteDTO> list = new List<RouteDTO>();

            db.ForEach(x =>
            {
                //x.Params = new Dictionary<string, dynamic>() { { "TableName", "User" } };
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