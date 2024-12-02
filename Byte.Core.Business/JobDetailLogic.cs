using Byte.Core.Business.Quartz;
using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.SqlSugar.IDbContext;
using Byte.Core.Tools;
using Mapster;
using Org.BouncyCastle.Asn1.Ocsp;
using Quartz;
using Quartz.Impl.Matchers;
using System.Reflection;
using static NPOI.HSSF.Util.HSSFColor;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 系统作业信息表
    /// </summary>

    public class JobDetailLogic : BaseBusinessLogic<long, JobDetail, JobDetailRepository>
    {

        private readonly IScheduler _scheduler;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public JobDetailLogic(JobDetailRepository repository, IScheduler scheduler) : base(repository)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
        }



        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<JobDetailDTO>> GetPageAsync(JobDetailParam param)
        {
            var page = await Repository.GetIQueryable()
                .Includes(x => x.Triggers)
                .Select(
                 x => new JobDetailDTO { Triggers = x.Triggers }, true
                ).ToPagedResultsAsync(param);

            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JobDetailInfo> GetInfoAsync(long id)
        {
            var entity = await Repository.GetIQueryable(x => x.Id == id).Select<JobDetailInfo>().FirstOrDefaultAsync();
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
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstOrDefaultAsync();
            await UpdateAsync(entity);
            return param.Id;
        }



        /// <summary>
        ///  设置状态
        /// </summary>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool Status)
        {
            var type = 1;
            var entiy = await GetIQueryable(x => x.Id == id).Includes(x => x.Triggers).FirstOrDefaultAsync();
            if (type == 1)
            {

                await _scheduler.PauseJob(new JobKey(entiy.Id.ToString()));
            }
            else
            {
                await _scheduler.PauseTrigger(new TriggerKey("myTrigger"));
            }


            //return await UpdateAsync(x => id == x.Id, x => new JobDetail { Status = Status });
            return 1;
        }
         
        /// <summary>
        ///  
        /// </summary>
        /// <returns></returns>
        public async Task StratAsync()
        {
            //IScheduler _scheduler = ServiceLocator.Resolve<IScheduler>();
            //获取数据库的QZ配置
            var list = GetIQueryable().Includes(x => x.Triggers.Where(y => y.Status == TriggerStatus.Ready).ToList()).ToList();
            Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>> triggersAndJobs = new Dictionary<IJobDetail, IReadOnlyCollection<ITrigger>>();
            // Get the job detail for each job
            foreach (JobDetail detail in list)
            {
                //await _scheduler.DeleteJob(jobDetail.Key);
                JobDataMap dataMap = new JobDataMap();
                var props = detail.Props?.ToObject<Dictionary<string, object>>();
                props?.ForEach(x => dataMap.Put(x.Key, x.Value));

                Assembly assIBll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/Byte.Core.Api.dll");
                //加载dll后,需要使用dll中某类.
                Type type = assIBll.GetType($"Byte.Core.Api.Common.Quartz.{detail.AssemblyName}");//获取类名，必须 命名空间+类名 

                IJobDetail jobDetail = JobBuilder.Create(type)
                        .WithIdentity(detail.Id.ToString(), detail.GroupName ?? $"Job-{detail.Id.ToString()}")
                         .SetJobData(dataMap)
                         .StoreDurably()
                        .Build();
                //Console.WriteLine($"Job Name: {jobKey.Name}, Group Name: {jobKey.Group}");
                await _scheduler.AddJob(jobDetail, true);

                List<ITrigger> triggers = new List<ITrigger>();
                foreach (JobTrigger trigger in detail.Triggers)
                {

                    JobDataMap triggerMap = new JobDataMap();
                    var triggerProps = trigger.Props?.ToObject<Dictionary<string, object>>();
                    triggerProps?.ForEach(x => triggerMap.Put(x.Key, x.Value));

                    ITrigger jobTrigger = TriggerBuilder.Create()
                    .WithIdentity(trigger.Id.ToString(), trigger.GroupName ?? $"Trigger-{trigger.Id.ToString()}")
                    .UsingJobData(triggerMap)//设置触发器的数据
                    .ForJob(jobDetail.Key)// 设置触发器的作业
                    .WithSimpleSchedule(x => x
                      .WithRepeatCount(trigger.MaxNumberOfRuns > 0 ? trigger.MaxNumberOfRuns - 1 : 0)//最大触发次数（需要减1，因为第一次执行不计入重复次数）
                      )
                     .WithCronSchedule(trigger.TriggerType)//设置触发器的类型 
                    //设置开始时间
                    .StartAt(new DateTimeOffset(trigger.StartTime ?? DateTime.Now, TimeSpan.Zero))
                    //设置结束时间
                    .EndAt(new DateTimeOffset(trigger.EndTime ?? DateTime.Now.AddYears(9), TimeSpan.Zero))
                    .Build();
                    triggers.Add(jobTrigger);
                }
                triggersAndJobs.Add(jobDetail, triggers);

            }
            if (triggersAndJobs.Count > 0)
            {
               
                Console.WriteLine("调度任务启动");
                await _scheduler.ScheduleJobs(triggersAndJobs, true);
            }

        }
    }

}
