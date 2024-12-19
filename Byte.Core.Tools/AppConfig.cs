
namespace Byte.Core.Tools
{
    public static partial class AppConfig
    {
        #region 数据库表配置
        public const string Tenant = "Byte.Core";

        //public static IWebHostEnvironment WebHostEnvironment => InternalApp.WebHostEnvironment;
        #endregion
        public const string Root = "ROOT";

        /// <summary>
        /// 后台首页
        /// </summary>
        public const string PathHome = "/home";
        /// <summary>
        /// 用户缓存Key
        /// </summary>
        public const string RoleCaChe = "Role:";

        /// <summary>
        ///角色按钮缓存
        /// </summary>
        public static string RoleButtonCaChe => RoleCaChe + "Button:";
        /// <summary>
        /// 哈希主键
        /// </summary>
        public const string HRedisDemoKey = "HRedisDemo";


        public const string AopRedisKey = "AopRedisKey";

        public static int OK => 0;
        public static int Error => 500;
        public static int ErrorJWT => 401;
        public static int ErrorRole => 403;
    }
}