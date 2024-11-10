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
    public class DataTableController(DataTableLogic logic) : BaseApiController
    {
        private readonly DataTableLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));

        #region 表头信息
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableHeaderDTO> GetTableHeaderAsync([FromQuery]TableHeaderParam param)=> await _logic.GetTableHeaderAsync(param);


        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<TableColumn> SetTableHeaderAsync(TableColumn param) =>await _logic.SetTableHeaderAsync(param);

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
        public async Task<List<TableColumn>> GetHeaderAsync([FromQuery]TableHeaderParam param) => await _logic.GetHeaderAsync(param);

        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<PagedResults<dynamic>> PageAsync([FromQuery] TableDataParam param)=> await _logic.PageAsync(param);

        #endregion



        ///// <summary>
        ///// 获取表字段
        ///// </summary>
        ///// <param name="tableName"></param>
        ///// <returns></returns>
        //public async Task<List<TableColumn>> GetTableColumnsAsync() => await _logic.GetTableColumnsAsync(); // await









        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<TableColumn> InfoAsync() => await _logic.InfoAsync(); // await

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()=> await _logic.UpdateAsync();
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync(int[] ids) => await _logic.DeleteAsync(ids);
        ///// <summary>
        /////  删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[ApiVersion("1.0", Deprecated = false)]
        //public async Task<int> DeleteAsync(int[] ids) => await _logic.DeleteAsync(ids);

        ///// <summary>
        ///// 设置状态
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="status"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[ApiVersion("1.0", Deprecated = false)]
        //public async Task<int> SetStatusAsync(int id, bool status) => await _logic.SetStatusAsync(id, status);






    }
}
