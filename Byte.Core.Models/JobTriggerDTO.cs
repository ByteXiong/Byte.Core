using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{
    public class JobTriggerParam : PageParam
    {

        public  string KeyWord { get; set; }
        
        public long?  JobId { get; set; }

    }
    public class JobTriggerDTO : JobTrigger
    {

    }


    public class JobTriggerInfo : JobTrigger
    {
    }

    public class UpdateJobTriggerParam : AddJobTriggerParam
    {
    }

    public class AddJobTriggerParam : JobTrigger
    {
    }
}
