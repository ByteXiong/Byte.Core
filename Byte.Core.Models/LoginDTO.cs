using Byte.Core.Tools;

namespace Byte.Core.Models
{

    public class LoginParam
    {

        /// <summary>
        /// 账号
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        public string CaptchaId { get; set; }
        public string CaptchaCode { get; set; }
    }

    public class WeLoginParam
    {
        public string Code { get; set; }
    }

    public class CaptchaDTO
    {

        /// <summary>
        /// 类型
        /// </summary>
        public string CaptchaId { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string Img { get; set; }

    }

    public class LoginToken
    {


        /// <summary>
        /// 授权token
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int Expires { get; set; }

        /// <summary>
        /// 类型
        /// </summary>
        public string TokenType { get; set; }

        /// <summary>
        /// 刷新token
        /// </summary>
        public string RefreshToken { get; set; }

        /// <summary>
        /// 允许token时间内
        /// </summary>
        public int RefreshTokenExpires { get; set; }

    }


    public class LoginInfoDTO
    {

        /// <summary>
        /// 主键Id!
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public String Avatar { get; set; }




        /// <summary>
        /// 账号
        /// </summary>
        public String UserName { get; set; }

        public List<string> Roles => CurrentUser.RoleCodes;
        public List<string> Buttons { get; set; }

    }
}
