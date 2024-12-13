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
using System;
using System.Reflection;
using static NPOI.HSSF.Util.HSSFColor;
using static Quartz.MisfireInstruction;

namespace Byte.Core.Business.Quartz
{
    /// <summary>
    /// 系统作业信息表
    /// </summary>

    public class JobDetailLogic : BaseBusinessLogic<long, JobDetail, JobDetailRepository>
    {

        private readonly IScheduler _scheduler;
        private readonly JobTriggerRepository _jobTriggerRepository;
        private readonly QuartzPersistence _qz;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public JobDetailLogic(JobDetailRepository repository, JobTriggerRepository jobTriggerRepository, IScheduler scheduler, QuartzPersistence quartzPersistence) : base(repository)
        {
            _scheduler = scheduler ?? throw new ArgumentNullException(nameof(scheduler));
            _jobTriggerRepository = jobTriggerRepository;
            _qz = quartzPersistence;
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
                .Select(
                 x => new JobDetailDTO {
                 AssemblyName= x.AssemblyName,
                 GroupName=  x.GroupName
                 }
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
        public async Task SetAllStateAsync(TriggerActionEnum action)
        {

            switch (action)
            {
                case TriggerActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeAll();
                    break;
                case TriggerActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseAll();
                    break;
                case TriggerActionEnum.重启:
                    break;
                case TriggerActionEnum.执行:
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        ///  设置作业状态
        /// </summary>
        /// <returns></returns>
        public async Task SetJobStateAsync(long id, TriggerActionEnum action)
        {
            switch (action)
            {
                case TriggerActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeJob(new JobKey(id.ToString()));
                    break;
                case TriggerActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseJob(new JobKey(id.ToString()));
                    break;
                case TriggerActionEnum.重启:
                    break;
                case TriggerActionEnum.执行:
                    break;
                default:
                    break;
            }
        }





     

    }

}
