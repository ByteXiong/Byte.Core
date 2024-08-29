using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 测试表
    /// </summary>
    public class RedisDemoRepository : BaseRepository<Guid, RedisDemo>
    {
        public RedisDemoRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

      
    }
}
