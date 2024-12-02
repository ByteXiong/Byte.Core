
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Api.Common.Quartz;
using Byte.Core.Business;
using Byte.Core.Common.Attributes;
using Byte.Core.Entity;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula;
using NPOI.SS.Formula.Functions;
using Quartz;
using Quartz.Impl.Matchers;
using static Azure.Core.HttpHeader;
using static Quartz.Logging.OperationName;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Route("api/[controller]/[action]")]
    [NoCheckJWT]
    public class QuartzController(IScheduler scheduler) : BaseApiController
    {
        private readonly IScheduler _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task AllAsync() {

             var jobKeys =await scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            // Get the job detail for each job
            foreach (JobKey jobKey in jobKeys)
            {
                IJobDetail jobDetail =await scheduler.GetJobDetail(new JobKey(jobKey.Name, jobKey.Group));
                Console.WriteLine($"Job Name: {jobKey.Name}, Group Name: {jobKey.Group}");
                var triggerKeys =await scheduler.GetTriggersOfJob(jobKey);
                foreach (ITrigger trigger in triggerKeys)
                {
                    Console.WriteLine($"  Trigger Name: {trigger.Key.Name}, Group Name: {trigger.JobKey.Name}");
                    if (trigger is ICronTrigger cronTrigger)
                    {
                        string cronExpression = cronTrigger.CronExpressionString;
                        Console.WriteLine($"    Cron Expression: {cronExpression}");
                    }
                }
            }
            
            //var triggerKeys = await scheduler.GetTriggerKeys(GroupMatcher<TriggerKey>.AnyGroup());

            //// Get the job detail for each job
            //foreach (TriggerKey triggerKey in triggerKeys)
            //{
            //    ITrigger jobDetail = await scheduler.GetTrigger(new TriggerKey(triggerKey.Name, triggerKey.Group));
            //    Console.WriteLine($"Job Name: {triggerKey.Name}, Group Name: {triggerKey.jo}");
            //}

            //var  jobGroupNames =await scheduler.GetJobGroupNames();
            //// Get all job names in each group
            //foreach (string groupName in jobGroupNames)
            //{
            //    var jobNames = await scheduler.job(groupName);

            //    // Get the job detail for each job
            //    foreach (string jobName in jobNames)
            //    {
            //        IJobDetail jobDetail = scheduler.GetJobDetail(jobName, groupName);
            //        Console.WriteLine($"Job Name: {jobName}, Group Name: {groupName}");
            //    }
            //}

            // Get all trigger group names
            //var  triggerGroupNames =await scheduler.GetTriggerGroupNames();

            ////Get all trigger names in each group
            //foreach (string triggerGroupName in triggerGroupNames)
            //{
            //    string[] triggerNames = scheduler.GetTriggerNames(triggerGroupName);

            //    // Get the trigger for each trigger
            //    foreach (string triggerName in triggerNames)
            //    {
            //        ITrigger trigger = scheduler.GetTrigger(triggerName, triggerGroupName);
            //        Console.WriteLine($"Trigger Name: {triggerName}, Group Name: {triggerGroupName}");
            //    }
            //}

        }
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public IActionResult ScheduleJob()
        {
            var job = JobBuilder.Create<DynamicJob>()
                .WithIdentity("testId")
                .WithDescription("测试")
                .Build();


            var trigger = TriggerBuilder.Create()
                .WithIdentity("testTrigger")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(123)
                    .RepeatForever())
                .Build();

            _scheduler.ScheduleJob(job, trigger);

            return Ok();
        }

        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public IActionResult DeleteJob()
        {
            _scheduler.DeleteJob(new JobKey("testId"));

            return Ok();
        }

        //[HttpPut]
        //public IActionResult UpdateJob()
        //{
        //    var jobDetail =await _scheduler.GetJobDetail(new JobKey("testId"));
        //    jobDetail.data.Put("myKey", request.NewValue);
        //    _scheduler.AddJob(jobDetail, true);

        //    return Ok();
        //}
    }
}