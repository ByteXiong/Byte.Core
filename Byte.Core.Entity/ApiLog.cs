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
    
    [SugarTable("byte_apilog")]
    public class ApiLog : BaseEntity<long>
    {


        /// <summary>
        /// 请求路径
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public  string Url { get; set; }

        /// <summary>
        /// 请求方式
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Method { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public  string Query { get; set; }
        /// <summary>
        /// 请求参数
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Body { get; set; }
        /// <summary>
        /// 返回结果
        /// </summary>
        [SugarColumn(IsNullable = true)]
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

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        
    }

    //[JsonIgnore]//隐藏
}
