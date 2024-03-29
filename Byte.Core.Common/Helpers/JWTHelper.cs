using Byte.Core.Common.Extensions;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json.Linq;
using System.Text.RegularExpressions;

namespace Byte.Core.Common.Helpers
{
    public static class JWTHelper
    {
        private static readonly string _headerBase64Url = "{\"alg\":\"HS256\",\"typ\":\"JWT\"}".Base64UrlEncode();
        public static readonly string JWTSecret = ConfigHelper.GetValue("JWTSecret");

        /// <summary>
        /// 生成Token
        /// </summary>
        /// <param name="payloadJsonStr">数据JSON字符串</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static string SetToken(string payloadJsonStr, string secret)
        {
            string payloadBase64Url = payloadJsonStr.Base64UrlEncode();
            string sign = $"{_headerBase64Url}.{payloadBase64Url}".ToHMACSHA256String(secret);

            return $"{_headerBase64Url}.{payloadBase64Url}.{sign}";
        }


        /// <summary>
        /// 获取Token
        /// </summary>
        /// <param name="req">请求</param>
        /// <returns></returns>
        public static string GetToken(this HttpRequest req)
        {
            string tokenHeader = req.Headers["Authorization"].ToString();
            if (tokenHeader.IsNullOrEmpty())
                return null;

            string pattern = "^Bearer (.*?)$";
            if (!Regex.IsMatch(tokenHeader, pattern))
                throw new Exception("token格式不对!格式为:Bearer {token}");

            string token = Regex.Match(tokenHeader, pattern).Groups[1]?.ToString();
            if (token.IsNullOrEmpty())
                throw new Exception("token不能为空!");

            return token;
        }

        /// <summary>
        /// 获取Token中的数据
        /// </summary>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static JObject GetPayload(string token)
        {
            return token.Split('.')[1].Base64UrlDecode().ToJObject();
        }

        /// <summary>
        /// 获取Token中的数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="token">token</param>
        /// <returns></returns>
        public static T GetPayload<T>(string token)
        {
            if (token.IsNullOrEmpty())
                return default;

            return token.Split('.')[1].Base64UrlDecode().ToObject<T>();
        }

        /// <summary>
        /// 校验Token
        /// </summary>
        /// <param name="token">token</param>
        /// <param name="secret">密钥</param>
        /// <returns></returns>
        public static bool CheckToken(string token, string secret)
        {
            var items = token.Split('.');
            var oldSign = items[2];
            string newSign = $"{items[0]}.{items[1]}".ToHMACSHA256String(secret);

            return oldSign == newSign;
        }
    }
}
