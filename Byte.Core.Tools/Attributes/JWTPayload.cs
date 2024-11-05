
using Byte.Core.Common.Attributes;
namespace Byte.Core.Tools.Attributes
{
    /// <summary>
    /// 表示 JWT 负载信息的类
    /// </summary>
    public class JWTPayload : JWTPayloadBase
    {
        /// <summary>
        /// 用户的唯一标识符
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 用户的账户
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 用户的姓名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 用户的角色类型，可为空
        /// </summary>
        public RoleTypeEnum? RoleType { get; set; }

        /// <summary>
        /// 用户所属部门的唯一标识符
        /// </summary>
        public Guid DeptId { get; set; }

        /// <summary>
        /// 用户的角色代码
        /// </summary>
        public string RoleCode { get; set; }
    }
}
