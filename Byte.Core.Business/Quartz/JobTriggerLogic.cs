using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Mapster;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business.Quartz
{

    /// <summary>
    /// 系统作业信息表
    /// </summary>
    public class JobTriggerLogic : BaseBusinessLogic<long, JobTrigger, JobTriggerRepository>
    {
        private readonly IScheduler _scheduler;
        private readonly QuartzPersistence _qz;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public JobTriggerLogic(JobTriggerRepository repository, IScheduler scheduler, QuartzPersistence qz) : base(repository)
        {
            _scheduler = scheduler;
            _qz = qz;
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<JobTriggerDTO>> GetPageAsync(JobTriggerParam param)
        {
            Expression<Func<JobTrigger, bool>> where = x =>true;

            if(param.JobId != null) {
                where = where.And(x =>x.JobId==param.JobId);
            }

            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.GroupName.Contains(param.KeyWord)|| x.AssemblyName.Contains(param.KeyWord));
            }
           
            var page = await Repository.GetIQueryable(where).Select(x => new JobTriggerDTO
            {
            }, true)
                .ToPagedResultsAsync(param);
            page.Data.ForEach(async trigger =>
            {
                    //var date = DateTime.UtcNow.ToTimeStamp();
                    //var timeStamp = trigger.StartTime ?? date;
                    var state = await _scheduler.GetTriggerState(new TriggerKey(trigger.Id.ToString()));
                    switch (state)
                    {
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
            });
            return page;
        }

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<JobTriggerInfo> GetInfoAsync(long id)
        {
            var entity = await Repository.GetIQueryable(x => x.Id == id).Select<JobTriggerInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> AddAsync(UpdateJobTriggerParam param)
        {
            JobTrigger model = param.Adapt<JobTrigger>();
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<long> UpdateAsync(UpdateJobTriggerParam param)
        {
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstAsync();
            await UpdateAsync(entity);
            return param.Id;
        }


        /// <summary>
        ///  设置触发器状态
        /// </summary>
        /// <returns></returns>
        public async Task SetStateAsync(long id, TriggerActionEnum action)
        {

            switch (action)
            {
                case TriggerActionEnum.启动:
                    //恢复
                    await _scheduler.ResumeTrigger(new TriggerKey(id.ToString()));
                    break;
                case TriggerActionEnum.暂停:
                    //暂停所有
                    await _scheduler.PauseTrigger(new TriggerKey(id.ToString()));
                    break;
                case TriggerActionEnum.重启:
                    break;
                case TriggerActionEnum.执行:
                    break;
                case TriggerActionEnum.加入:
                    var trigger = await GetIQueryable(x => x.Id == id).FirstAsync();
                    ITrigger jobTrigger = _qz.AddTrigger(trigger);
                    await _scheduler.ScheduleJob(jobTrigger);
                    await UpdateAsync(x => x.Id == id, x => new JobTrigger { Status = true });
                    break;
                case TriggerActionEnum.移除:
                    await _scheduler.UnscheduleJob(new TriggerKey(id.ToString()));
                    await UpdateAsync(x => x.Id == id, x => new JobTrigger { Status = false });
                    break;
                default:
                    break;
            }

        }
    }
}
