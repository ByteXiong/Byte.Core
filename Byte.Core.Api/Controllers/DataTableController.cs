using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Entity;
using Byte.Core.Models;
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

        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<TableColumn>> GetTableColumnsAsync() =>await _logic.GetTableColumnsAsync(); // await


        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<TableColumn>> GetTableColumnsAsync() => await _logic.GetTableColumnsAsync(); // await



        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        public async Task PageAsync()=> await _logic.QueryAsync();
        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()=> await _logic.UpdateAsync();
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync() => await _logic.DeleteAsync();
        ///// <summary>
        /////  删除
        ///// </summary>
        ///// <param name="ids"></param>
        ///// <returns></returns>
        //[HttpDelete]
        //[ApiVersion("1.0", Deprecated = false)]
        //public async Task<int> DeleteAsync(Guid[] ids) => await _logic.DeleteAsync(ids);

        ///// <summary>
        ///// 设置状态
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="state"></param>
        ///// <returns></returns>
        //[HttpPut]
        //[ApiVersion("1.0", Deprecated = false)]
        //public async Task<int> SetStateAsync(Guid id, bool state) => await _logic.SetStateAsync(id, state);






    }
}
