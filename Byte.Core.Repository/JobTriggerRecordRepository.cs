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
    public class JobTriggerRecordRepository : BaseRepository<long, JobTriggerRecord>
    {
        public JobTriggerRecordRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
