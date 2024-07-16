using Byte.Core.Common;
using Byte.Core.SqlSugar;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 菜单
    /// </summary>
    [SugarTable("Menu")]
    public class Menu : BaseEntity<Guid>
    {
   
        /// <summary>
        /// 状态
        /// </summary>
        public MenuType Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }


        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }


        /// <summary>
        /// 菜单名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Name { get; set; }


        /// <summary>
        /// 路径
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Path { get; set; }


        /// <summary>
        /// 组件
        /// </summary>
        [SugarColumn(Length = 255, IsNullable = true)]
        public String Component { get; set; }


        /// <summary>
        /// 是否清除缓存
        /// </summary>
        public Boolean NoCache { get; set; }


        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public String Icon { get; set; }


        /// <summary>
        /// 是否隐藏
        /// </summary>
        public Boolean Hidden { get; set; }


        /// <summary>
        /// 是否显示面包屑
        /// </summary>
        public Boolean Breadcrumb { get; set; }


        /// <summary>
        /// 高亮菜单
        /// </summary>
        public Boolean AlwaysShow { get; set; }


        /// <summary>
        /// 是否固定在标签页
        /// </summary>
        public Boolean Affix { get; set; }


        /// <summary>
        /// 是否按钮权限
        /// </summary>
        public Boolean IsPermission { get; set; }


        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Title { get; set; }


        /// <summary>
        /// 是否可跳转
        /// </summary>
        public Boolean CanTo { get; set; }


     

        /// <summary>
        /// 排序
        /// </summary>
        public Int32 Sort { get; set; }
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
