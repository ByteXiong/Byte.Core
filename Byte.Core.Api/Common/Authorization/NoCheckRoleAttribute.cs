using Byte.Core.Common.Attributes;

namespace Byte.Core.Api.Attributes
{
    /// <summary>
    /// 忽略权限
    /// </summary>
    public class NoCheckRoleAttribute : BaseActionFilter
    {
    }
    /// <summary>
    /// Get鉴权
    /// </summary>
    public class GetCheckRoleAttribute : BaseActionFilter
    {
    }
}
