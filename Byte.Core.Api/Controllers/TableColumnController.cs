using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.UserModel;
using Org.BouncyCastle.Crypto;

namespace Byte.Core.Api.Controllers
{

    [Route("api/[controller]/[action]/{tableof}")]
    public class TableColumnController(TableColumnLogic logic) : BaseApiController
    {


        private readonly TableColumnLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));



        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<dynamic>> PageAsync([FromQuery] TableDataPageParam param, string tableof) => await _logic.PageAsync(param, tableof);



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<dynamic> GetFormAsync( int id, string tableof) => await _logic.GetFormAsync(id, tableof);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<int> Submit(Dictionary<string, object> param, string tableof)
    {
            object id = "";
            param.TryGetValue("id", out id);
        if ( string .IsNullOrEmpty(id?.ToString()))
        {
            return await _logic.AddAsync(param, tableof);
        }
        else
        {
            return await _logic.UpdateAsync(param, tableof);
        }
    }
    /// <summary>
    /// 删除表头
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task DeleteAsync(int[] ids, string tableof) => await _logic.DeleteAsync(ids, tableof);
    }
}
