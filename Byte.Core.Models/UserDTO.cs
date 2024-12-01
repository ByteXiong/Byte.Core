using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Newtonsoft.Json;
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
        public long? DeptId { get; set; }
    }
    /// <summary>!
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
        public List<long>? RoleIds { get; set; }
    }

    /// <summary>
    /// 用户
    /// </summary>
    public class UserDTO
    {
       
        public long? Id { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        public String NickName { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public String Avatar { get; set; }


        /// <summary>
        /// 手机号
        /// </summary>
        public String Phone { get; set; }

        /// <summary>
        /// 电子邮箱
        /// </summary>
        public String Email { get; set; }
        

        /// <summary>
        /// 状态
        /// </summary>

        public bool Status { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        public String UserName { get; set; }

    }


    /// <summary>
    /// 用户 详情
    /// </summary>
    public class UserInfo : User
    {
        public List<long>? RoleIds { get; set; }
    }
    /// <summary>
    /// 设置密码
    /// </summary>
    public class SetPasswordParam { 
        /// <summary>
        /// Id
        /// </summary>
    public long Id { get; set; }
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
