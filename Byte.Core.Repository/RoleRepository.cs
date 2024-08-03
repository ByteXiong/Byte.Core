using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 角色
    /// </summary>
    public class RoleRepository : BaseRepository<Guid, Role>
    {
        public RoleRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}