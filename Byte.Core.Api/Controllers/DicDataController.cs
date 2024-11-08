using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{

    /// <summary>
    /// 字典
    /// </summary>
    /// <param name="logic"></param>
    [Route("api/[controller]/[action]")]
    public class DicDataController(DicDataLogic logic) : BaseApiController
    {
        private readonly DicDataLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));

        #region 查询
        /// <summary>
        /// 获取所有分组
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<string>> GetAllGroupByAsync()=> await _logic.GetAllGroupByAsync();
        /// <summary>
        /// 获取下拉
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<List<DicDataSelectDTO>> GetSelectAsync(string groupBy)=> await _logic.GetSelectAsync(groupBy);
        #endregion
    }
}
