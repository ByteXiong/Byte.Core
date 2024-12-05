using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using Byte.Core.SqlSugar.IDbContext;
using Byte.Core.Tools;
using Mapster;
using Org.BouncyCastle.Asn1.Ocsp;
using Quartz;
using Quartz.Impl.Matchers;
using Quartz.Impl.Triggers;
using System.Reflection;
using static NPOI.HSSF.Util.HSSFColor;

namespace Byte.Core.Business.Quartz
{
    /// <summary>
    /// 系统作业信息表
    /// </summary>

    public class JobDetailLogic : BaseBusinessLogic<long, JobDetail, JobDetailRepository>
    {

        private readonly IScheduler _scheduler;
        private readonly JobTriggerRepository _jobTriggerRepository;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public JobDetailLogic(JobDetailRepository repository, JobTriggerRepository jobTriggerRepository, IScheduler scheduler) : base(repository)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _jobTriggerRepository = jobTriggerRepository;
        }



        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<JobDetailDTO>> GetPageAsync(JobDetailParam param)
        {

            var jobKeys = await _scheduler.GetJobKeys(GroupMatcher<JobKey>.AnyGroup());

            // Get the job detail for each job
            //foreach (JobKey jobKey in jobKeys)
            //{
            //    IJobDetail jobDetail = await _scheduler.GetJobDetail(new JobKey(jobKey.Name, jobKey.Group));
            //    Console.WriteLine($"Job Name: {jobKey.Name}, Group Name: {jobKey.Group}");
            //    var triggerKeys = await _scheduler.GetTriggersOfJob(jobKey);
            //    foreach (ITrigger trigger in triggerKeys)
            //    {
            //        Console.WriteLine($"  Trigger Name: {trigger.Key.Name}, Group Name: {trigger.JobKey.Name}");
            //        if (trigger is ICronTrigger cronTrigger)
            //        {
            //            string cronExpression = cronTrigger.CronExpressionString;
            //            Console.WriteLine($"    Cron Expression: {cronExpression}");
            //        }
            //    }
            //}


            var page = await Repository.GetIQueryable()
                .Includes(x => x.Triggers)
                .Select(
                 x => new JobDetailDTO { Triggers = x.Triggers }, true
                ).ToPagedResultsAsync(param);

            page.Data.ForEach(job =>
            {
                job.Triggers.ForEach(async trigger =>
                {
                    //var date = DateTime.UtcNow.ToTimeStamp();
                    //var timeStamp = trigger.StartTime ?? date;
                    var state = await _scheduler.GetTriggerState(new TriggerKey(trigger.Id.ToString()));
                    switch (state) { 
                    case TriggerState.Normal:
                        trigger.State = TriggerStateEnum.Running;
                        break;
                    case TriggerState.Paused://暂停
                        trigger.State = TriggerStateEnum.Paused;
                        break;
                    case TriggerState.Complete://完成
                        trigger.State = TriggerStateEnum.Complete;
                        break;
                    case TriggerState.Error://错误
                        trigger.State = TriggerStateEnum.ErrorToReady;
                        break;
                    case TriggerState.Blocked://阻塞
                        trigger.State = TriggerStateEnum.Blocked;
                        break;
                    case TriggerState.None:
                        trigger.State = TriggerStateEnum.NotStart;
                        break;
                    }

                }

                );
            }
            );


            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JobDetailInfo> GetInfoAsync(long id)
        {
            var entity = await Repository.GetIQueryable(x => x.Id == id).Select<JobDetailInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateJobDetailParam param)
        {
            JobDetail model = param.Adapt<JobDetail>();
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateJobDetailParam param)
        {
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstAsync();
            await UpdateAsync(entity);
            return param.Id;
        }
        /// <summary>
        /// 全局启动
        /// </summary>
        /// <returns></returns>
        public async Task SetAllStateAsync(JobActionEnum action)
        {

            switch (action)
            {
                case JobActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeAll();
                    break;
                case JobActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseAll();
                    break;
                case JobActionEnum.重启:
                    break;
                case JobActionEnum.执行:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  设置作业状态
        /// </summary>
        /// <returns></returns>
        public async Task SetJobStateAsync(long id, JobActionEnum action)
        {

            switch (action)
            {
                case JobActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeJob(new JobKey(id.ToString()));
                    break;
                case JobActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseJob(new JobKey(id.ToString()));
                    break;
                case JobActionEnum.重启:
                    break;
                case JobActionEnum.执行:
                    break;
                default:
                    break;
            }

        }

        /// <summary>
        ///  设置触发器状态
        /// </summary>
        /// <returns></returns>
        public async Task SetTriggerStateAsync(long id, JobActionEnum action)
        {

            switch (action)
            {
                case JobActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeTrigger (new TriggerKey(id.ToString()));
                    break;
                case JobActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseTrigger(new TriggerKey(id.ToString()));
                    break;
                case JobActionEnum.重启:
                    break;
                case JobActionEnum.执行:
                    break;
                case JobActionEnum.加入:
              var trigger = await  _jobTriggerRepository.GetIQueryable(x => x.Id == id).FirstAsync();
                     ITrigger jobTrigger = AddTrigger(trigger);
                    await  _scheduler.ScheduleJob(jobTrigger);
                   await _jobTriggerRepository.UpdateAsync(x => x.Id == id, x => new JobTrigger {Status=true});
                    break;
                case JobActionEnum.移除:
                    await _scheduler.UnscheduleJob(new TriggerKey(id.ToString()));
                    await _jobTriggerRepository.UpdateAsync(x => x.Id == id, x => new JobTrigger { Status = false });
                    break;
                default:


                    break;
            }

        }



        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public async Task ScheduleJobs()
        {
            //IScheduler _scheduler = ServiceLocator.Resolve<IScheduler>();
            //获取数据库的QZ配置
            var list = GetIQueryable().Includes(x => x.Triggers.Where(y => y.Status).ToList()).ToList();
            // Get the job detail for each job
            foreach (JobDetail detail in list)
            {
                IJobDetail jobDetail = AddJob(detail);
                await _scheduler.AddJob(jobDetail, true);

                List<ITrigger> triggers = new List<ITrigger>();
                foreach (JobTrigger trigger in detail.Triggers)
                {

                    ITrigger jobTrigger =AddTrigger(trigger);
                    triggers.Add(jobTrigger);
                }
                await _scheduler.ScheduleJob(jobDetail, triggers,true);
            }
        }


        public IJobDetail AddJob(JobDetail detail)
        {

            JobDataMap dataMap = new JobDataMap();
            var props = detail.Props?.ToObject<Dictionary<string, object>>();
            props?.ForEach(x => dataMap.Put(x.Key, x.Value));

            Assembly assIBll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/Byte.Core.Api.dll");
            //加载dll后,需要使用dll中某类.
            Type type = assIBll.GetType($"Byte.Core.Api.Quartz.{detail.AssemblyName}");//获取类名，必须 命名空间+类名 

            IJobDetail jobDetail = JobBuilder.Create(type)
                    .WithIdentity(detail.Id.ToString())
                     .SetJobData(dataMap)
                     .StoreDurably()
                    .Build();

            return jobDetail;
        }

        public ITrigger AddTrigger(JobTrigger trigger) {

            JobDataMap triggerMap = new JobDataMap();
            var triggerProps = trigger.Props?.ToObject<Dictionary<string, object>>();
            triggerProps?.ForEach(x => triggerMap.Put(x.Key, x.Value));

            //ITrigger jobTrigger = TriggerBuilder.Create()
            //.WithIdentity(trigger.Id.ToString())
            //.UsingJobData(triggerMap)//设置触发器的数据
            //.ForJob(trigger.DetailId.ToString())// 设置触发器的作业
            //.WithSimpleSchedule(x => x
            //  .WithRepeatCount(trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0)//最大触发次数（需要减1，因为第一次执行不计入重复次数）
            //  )
            // .WithCronSchedule(trigger.TriggerType)//设置触发器的类型 
            //                                       //设置开始时间
            ////.StartAt(DateTimeOffset.FromUnixTimeSeconds(trigger.StartTime ?? DateTime.UtcNow.ToTimeStamp()))
            //////设置结束时间
            ////.EndAt(DateTimeOffset.FromUnixTimeSeconds(trigger.EndTime ?? DateTime.UtcNow.AddYears(1).ToTimeStamp()))
            //.Build();
            int   simple= 0;
            var any = int.TryParse(trigger.TriggerType, out simple);
            if(any){
                ITrigger jobTrigger = new SimpleTriggerImpl(trigger.Id.ToString())
                {
                    JobDataMap = triggerMap,
                    JobName = trigger.DetailId.ToString(),
                    StartTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.StartTime ?? DateTime.UtcNow.ToTimeStamp()),
                    EndTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.EndTime ?? DateTime.UtcNow.AddYears(1).ToTimeStamp()),
                    RepeatCount = trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0,//指定触发器的重复次数
                    TimesTriggered = trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0,//指定触发器被触发的次数
                    RepeatInterval = TimeSpan.FromSeconds(simple)
                };
                return jobTrigger;
            }
            else { 
            ITrigger jobTrigger = new CronTriggerImpl(trigger.Id.ToString(), null, trigger.TriggerType)
            {
                JobDataMap=triggerMap,
                JobName = trigger.DetailId.ToString(),
                StartTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.StartTime ?? DateTime.UtcNow.ToTimeStamp()),
                EndTimeUtc = DateTimeOffset.FromUnixTimeSeconds(trigger.EndTime ?? DateTime.UtcNow.AddYears(1).ToTimeStamp()),
                MisfireInstruction = MisfireInstruction.CronTrigger.DoNothing,
            };

            return jobTrigger;
            }
        }

    }

}
