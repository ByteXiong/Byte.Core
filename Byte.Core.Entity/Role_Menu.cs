using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 角色-菜单
    /// </summary>
    [SugarTable("Role_Menu")]
    public class Role_Menu
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid MenuId { get; set; }


        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public Guid RoleId { get; set; }
    }

    //[JsonIgnore]//隐藏
}
