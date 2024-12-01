using Byte.Core.Common.IoC;
using Byte.Core.Repository;
using Byte.Core.SqlSugar.IDbContext;
using Quartz;
using System;
using static Quartz.Logging.OperationName;

namespace Byte.Core.Api.Common.Quartz
{
    public class SchedulerListener : ISchedulerListener
    {
        private readonly JobDetailLogic _jobDetailLogic = ServiceLocator.Resolve<JobDetailLogic>();
        private readonly JobTriggerLogic _jobTriggerLogic = ServiceLocator.Resolve<JobTriggerLogic>();




        public Task JobScheduled(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"任务 {trigger.JobKey} 已经被调度");
            return Task.CompletedTask;
        }

        public Task JobUnscheduled(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"任务 {triggerKey} 已经被取消调度");
            return Task.CompletedTask;
        }

        public Task TriggerFinalized(ITrigger trigger, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"触发器 {trigger.Key} 已经完成");
            return Task.CompletedTask;
        }

        public Task TriggerPaused(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"触发器 {triggerKey} 已经被暂停");
            return Task.CompletedTask;
        }

        public Task TriggersPaused(string triggerGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("所有触发器已经被暂停");
            return Task.CompletedTask;
        }

        public Task TriggerResumed(TriggerKey triggerKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"触发器 {triggerKey} 已经被恢复");
            return Task.CompletedTask;
        }

        public Task TriggersResumed(string triggerGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine("所有触发器已经被恢复");
            return Task.CompletedTask;
        }

        public Task JobAdded(IJobDetail jobDetail, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"添加了作业 {jobDetail.Key}");
            return Task.CompletedTask;
        }

        public Task JobDeleted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"删除了作业 {jobKey}");
            return Task.CompletedTask;
        }

        public Task JobPaused(JobKey jobKey, CancellationToken cancellationToken = default)
        {
           
            Console.WriteLine($"暂停了作业 {jobKey}");
            return Task.CompletedTask;
        }

        public Task JobInterrupted(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"中断了作业 {jobKey}");
            return Task.CompletedTask;
        }

        public Task JobsPaused(string jobGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"暂停了所有作业，原因：{jobGroup}");
            return Task.CompletedTask;

        }

        public Task JobResumed(JobKey jobKey, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"恢复了作业 {jobKey}");
            return Task.CompletedTask;
        }

        public Task JobsResumed(string jobGroup, CancellationToken cancellationToken = default)
        {
            Console.WriteLine($"恢复了所有作业，之前的暂停原因：{jobGroup}");
            return Task.CompletedTask;
        }

        public Task SchedulerError(string msg, SchedulerException cause, CancellationToken cancellationToken = default)
        {

            Console.WriteLine($"调度器错误：{msg}, 异常：{cause.Message}");
            return Task.CompletedTask;
        }

        public Task SchedulerInStandbyMode(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度器进入待机模式");
              return Task.CompletedTask;
        }

        public Task SchedulerStarted(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度器启动成功");
              return Task.CompletedTask;
        }

        public Task SchedulerStarting(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度器正在启动...");
              return Task.CompletedTask;
        }

        public Task SchedulerShutdown(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度器关闭成功");
              return Task.CompletedTask;
        }

        public Task SchedulerShuttingdown(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度器正在关闭...");
              return Task.CompletedTask;
        }

        public Task SchedulingDataCleared(CancellationToken cancellationToken = default)
        {
            Console.WriteLine("调度数据已清除");
              return Task.CompletedTask;
        }
    }
}
