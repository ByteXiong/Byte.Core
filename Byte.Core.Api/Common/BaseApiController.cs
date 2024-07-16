using Byte.Core.Common.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Common
{  /// <summary>
   /// Mvc对外接口基控制器
   /// </summary>
    [CheckJWT]
    //[ApiLog]
    [ApiController]
    public class BaseApiController : BaseController
    {
    }
}