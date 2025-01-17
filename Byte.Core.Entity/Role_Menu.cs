using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 角色-菜单
    /// </summary>
    [SugarTable("byte_role_menu")]
    
    public class Role_Menu
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long MenuId { get; set; }


        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public long RoleId { get; set; }
    }

    //[JsonIgnore]//隐藏
}
