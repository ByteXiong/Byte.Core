using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{
    public class JobDetailDTO
    {
        /// <summary>
        /// 组名称
        /// </summary>
        public string GroupName { get; set; }
        /// <summary>
        /// 程序集Name
        /// </summary>

        public string AssemblyName { get; set; }
    }
    public  class JobDetailParam : PageParam
    {

    }
    public class JobDetailInfo
    {

    }
    public class UpdateJobDetailParam: JobDetail
    { 
    
    }
}
