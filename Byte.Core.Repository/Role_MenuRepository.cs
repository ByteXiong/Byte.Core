using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 角色-菜单
    /// </summary>
    public class Role_MenuRepository : BaseRepository<long, Role_Menu>
    {
        public Role_MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}