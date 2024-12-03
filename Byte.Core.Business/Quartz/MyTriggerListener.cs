using Byte.Core.Common.Extensions;
using Byte.Core.Common.IoC;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Quartz;

namespace Byte.Core.Business.Quartz
{
    public class MyTriggerListener : ITriggerListener
    {
        private readonly IUnitOfWork  _unitOfWork = ServiceLocator.Resolve<IUnitOfWork>();
        private readonly JobDetailLogic _jobDetailLogic = ServiceLocator.Resolve<JobDetailLogic>();
        private readonly JobTriggerLogic _jobTriggerLogic = ServiceLocator.Resolve<JobTriggerLogic>();
        private readonly JobTriggerRecordLogic _jobTriggerRecordLogic = ServiceLocator.Resolve<JobTriggerRecordLogic>();

        public string Name => "MyTriggerListener";

        public Task TriggerComplete(ITrigger trigger, IJobExecutionContext context, SchedulerInstruction triggerInstructionCode, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("触发器完成，触发器名称：{0}", trigger.Key.Name);
            return Task.CompletedTask;
        }

        public Task TriggerFired(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("触发器被触发，触发器名称：{0}", trigger.Key.Name);
            return Task.CompletedTask;
        }

        public Task TriggerMisfired(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("触发器错过触发时间，触发器名称：{0}", trigger.Key.Name);
            return Task.CompletedTask;
        }

        public async Task<bool> VetoJobExecution(ITrigger trigger, IJobExecutionContext context, CancellationToken cancellationToken = default)
        {
            //获取当前触发器 执行次数
            var triggerId = trigger.Key.Name.ToLong();
            var numberOfRuns = await _jobTriggerLogic.GetIQueryable(x=>x.Id== triggerId).Select(x=>x.NumberOfRuns).FirstAsync();

          
            await _jobTriggerLogic.UpdateAsync(x=> x.Id == triggerId, x=>new JobTrigger() { 
                NumberOfRuns = numberOfRuns + 1,
                LastRunTime = context.FireTimeUtc.ToUnixTimeSeconds(), 
            });

    

            var jobTriggerRecord = new JobTriggerRecord()
            {
                TriggerId = triggerId,
                NumberOfRuns = numberOfRuns + 1,
                LastRunTime = context.FireTimeUtc.LocalDateTime,
                NextRunTime = context.NextFireTimeUtc?.LocalDateTime,
            };
            await _jobTriggerRecordLogic.AddAsync(jobTriggerRecord);

            context.JobDetail.JobDataMap.Put("recordId", jobTriggerRecord.Id);

            Console.WriteLine("任务执行前事件通知，任务名称：{0}", context.JobDetail.Key.Name);
            await Task.Delay(0, cancellationToken);
            return false;
            //return context.();
        }
    }
}
