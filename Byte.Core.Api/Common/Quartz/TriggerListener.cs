using Quartz;

namespace Byte.Core.Api.Common.Quartz
{
    public class TriggerListener : ITriggerListener
    {
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
            Console.WriteLine("任务执行前事件通知，任务名称：{0}", context.JobDetail.Key.Name);
            await Task.Delay(0, cancellationToken);
            return false;
            //return context.();
        }
    }
}
