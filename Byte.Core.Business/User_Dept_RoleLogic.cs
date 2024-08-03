using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;

namespace Byte.Core.Business
{
    /// <summary>
    /// 用户-角色
    /// </summary>
    public class User_Dept_RoleLogic : BaseBusinessLogic<Guid, User_Dept_Role, User_Dept_RoleRepository>
    {
        public User_Dept_RoleLogic(User_Dept_RoleRepository repository) : base(repository)
        {
        }
    }
}