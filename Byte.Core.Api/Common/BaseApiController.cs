using Byte.Core.Api.Attributes;
using Byte.Core.Tools.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Common
{  /// <summary>
   /// Mvc对外接口基控制器
   /// </summary>
    [CheckJWT]
    //[ApiLog]
    [ApiController]
    [CheckRole]
    public class BaseApiController : BaseController
    {
    }
}