using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
namespace Byte.Core.Models
{
    /// <summary>
    ///  用户分页查询
    /// </summary>
    public class UserParam : PageParam
    {
        // public string KeyWord { get; set; }	


        public string? KeyWord { get; set; }

        /// <summary>
        /// 公司
        /// </summary>
        public Guid? DeptId { get; set; }
    }
    /// <summary>
    ///  用户 修改
    /// </summary>
    public class UpdateUserParam : AddUserParam
    {

    }

    /// <summary>
    ///  用户 添加
    /// </summary>
    public class AddUserParam : User
    {
        public Guid[]? RoleIds { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class UserDTO
    {
       
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public String Avatar { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }




        /// <summary>
        /// 状态
        /// </summary>

        public bool State { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        public String Account { get; set; }



    }


    /// <summary>
    /// 用户 详情
    /// </summary>
    public class UserInfo : User
    {

    }
    /// <summary>
    /// 设置密码
    /// </summary>
    public class SetPasswordParam { 
        /// <summary>
        /// Id
        /// </summary>
    public  Guid Id { get; set; }
        /// <summary>
        /// 旧密码
        /// </summary>
    public string OldPassword { get; set; } 
        /// <summary>
        /// 新密码
        /// </summary>
    public string NewPassword { get; set; }
    }
}
