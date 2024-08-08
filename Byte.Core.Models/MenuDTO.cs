using Byte.Core.Common;
using Byte.Core.Entity;
using Byte.Core.Tools;
using System.Drawing.Drawing2D;
using Byte.Core.Tools;
using SqlSugar;
namespace Byte.Core.Models
{
    /// <summary>
    ///  菜单 修改
    /// </summary>
    public class UpdateMenuParam : AddMenuParam
    {


    }

    /// <summary>
    ///  菜单 添加
    /// </summary>
    public class AddMenuParam : Menu
    {

    }
    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuTreeDTO 
    {

        public Guid Id { get; set; }
        /// <summary>
        /// 菜单标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string ComponentName { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// icon图标
        /// </summary>
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// 1.目录 2.菜单 3.按钮
        /// </summary>
        public MenuTypeEnum Type { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool Hidden { get; set; }
        /// <summary>
        /// 跳转路由
        /// </summary>
        public string Redirect { get; set; }
        /// <summary>
        /// 根目录始终显示 
        /// </summary>
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }


       
        public  List<MenuTreeDTO> Children { get; set; }

    }


    /// <summary>
    /// 菜单
    /// </summary>
    public class MenuSelectDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public String Title { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public String Icon { get; set; }
        public List<MenuSelectDTO>? Children { get; set; }
    }


    /// <summary>
    /// 菜单 详情
    /// </summary>
    public class MenuInfo : Menu
    {

        //public List<Permission> PermissionList { get; set; }
    }

    /// <summary>
    /// 当前系统我的所有权限
    /// </summary>

    public class RouteDTO
    {
        public Guid Id { get; set; }
        /// <summary>
        /// 路径
        /// </summary>
        public string Path { get; set; }


        /// <summary>
        /// 组件
        /// </summary>
        public string Component { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 父级ID
        /// </summary>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 类型
        /// </summary>
        public MenuTypeEnum Type { get; set; }
        /// <summary>
        /// 跳转路由
        /// </summary>
        public string Redirect { get; set; }

        ///// <summary>
        ///// 缓存
        ///// </summary>
        //public bool Cache { get; set; }

        ///// <summary>
        ///// 子节点个数
        ///// </summary>
        //public int SubCount { get; set; }

        public RouteMeta Meta { get; set; }

        /// <summary>
        /// 子节点
        /// </summary>
        public List<RouteDTO> Children { get; set; }
    }
    public class RouteMeta
    {
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 隐藏
        /// </summary>
        public bool Hidden { get; set; }


        /// <summary>
        /// Icon
        /// </summary>
        public string Icon { get; set; }
        /// <summary>
        /// 是否缓存
        /// </summary>
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 根目录始终显示 
        /// </summary>
        public bool AlwaysShow { get; set; }
        /// <summary>
        /// 权限标识
        /// </summary>
        public string[] Roles => new string[] { CurrentUser.RoleCode };
    }

}
