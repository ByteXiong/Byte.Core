
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Entity;
using Byte.Core.Models;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using SqlSugar;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 菜单
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class MenuController(MenuLogic logic) : BaseApiController
    {
        private readonly MenuLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));


        /// <summary>
        /// 列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<Menu>> GetTreeAsync() => await _logic.GetTreeAsync();


        /// <summary>
        /// 下拉框
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<MenuSelectDTO>> GetTreeSelectAsync(Guid? parentId = null) => await _logic.GetTreeSelectAsync(parentId);

        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<MenuInfo> GetInfoAsync(Guid id) => await _logic.GetInfoAsync(id);
        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> Submit(UpdateMenuParam param)
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
        ///  设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> SetStateAsync(Guid id, bool state) => await _logic.UpdateAsync(x => id == x.Id, x => new Menu { State = state });

        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> DeleteAsync(Guid[] ids) => await _logic.DeleteAsync(x => ids.Contains(x.Id));



        /// <summary>
        ///  获取我的路由
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<RouteDTO>> GetRoutesAsync() => await _logic.GetRoutesAsync();

    }
}