using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using Byte.Core.Common.Filters;
using System.Linq.Expressions;
using Byte.Core.Common.Extensions;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 用户
    /// </summary>
    public class UserRepository : BaseRepository<int, User>
    {
        public UserRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public override async Task<int> AddRangeAsync(List<User> entitys)
        {
            var accounts = entitys.Select(x => x.UserName).ToList();
            var users = await GetIQueryable(x => accounts.Contains(x.UserName)).Select(x => x.UserName).ToArrayAsync();
            if (users.Count() > 0)
            {
                throw new BusException($"{string.Join(",", users)}已存在");
            }

            return await base.AddRangeAsync(entitys);
        }

        public override async Task<int> AddAsync(User entity)
        {
            await OnlyAccountAsync(entity.UserName);
            return await base.AddAsync(entity);
        }

        public override async Task<int> UpdateAsync(User entity, Expression<Func<User, object>> lstIgnoreColumns = null, bool isLock = true)
        {
            await OnlyAccountAsync(entity.UserName, entity.Id);
            return await base.UpdateAsync(entity, lstIgnoreColumns, isLock);
        }

        /// <summary>
        /// 账号唯一
        /// </summary>
        /// <returns></returns>
        private async Task OnlyAccountAsync(string userName, int? id = default)
        {

            Expression<Func<User, bool>> where = x => x.UserName == userName;
            if (id != null)
            {
                where = where.And(x => x.Id != id);
            }
            var any = await GetIQueryable(where).AnyAsync();
            if (any) throw new BusException("账号已存在");
        }
    }
}