using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
namespace Byte.Core.Models
{
    /// <summary>
    ///  角色-菜单分页查询
    /// </summary>
    public class Role_MenuParam : PageParam
    {
        // public string KeyWord { get; set; }	
    }
    /// <summary>
    ///  角色-菜单 修改
    /// </summary>
    public class UpdateRole_MenuParam : AddRole_MenuParam
    {


    }

    /// <summary>
    ///  角色-菜单 添加
    /// </summary>
    public class AddRole_MenuParam : Role_Menu
    {

    }

    /// <summary>
    /// 角色-菜单
    /// </summary>
    public class Role_MenuTreeDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 菜单Id
        /// </summary>
        [Required]
        public long MenuId { get; set; }


        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public long RoleId { get; set; }



    }


    /// <summary>
    /// 角色-菜单 详情
    /// </summary>
    public class Role_MenuInfo : Role_Menu
    {


    }
}
