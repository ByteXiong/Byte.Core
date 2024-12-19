using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Tools;
using Quartz.Impl.Triggers;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Byte.Core.Repository;
using Byte.Core.Common.Helpers;

namespace Byte.Core.Business.Quartz
{
    public class QuartzPersistence 
    {
        private readonly JobDetailRepository _jobDetailRepository;
        private readonly JobTriggerRepository _jobTriggerRepository;
        private readonly IScheduler _scheduler;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public QuartzPersistence(JobDetailRepository jobDetailRepository,JobTriggerRepository  jobTriggerRepository, IScheduler scheduler)
        { 
            _jobDetailRepository = jobDetailRepository;
            _jobTriggerRepository = jobTriggerRepository;
            _scheduler = scheduler;
        }

        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public async Task ScheduleJobs()
        {
            //IScheduler _scheduler = ServiceLocator.Resolve<IScheduler>();
            //获取数据库的QZ配置
            var list = _jobDetailRepository.GetIQueryable().Includes(x => x.Triggers.Where(y => y.Status).ToList()).ToList();
            // Get the job detail for each job
            foreach (JobDetail detail in list)
            {
                IJobDetail jobDetail = AddJob(detail);
                await _scheduler.AddJob(jobDetail, true);

                List<ITrigger> triggers = new List<ITrigger>();
                foreach (JobTrigger trigger in detail.Triggers)
                {

                    ITrigger jobTrigger = AddTrigger(trigger);
                    triggers.Add(jobTrigger);
                }
                await _scheduler.ScheduleJob(jobDetail, triggers, true);
            }
        }

        public IJobDetail AddJob(JobDetail detail)
        {

            JobDataMap dataMap = new JobDataMap();
            var props = detail.Props?.ToObject<Dictionary<string, object>>();
            props?.ForEach(x => dataMap.Put(x.Key, x.Value));

            Type type =null;
            foreach (var assembly in RuntimeHelper.GetAllAssemblies()) {

                type = assembly.GetTypes().Where(x => x.Name == detail.AssemblyName).FirstOrDefault();
                if (type != null)
                {
                    break;
                }

            }

            //var assembly = RuntimeHelper.GetAssembly(detail.AssemblyName);
         ;
            //加载dll后,需要使用dll中某类.
            //Type type = RuntimeHelper.GetAllAssemblies().Where(assembly =>).FirstOrDefault();//获取类名，必须 命名空间+类名

            IJobDetail jobDetail = JobBuilder.Create(type)
                    .WithIdentity(detail.Id.ToString())
                     .SetJobData(dataMap)
                     .StoreDurably()
                    .Build();

            return jobDetail;
        }

        public ITrigger AddTrigger(JobTrigger trigger)
        {

            JobDataMap triggerMap = new JobDataMap();
            var triggerProps = trigger.Props?.ToObject<Dictionary<string, object>>();
            triggerProps?.ForEach(x => triggerMap.Put(x.Key, x.Value));
            ITrigger jobTrigger = null;
            int simple = 0;
            switch (trigger.TriggerType)
            {
                case TriggerTypeEnum.简单触发器:


                    jobTrigger = new SimpleTriggerImpl(trigger.Id.ToString())
                    {
                        JobDataMap = triggerMap,
                        JobName = trigger.JobId.ToString(),
                        StartTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.StartTime ?? DateTime.Now.ToTimeStamp()),
                        EndTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.EndTime ?? DateTime.Now.AddYears(1).ToTimeStamp()),
                        //RepeatCount = trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0,//指定触发器的重复次数
                        TimesTriggered = trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0,//指定触发器被触发的次数
                        RepeatInterval = TimeSpan.FromSeconds(simple)
                    };
                    break;
                case TriggerTypeEnum.Cron触发器:

                    jobTrigger = new CronTriggerImpl(trigger.Id.ToString(), null, trigger.Description)
                    {
                        JobDataMap = triggerMap,
                        JobName = trigger.JobId.ToString(),
                        StartTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.StartTime ?? DateTime.Now.ToTimeStamp()),
                        EndTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.EndTime ?? DateTime.Now.AddYears(1).ToTimeStamp()),
                        MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing,
                    };

                
                    break;
                case TriggerTypeEnum.日历间隔触发器:
                    jobTrigger = new CalendarIntervalTriggerImpl(trigger.Id.ToString(), IntervalUnit.Month, 5)
                    {
                        JobDataMap = triggerMap


                    };


                    break;

                case TriggerTypeEnum.每日时间间隔触发器:

                    jobTrigger = new DailyTimeIntervalTriggerImpl(trigger.Id.ToString(), null, TimeOfDay.HourAndMinuteOfDay(9, 0), TimeOfDay.HourAndMinuteOfDay(17, 0), IntervalUnit.Hour, 1)
                    {

                    };
                    break;






                default:
                    break;

            }
            return jobTrigger;
        }
    }
}
