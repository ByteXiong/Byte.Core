using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 用户-角色
    /// </summary>
    public class User_Dept_RoleRepository : BaseRepository<Guid, User_Dept_Role>
    {
        public User_Dept_RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}