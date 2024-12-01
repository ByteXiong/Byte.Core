using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Repository
{

    /// <summary>
    /// 系统作业信息表
    /// </summary>
    public class JobTriggerRepository : BaseRepository<long, JobTrigger>
    {
        public JobTriggerRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
