using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 字典数据
    /// </summary>
    public class DicDataRepository : BaseRepository<int, DicData>
    {
        public DicDataRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}