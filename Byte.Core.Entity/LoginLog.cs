using Byte.Core.SqlSugar;
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
    public class LoginLog : BaseEntity<int>
    {


        /// <summary>
        /// 登录Ip地址
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string Ip { get; set; }

        /// <summary>
        /// 登录版本
        /// </summary>
        public decimal version { get; set; }
    }

    //[JsonIgnore]//隐藏
}
