using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    
    [Route("api/[controller]/[action]")]
    public class TableColumnController(TableColumnLogic logic) : BaseApiController
    {
        private readonly TableColumnLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<PagedResults<TableColumnDTO>> GetPageAsync([FromQuery] TableColumnParam param) => await _logic.GetPageAsync(param);
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableColumnInfo> GetInfoAsync(Guid id) => await _logic.GetInfoAsync(id);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> Submit(UpdateTableColumnParam param)
        {
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




        /// <summary>
        /// 获取头获取列表
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>

        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableModel> GetColumnsAsync([FromQuery] TableGetColumnParam param) => await _logic.GetColumnsAsync(param);

        /// <summary>
        /// 设置列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetColumnsAsync(TableModel param) => await _logic.SetColumnsAsync(param);
    }
}
