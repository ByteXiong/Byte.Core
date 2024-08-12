using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System.Linq.Expressions;
using Byte.Core.Common.Filters;

namespace Byte.Core.Repository
{
    public class MenuRepository : BaseRepository<Guid, Menu>
    {
        public MenuRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override Task<int> AddAsync(Menu entity)
        {
#if !DEBUG
            throw new BusException("线上环境不允许新增菜单");
#endif
            return base.AddAsync(entity);
        }
        public override Task<int> UpdateAsync(Menu entity, Expression<Func<Menu, object>> lstIgnoreColumns = null, bool isLock = true)
        {
#if !DEBUG
            throw new BusException("线上环境不允许编辑菜单");
#endif

            return base.UpdateAsync(entity, lstIgnoreColumns, isLock);
        }
        public override Task<int> DeleteAsync(Guid[] ids, bool isLock = true)
        {
#if !DEBUG
            throw new BusException("线上环境不允许删除菜单");
#endif
            return base.DeleteAsync(ids, isLock);
        }
    }
}
