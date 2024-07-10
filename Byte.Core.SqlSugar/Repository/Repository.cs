using SqlSugar;
using System.Linq.Expressions;
using System.Reflection;

namespace Byte.Core.SqlSugar;

/// <summary>
/// SqlSugar仓储
/// </summary>
/// <typeparam name="T"></typeparam>
public class Repository<T> : BaseRepository<T>, IRepository<T> where T : class, new()
{
    public Repository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
