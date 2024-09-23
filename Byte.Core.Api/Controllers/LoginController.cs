
using Asp.Versioning;
using Byte.Core.Api.Common;
using Byte.Core.Business;
using Byte.Core.Common.Attributes;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace Byte.Core.Api.Controllers
{
    /// <summary>
    /// 登录
    /// </summary>
    [Route("api/[controller]/[action]")]
    //public class LoginController(LoginLogic _logic, ICaptcha captcha) : BaseApiController
    public class LoginController(LoginLogic logic) : BaseApiController
    {
        //private readonly ICaptcha _captcha = captcha ?? throw new ArgumentNullException(nameof(ICaptcha));
        private readonly LoginLogic _logic = logic ?? throw new ArgumentNullException(nameof(logic));



        ///// <summary>
        ///// 获取验证码
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpGet]
        //[NoCheckJWT]
        //[ApiVersion("1.0", Deprecated = false)]
        //public CaptchaDTO Captcha()
        //{
        //    return new();
        //    //var captchaId = IdHelper.GetId();
        //    //var info = _captcha.Generate(captchaId);
        //    ////var stream = new MemoryStream(info.Bytes);
        //    //return new CaptchaDTO
        //    //{
        //    //    CaptchaId = captchaId,
        //    //    Img = "data:image/png;base64," + info.Bytes.ToBase64(),

        //    //};

        //}


        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<LoginToken> LoginAsync(LoginParam param)
        {
//#if !DEBUG
//            if (!_captcha.Validate(param.CaptchaId, param.CaptchaCode))
//            {
//                throw new BusException("无效验证码");
//            }
//#endif
            return await _logic.LoginAsync(param);
        }


        /// <summary>
        /// 微信登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]

        [ApiVersion("2.0", Deprecated = false), NoCheckJWT]
        public async Task<LoginToken> WeChatAsync() => await _logic.WeChatAsync();



        /// <summary>
        /// 获取登录信息
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task<LoginInfoDTO> InfoAsync() => await _logic.InfoAsync();

        /// <summary>
        /// 退出登录
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [NoCheckJWT]
        [ApiVersion("1.0", Deprecated = false)]
        [ApiVersion("2.0", Deprecated = false)]
        public async Task LoginOutAsync()
        {

        }




    }
}
