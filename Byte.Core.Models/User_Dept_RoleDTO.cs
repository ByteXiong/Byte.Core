using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System.ComponentModel.DataAnnotations;
namespace Byte.Core.Models
{
    /// <summary>
    ///  用户-角色分页查询
    /// </summary>
    public class User_Dept_RoleParam : PageParam
    {
        // public string KeyWord { get; set; }	
    }
    /// <summary>
    ///  用户-角色 修改
    /// </summary>
    public class UpdateUser_Dept_RoleParam : AddUser_Dept_RoleParam
    {


    }

    /// <summary>
    ///  用户-角色 添加
    /// </summary>
    public class AddUser_Dept_RoleParam : User_Dept_Role
    {

    }

    /// <summary>
    /// 用户-角色
    /// </summary>
    public class User_Dept_RoleDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        [Required]
        public int UserId { get; set; }


        /// <summary>
        /// 角色Id
        /// </summary>
        [Required]
        public int RoleId { get; set; }



    }


    /// <summary>
    /// 用户-角色 详情
    /// </summary>
    public class User_Dept_RoleInfo : User_Dept_Role
    {


    }
}
