using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    [SugarTable("Byte_Role")]
    
    public class Role : BaseEntity<int>
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Name { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public RoleTypeEnum Type { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Remark { get; set; }


        ///// <summary>
        ///// 数据权限
        ///// </summary>
        //[SugarColumn(Length = 50, IsNullable = false)]
        //public string DataScope { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = false)]
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Sort { get; set; }
        /// <summary>
        /// 公司id
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public int DeptId { get; set; }

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(DeptId))]
        public Dept Dept { get; set; }



        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(Role_Menu), nameof(Role_Menu.RoleId), nameof(Role_Menu.MenuId))]
        public List<Menu> Menus { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.RoleId), nameof(User_Dept_Role.UserId))]
        public List<User> Users { get; set; }
        #endregion
    }

    //[JsonIgnore]//隐藏
}
