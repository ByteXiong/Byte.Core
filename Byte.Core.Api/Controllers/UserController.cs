﻿
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Filters;
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
        public async Task<UserInfo> GetInfoAsync(long id) => await _logic.GetInfoAsync(id);

        /// <summary>
        /// 提交
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<long> Submit(UpdateUserParam param)
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
        public async Task<int> DeleteAsync(long[] ids) => await _logic.DeleteAsync(ids);


        /// <summary>
        /// 设置状态
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<int> SetStatusAsync(long id, bool status) => await _logic.SetStatusAsync(id, status);


        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        /// <exception cref="BusException"></exception>
        [HttpPut]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task SetPassword(SetPasswordParam param) => await _logic.SetPassword(param);
    }
}