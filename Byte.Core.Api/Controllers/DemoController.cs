using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Models;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Microsoft.AspNetCore.Mvc;
using NPOI.SS.Formula.Functions;
using System.Linq.Expressions;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 代码生成器
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class DemoController (IUnitOfWork unitOfWork, RedisDemoLogic logic) : BaseApiController
    {
        readonly IUnitOfWork _unitOfWork = unitOfWork;
        readonly RedisDemoLogic _logic = logic;


        [HttpPost]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<ExcutedResult>  SetRoleDataAsync(int num =50000)
        {
            List<RedisDemo> list = new List<RedisDemo>();

            for (int i = 0; i < num; i++)
            {
                list.Add(new RedisDemo
                {
                    Id = Guid.NewGuid(),
                    Name = $"name{DateTime.Now.ToString("yyyyMMddHHmmssfff")}",
                    Sort = i,
                    Code = $"Code{i/3}"
                });
            }
            

           var date = DateTime.Now;
            int count = await _unitOfWork.GetDbClient().Fastest<RedisDemo>().BulkCopyAsync(list);
            //int count =  await _unitOfWork.GetDbClient().Insertable(list).PageSize(100).ExecuteCommandAsync();
            var tep = DateTime.Now - date;
            return ExcutedResult.SuccessResult(data: count, msg:$"插入成功{tep}秒");
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<ExcutedResult> GetPageAsync([FromQuery] RedisDemoParam param)
        {
            var date = DateTime.Now;
            var list = await _logic.GetPageAsync(param);
            var tep = DateTime.Now - date;
            return ExcutedResult.SuccessResult(data: list, msg: $"查询{tep}秒");


        }
       

    }
}