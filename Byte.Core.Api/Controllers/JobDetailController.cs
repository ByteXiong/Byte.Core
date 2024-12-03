using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Business.Quartz;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;
using Quartz;
using System;

namespace Byte.Core.Api.Controllers
{

    [Route("api/[controller]/[action]")]
    public class JobDetailController(JobDetailLogic logic) : BaseApiController
    {
        private readonly JobDetailLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<JobDetailDTO>> GetPageAsync([FromQuery] JobDetailParam param) => await _logic.GetPageAsync(param);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<JobDetailInfo> GetInfoAsync(long id) => await _logic.GetInfoAsync(id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> Submit(UpdateJobDetailParam param)
        {
            if (param.Id == default)
            {

                return await _logic.AddAsync(param);
            }
            else
            {
                return await _logic.UpdateAsync(param);
            }
        }
        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> DeleteAsync(long[] ids) => await _logic.DeleteAsync(ids);


        /// <summary>
        /// 全局启动
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetAllStateAsync([FromBody] JobActionEnum action) => await _logic.SetAllStateAsync(action);


        /// <summary>
        ///  设置作业状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetJobStateAsync(long id, [FromBody] JobActionEnum action)=>await _logic.SetJobStateAsync( id,action);


        /// <summary>
        ///  设置触发器状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetTriggerStateAsync(long id, [FromBody] JobActionEnum action) => await _logic.SetTriggerStateAsync( id, action);
}
}
