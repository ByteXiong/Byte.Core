using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 登录日志
    /// </summary>
    [SugarTable("Byte_LoginLog")]
    public class ApiLog : BaseEntity<int>
    {



        /// <summary>
        /// 请求路径
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public  string Path { get; set; }
       
        /// <summary>
        /// 请求方式
        /// </summary>

        public string Method { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public  string Body { get; set; }

        /// <summary>
        /// 返回结果
        /// </summary>
        public  string Result { get; set; }

        /// <summary>
        /// 登录Ip地址
        /// </summary>
        [SugarColumn(Length = 50,IsNullable = true)]
        public string Ip { get; set; }

        /// <summary>
        /// 登录版本
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public VersionEnum? Version { get; set; }
    }

    //[JsonIgnore]//隐藏
}
