using Byte.Core.Common;
using Byte.Core.SqlSugar;
using SqlSugar;
using System.Drawing.Drawing2D;
using Byte.Core.Tools;
namespace Byte.Core.Entity
{
    /// <summary>
    /// 菜单
    /// </summary>
    [SugarTable("Menu")]
    public class Menu : BaseEntity<Guid>
    {
        /// <summary>
        /// 菜单标题
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public string Title { get; set; }

        /// <summary>
        /// 组件路径
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Path { get; set; }

        /// <summary>
        /// 权限标识符
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Perm { get; set; }

        /// <summary>
        /// 是否iframe
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public bool IFrame { get; set; }

        /// <summary>
        /// 组件
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Component { get; set; }

        /// <summary>
        /// 组件名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string ComponentName { get; set; }

        /// <summary>
        /// 父级菜单ID
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public Guid? ParentId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Sort { get; set; }

        /// <summary>
        /// icon图标
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Icon { get; set; }

        /// <summary>
        /// 类型
        /// 1.目录 2.菜单 3.按钮
        /// </summary>
        [SugarColumn(IsNullable = false)]
        public MenuTypeEnum Type { get; set; }

        /// <summary>
        /// 是否缓存
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool KeepAlive { get; set; }

        /// <summary>
        /// 是否隐藏
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public bool Hidden { get; set; }
        /// <summary>
        /// 跳转路由
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Redirect { get; set; }
        /// <summary>
        /// 根目录始终显示 
        /// </summary>
        public bool AlwaysShow { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 是否已删除
        /// </summary>
        public bool IsDeleted { get; set; }

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(Role_Menu), nameof(Role_Menu.MenuId), nameof(Role_Menu.RoleId))]
        public List<Role> Roles { get; set; }
        [SugarColumn(IsIgnore = true)]
        public List<Menu> Children { get; set; }
        #endregion
    }

    //[JsonIgnore]//隐藏
}
