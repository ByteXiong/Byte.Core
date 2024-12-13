using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Attributes;
using Byte.Core.Common.Extensions;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{    /// <summary>
     /// 代码生成器
     /// </summary>
    [Route("api/[controller]/[action]")]
    [NoCheckJWT]
    public class CodeController(IUnitOfWork  unitOfWork) : BaseApiController
    {
        readonly IUnitOfWork _unitOfWork = unitOfWork;

#if DEBUG
        [HttpGet]
        public void DbFirst()
        {

            var db = _unitOfWork.GetDbClient().GetConnection(AppConfig.TenantDts).DbFirst.IsCreateAttribute().StringNullable();
            db.FormatFileName(x => 
            { 
               var arr = x.Split("_");
                var str = "";
                for (int i = 0; i < arr.Length; i++)
                { 
                       if (i == 0)
                    {
                        str += arr[i].ToUpper();
                    }
                    else
                    {
                        str += "_"+arr[i].ToFirstUpperStr();
                    }
                }
            return str;
            } )
           .FormatClassName(x =>
           {
               var arr = x.Split("_");
               var str = "";
               for (int i = 0; i < arr.Length; i++)
               {
                   if (i == 0)
                   {
                       str += arr[i].ToUpper();
                   }
                   else
                   {
                       str += "_" + arr[i].ToFirstUpperStr();
                   }
               }
               return str;
           }
           )//格式化类名 （类名和表名不一样的情况）
        .FormatPropertyName(x => {
            var arr = x.Split("_");
            var str = "";
            for (int i = 0; i < arr.Length; i++)
            {
                if (i == 0)
                {
                    str += arr[i].ToFirstUpperStr();
                }
                else
                {
                    str += "_" + arr[i].ToFirstUpperStr();
                }
            }
            return str;
        })//格式化属性名 （属性名和字段名不一样情况）

           .CreateClassFile("c:\\Demo\\1", "Byte.Core.Models");
            //_unitOfWork.GetDbClient().GetConnection(AppConfig.TenantDts).DbFirst.Where("dts_sha_log").CreateClassFile("c:\\", "Models");

        }
#endif
    }
}
