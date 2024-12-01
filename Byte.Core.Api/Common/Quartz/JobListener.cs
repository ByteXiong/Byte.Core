using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Common.SnowflakeIdHelper;
using Byte.Core.Entity;
using Byte.Core.Repository;
using Quartz;
using System;
using static Quartz.Logging.OperationName;

namespace Byte.Core.Api.Common.Quartz
{
    public class JobListener : IJobListener
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
             var id = IdHelper.GetLongId();
            context.JobDetail.JobDataMap.Put("recordId", id);
            Console.WriteLine("任务即将执行，任务名称：{0}", context.JobDetail.Key.Name);
            var jobTriggerRecord = new JobTriggerRecord()
            {
                Id = id,
                LastRunTime= context.FireTimeUtc.LocalDateTime,
                NextRunTime = context.NextFireTimeUtc?.LocalDateTime,
            };
            await  _jobTriggerRecordLogic.AddAsync(jobTriggerRecord);
        }

        public async Task JobWasExecuted(IJobExecutionContext context, JobExecutionException jobException, CancellationToken cancellationToken = default)
        {
            var result = context.Result.ToString();
            if (jobException != null)
            {
                // 获取错误信息
                result = jobException.Message;
            }
            var recordId = context.JobDetail.JobDataMap.GetLong("recordId");
            TimeSpan executionTime = (context.NextFireTimeUtc ?? DateTimeOffset.Now) - context.FireTimeUtc;
            await  _jobTriggerRecordLogic.UpdateAsync(x => x.Id == recordId, x =>new JobTriggerRecord() { Result = result, ElapsedTime= executionTime.Milliseconds });
            Console.WriteLine("任务执行完成，执行时间：{0} 毫秒", executionTime.Milliseconds);
        }
    }
}
