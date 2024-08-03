
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 用户
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UserController(UserLogic logic) : BaseApiController
    {
        private readonly UserLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));
        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<PagedResults<UserDTO>> GetPageAsync([FromQuery] UserParam param) => await _logic.GetPageAsync(param);
        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<UserInfo> GetInfoAsync(Guid id) => await _logic.GetInfoAsync(id);

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> AddAsync(UpdateUserParam param) => await _logic.AddAsync(param);

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<Guid> UpdateAsync(UpdateUserParam param) => await _logic.UpdateAsync(param);
        /// <summary>
        ///  删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpDelete]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> DeleteAsync(Guid[] ids) => await _logic.DeleteAsync(x => ids.Contains(x.Id));


        /// <summary>
        ///  设置状态
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> SetStateAsync(Guid id, bool state) => await SetStateAsync(id, state);
    }
}