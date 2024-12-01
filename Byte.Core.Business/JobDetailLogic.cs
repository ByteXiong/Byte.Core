using Byte.Core.Entity;
using Byte.Core.Models;
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
                 x=> new JobDetailDTO { Triggers = x.Triggers },true
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
            entity.Name = param.Name;
            await UpdateAsync(entity);
            return param.Id;
        }




        /// <summary>
        ///  设置状态
        /// </summary>
        /// <returns></returns>
        public async Task<int> SetStatusAsync(int id, bool Status) {
            var type = 1;
            var entiy = await GetIQueryable(x => x.Id == id).Includes(x => x.Triggers).FirstOrDefaultAsync();
            if (type==1)
            {
           

            
             await _scheduler.PauseJob(new JobKey(entiy.Name) );
            }
            else
            {
             await   _scheduler.PauseTrigger(new TriggerKey("myTrigger"));
            }


            //return await UpdateAsync(x => id == x.Id, x => new JobDetail { Status = Status });
            return 1;



        }
    }

}
