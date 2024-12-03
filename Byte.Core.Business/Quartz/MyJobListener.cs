using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Common.SnowflakeIdHelper;
using Byte.Core.Entity;
using Microsoft.AspNetCore.Authentication;
using Quartz;
using System;
using static Quartz.Logging.OperationName;

namespace Byte.Core.Business.Quartz
{
    public class MyJobListener : IJobListener
    {
        private readonly JobDetailLogic _jobDetailLogic = ServiceLocator.Resolve<JobDetailLogic>();
        private readonly JobTriggerLogic _jobTriggerLogic = ServiceLocator.Resolve<JobTriggerLogic>();
        private readonly JobTriggerRecordLogic _jobTriggerRecordLogic = ServiceLocator.Resolve<JobTriggerRecordLogic>();

        public string Name => "MyJobListener";

        public Task JobExecutionVetoed(IJobExecutionContext context, CancellationToken cancellationToken = default)
        {

            Console.WriteLine("任务执行被否决，原因：{0}", context.Result.ToString());
            return Task.CompletedTask;
        }

        public async Task JobToBeExecuted(IJobExecutionContext context, CancellationToken cancellationToken = default)
            {

            var nextRunTime = context.NextFireTimeUtc?.LocalDateTime;


            Console.WriteLine("任务即将执行，任务名称：{0}", context.JobDetail.Key.Name);
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var result = "";
             var triggerId = context.Trigger.Key.Name.ToLong();
            var nextRunTime = context.NextFireTimeUtc?.ToUnixTimeSeconds();
            if (jobException != null)
            {
                // 获取错误信息
                result = jobException.Message;
                await   _jobTriggerLogic.UpdateAsync(x=>x.Id==triggerId, x=> new JobTrigger() 
                {
                    NumRetries = context.RefireCount,
                    NumberOfRuns = +1,
                    NumberOfErrors = +1,
                    NextRunTime = nextRunTime

                });
             
            }
            else {

                result = context.Result?.ToString();
                await _jobTriggerLogic.UpdateAsync(x => x.Id == triggerId, x => new JobTrigger()
                {
                    NumRetries = context.RefireCount,
                    NextRunTime = nextRunTime
                });
            }
            var recordId = context.JobDetail.JobDataMap.GetLong("recordId");
            TimeSpan executionTime = (context.NextFireTimeUtc ?? DateTimeOffset.Now) - context.FireTimeUtc;
            await  _jobTriggerRecordLogic.UpdateAsync(x => x.Id == recordId, x =>new JobTriggerRecord() { Result = result, ElapsedTime= executionTime.Milliseconds });
            Console.WriteLine("任务执行完成，执行时间：{0} 毫秒", executionTime.Milliseconds);
        }
    }
}
