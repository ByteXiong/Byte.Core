using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;

namespace Byte.Core.Business
{
    /// <summary>
    /// 角色-菜单
    /// </summary>
    public class RoleMenuLogic : BaseBusinessLogic<long, Role_Menu, Role_MenuRepository>
    {
        /// <summary />
        /// <param name="repository"></param>
        public RoleMenuLogic(Role_MenuRepository repository) : base(repository)
        {

        }
    }
}