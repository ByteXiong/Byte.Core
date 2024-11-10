
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Models;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;


namespace Byte.Core.Api.Controllers
{

    /// <remarks>
    /// 部门
    /// </remarks>
    /// <param name="logic"></param>
    /// <exception cref="ArgumentNullException"></exception>
    [Route("api/[controller]/[action]")]
    public class DeptController(DeptLogic logic) : BaseApiController
    {
        private readonly DeptLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));


        /// <summary>
        /// 树图
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<List<DeptTreeDTO>> GetTreeAsync() => await _logic.GetTreeAsync();
        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="types"></param>
        /// <param name="parentId"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<DeptSelectDTO>> GetTreeSelectAsync([ FromQuery]DeptTypeEnum[] types=null, [FromQuery]int? parentId = null) => await _logic.GetTreeSelectAsync(types, parentId);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<DeptInfo> GetInfoAsync(int id) => await _logic.GetInfoAsync(id);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> Submit(UpdateDeptParam param)
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
        public async Task<int> DeleteAsync(int[] ids) => await _logic.DeleteAsync(ids);

        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> SetStatusAsync(int id, bool status) => await _logic.SetStatusAsync(id, status);


    }
}