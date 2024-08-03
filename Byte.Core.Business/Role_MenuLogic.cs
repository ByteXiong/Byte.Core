using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Repository;

namespace Byte.Core.Business
{
    /// <summary>
    /// 角色-菜单
    /// </summary>
    public class Role_MenuLogic : BaseBusinessLogic<Guid, Role_Menu, Role_MenuRepository>
    {

        public Role_MenuLogic(Role_MenuRepository repository) : base(repository)
        {

        }
    }
}