using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    /// <remarks>
    ///  数据表列表
    /// </remarks>
    /// <param name="logic"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [Route("api/[controller]/[action]")]
    public class TableViewController(TableViewLogic logic) : BaseApiController
    {
        private readonly TableViewLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));

        #region 表头信息
    /// <summary>
    /// 
    /// </summary>
    /// <param name="param"></param>
    /// <param name="configId"></param>
    /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableView> GetTableHeaderAsync([FromQuery] TableViewParam param)=> await _logic.GetTableHeaderAsync(param);


        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableColumn> SetTableHeaderAsync(TableColumn param) =>await _logic.SetTableHeaderAsync(param);
        /// <summary>
        /// 排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetTableSortAsync(List<TableSortParam> param) => await _logic.SetTableSortAsync(param);


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> Submit(UpdateTableViewParam param)
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
        /// 删除表头
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task DeleteTableHeaderAsync(int[] ids) => await _logic.DeleteTableHeaderAsync(ids);
        #endregion

        #region 表头信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableView> GetViewAsync([FromQuery] TableViewParam param) => await _logic.GetViewAsync(param);


        #endregion





    }
}
