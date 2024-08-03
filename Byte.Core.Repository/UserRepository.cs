using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserRepository : BaseRepository<Guid, User>
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}