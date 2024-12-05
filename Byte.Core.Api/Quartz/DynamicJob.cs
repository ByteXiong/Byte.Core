using Microsoft.Extensions.DependencyInjection;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Api.Quartz
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

        public async Task Execute(IJobExecutionContext context)
        {
            //JobDataMap dataMap = context.JobDetail.JobDataMap;
            //string type = dataMap.GetString("type");
            JobDataMap dataMap = context.Trigger.JobDataMap;
            string type = dataMap.GetString("type");

            var isRandNum = dataMap.TryGetInt("randNum", out int randNum);
            if (!isRandNum) randNum = 1;

            for (int i = 0; i < randNum; i++)
            {
                var date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Random rand = new Random();
                int randomNumber = rand.Next(1000, 9999);
                //await Task.Delay(randomNumber);
                Console.WriteLine("动态作业任务" + type + "时间:" + date + "随机数:" + randomNumber + "线程:" + Thread.CurrentThread.ManagedThreadId + "当前时间" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            }
        }

    }
}
