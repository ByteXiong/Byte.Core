using Byte.Core.Entity;
using Byte.Core.SqlSugar;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 系统作业集群表
    /// </summary>
    public class JobClusterRepository : BaseRepository<long, JobCluster>
    {
        public JobClusterRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
