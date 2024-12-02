using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business.Quartz
{
    /// <summary>
    /// 动态作业任务
    /// </summary>
    //[JobDetail("你的作业编号")]
    public class DynamicJob : IJob
    {

        public DynamicJob()
        {
        }

        public Task Execute(IJobExecutionContext context)
        {
            //JobDataMap dataMap = context.JobDetail.JobDataMap;
            //string type = dataMap.GetString("type");
            JobDataMap dataMap = context.Trigger.JobDataMap;
            string type = dataMap.GetString("type");
            Console.WriteLine("动态作业任务"+ type);
            context.Result ="动态作业任务"+ type;
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }

    }
}
