using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
namespace Byte.Core.Models
{
    /// <summary>
    ///  角色分页查询
    /// </summary>
    public class RoleParam : PageParam
    {
        public string KeyWord { get; set; }
    }
    /// <summary>
    ///  角色 修改
    /// </summary>
    public class UpdateRoleParam : AddRoleParam
    {

        /// <summary>
        /// 菜单
        /// </summary>
        public Guid[]? MenuIds { get; set; }
    }

    /// <summary>
    ///  角色 添加
    /// </summary>
    public class AddRoleParam : Role
    {

    }

    /// <summary>
    /// 角色
    /// </summary>
    public class RoleDTO : Role
    {
        public string DeptName { get; set; }
    }

    /// <summary>
    /// 角色
    /// </summary>
    public class RoleSelectDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        public String Name { get; set; }


    }

    /// <summary>
    /// 角色 详情
    /// </summary>
    public class RoleInfo : Role
    {

        public Guid[] MenuIds { get; set; }

    }
}
