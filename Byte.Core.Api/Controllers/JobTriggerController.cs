using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Business.Quartz;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{

    [Route("api/[controller]/[action]")]
    public class JobTriggerController(JobTriggerLogic logic) : BaseApiController
    {

        private readonly JobTriggerLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));



        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<JobTriggerDTO>> GetPageAsync([FromQuery] JobTriggerParam param) => await _logic.GetPageAsync(param);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<JobTriggerInfo> GetInfoAsync(long id) => await _logic.GetInfoAsync(id);
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> Submit(UpdateJobTriggerParam param)
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
        ///  设置触发器状态
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetStateAsync(long id, [FromBody] TriggerActionEnum action) => await _logic.SetStateAsync(id, action);

    }
}
