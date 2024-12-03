using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business.Quartz
{
    /// <summary>
    /// 系统作业集群表
    /// </summary>
    public class JobClusterLogic : BaseBusinessLogic<long, JobCluster, JobClusterRepository>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public JobClusterLogic(JobClusterRepository repository) : base(repository)
        {
        }
    }
}
