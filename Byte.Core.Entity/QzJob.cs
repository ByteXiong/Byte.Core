using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Entity
{

    [SugarTable("Byte_QzJob")]
    
    public  class QzJob  : BaseEntity<int>
    {
      
        /// <summary>
        /// 组名称
        /// </summary>
        [SugarColumn(ColumnDescription = "组名称", Length = 128)]
        [MaxLength(128)]
        public string GroupName { get; set; } = "default";

        /// <summary>
        /// 作业类型FullName
        /// </summary>
        [SugarColumn(ColumnDescription = "作业类型", Length = 128)]
        [MaxLength(128)]
        public string JobType { get; set; }

        /// <summary>
        /// 程序集Name
        /// </summary>
        [SugarColumn(ColumnDescription = "程序集", Length = 128)]
        [MaxLength(128)]
        public string AssemblyName { get; set; }

        /// <summary>
        /// 描述信息
        /// </summary>
        [SugarColumn(ColumnDescription = "描述信息", Length = 128)]
        [MaxLength(128)]
        public string Description { get; set; }

        /// <summary>
        /// 是否并行执行
        /// </summary>
        [SugarColumn(ColumnDescription = "是否并行执行")]
        public bool Concurrent { get; set; } = true;

        /// <summary>
        /// 是否扫描特性触发器
        /// </summary>
        [SugarColumn(ColumnDescription = "是否扫描特性触发器")]
        public bool IncludeAnnotation { get; set; } = false;

        /// <summary>
        /// 额外数据
        /// </summary>
        [SugarColumn(ColumnDescription = "额外数据", ColumnDataType = StaticConfig.CodeFirst_BigString)]
        public string Properties { get; set; } 
        /// <summary>
        /// 脚本代码
        /// </summary>
        [SugarColumn(ColumnDescription = "脚本代码", ColumnDataType = StaticConfig.CodeFirst_BigString)]
        public string ScriptCode { get; set; }

    }
}
