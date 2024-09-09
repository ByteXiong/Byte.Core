using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Extensions;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    
    /// <summary>
    /// 表
    /// </summary>
    /// <param name="logic"></param>
    [Route("api/[controller]/[action]")]
    public class TableModelController(TableModelLogic logic) : BaseApiController
    {
        private readonly TableModelLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<PagedResults<TableModelDTO>> GetPageAsync([FromQuery] TableModelParam param) => await _logic.GetPageAsync(param);
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableModelInfo> GetInfoAsync([FromQuery] TableModelInfoParam param) => await _logic.GetInfoAsync(param);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> Submit(UpdateTableModelParam param)
        {
            int i = 0;
            param.TableColumns?.ForEach(x => x.Sort = i++);
            if (param.Id == Guid.Empty)
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
        public async Task<int> DeleteAsync(Guid[] ids) => await _logic.DeleteAsync(ids);



    }
}
