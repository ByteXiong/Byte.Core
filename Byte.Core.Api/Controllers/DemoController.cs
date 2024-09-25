using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Attributes;
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
        public async Task<ExcutedResult<int>>  SetRoleDataAsync(int num =50000)
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
            return ExcutedResult<int>.SuccessResult(count, msg:$"插入成功{tep}秒");
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        public async Task<ExcutedResult<PagedResults<RedisDemoDTO>>> GetPageAsync([FromQuery] RedisDemoParam param)
        {
            var date = DateTime.Now;
            var list = await _logic.GetPageAsync(param);
            var tep = DateTime.Now - date;
            return ExcutedResult<PagedResults<RedisDemoDTO>>.SuccessResult(data: list, msg: $"查询成功:{tep}");

        }


        /// <summary>
        /// 取消请求测试
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [NoCheckJWT]
        [ApiVersion("0.0", Deprecated = false)]
        public async Task<List<User>> CancelRequestAsync(CancellationToken cancellationToken)
        {
            try
            {
                await Task.Delay(5000, cancellationToken); // 10 seconds delay
                var list = await _unitOfWork.GetDbClient().Queryable<User>().ToListAsync(cancellationToken);

                // Simulate a long-running operation
                //await Task.Delay(10000, cancellationToken); // 10 seconds delay
                Console.WriteLine($"{HttpContext.Request.Host}-请求成功");
                return list;
            }
            catch (Exception)
            {
                Console.WriteLine($"{HttpContext.Request.Host}-已经取消请求");
                //请求取消抛异常
                throw;
            }
           
          
        }


        /// <summary>
        /// 同步请求
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpGet]
        [NoCheckJWT]
        [ApiVersion("1.0", Deprecated = false)]
        public string GetSync()
        {
             var str = DateTime.Now.ToString();
            var i = 0;
            while (i <=10)
            {
                Thread.Sleep(1000);
                i++;
            }
            ////线程暂停
            //// 挂起当前线程1秒钟
          
            var a = $"{str}-{DateTime.Now.ToString()}";
            Console.WriteLine(a);
            return a;
        }

    }
}