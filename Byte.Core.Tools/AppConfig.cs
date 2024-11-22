using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Tools
{
     
    public static class AppConfig
    {
        #region 数据库表配置
        public const string Tenant = "runtest";
        public const string Tenant1 = "DataConnection";
        public const string Tenant2 = "Byte.Core_DB";
        public const string TenantDts = "runtest2";
        
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
        public const string  HRedisDemoKey = "HRedisDemo";


        public const string AopRedisKey = "AopRedisKey";

        public static int OK => 0;
        public static int Error => 500;
        public static int ErrorJWT => 401;
        public static int ErrorRole => 403;
    }
}
