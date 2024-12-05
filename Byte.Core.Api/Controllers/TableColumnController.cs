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

    /// <summary>
    /// 模型
    /// </summary>
    /// <param name="logic"></param>
    [Route("api/[controller]/[action]/{configId}/{tableof}")]
    public class TableColumnController(TableColumnLogic logic) : BaseApiController
    {


        private readonly TableColumnLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));



        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<dynamic>> PageAsync([FromQuery] TableDataPageParam param, string configId, string tableof) => await _logic.PageAsync(param, configId, tableof);



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<dynamic> GetFormAsync( int id, string configId, string tableof) => await _logic.GetFormAsync(id, configId, tableof);

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task<int> Submit(Dictionary<string, object> param, string configId, string tableof)
    {
            object id = "";
            param.TryGetValue("id", out id);
        if ( string .IsNullOrEmpty(id?.ToString()))
        {
            return await _logic.AddAsync(param,configId, tableof);
        }
        else
        {
            return await _logic.UpdateAsync(param, configId, tableof);
        }
    }
    /// <summary>
    /// 删除表头
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    [HttpDelete]
    [ApiVersion("1.0", Deprecated = false)]
    public async Task DeleteAsync(int[] ids, string configId, string tableof) => await _logic.DeleteAsync(ids,  configId, tableof);
    }
}
