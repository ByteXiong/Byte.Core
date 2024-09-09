using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Byte.Core.SqlSugar;
using SqlSugar;
using Byte.Core.Tools;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("TableColumn")]
    public class TableColumn : BaseEntity<Guid>
    {

        public Guid TableModelId { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Type { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Label { get; set; }

        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Prop { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Int32? Sort { get; set; }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public TableAlignEnum Align { get; set; }
        
        /// <summary>
        /// 列是否固定在左侧或者右侧。 
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public TableFixedEnum Fixed { get; set; }
        /// <summary>
        /// 宽度
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public Int32? Width { get; set; }

        /// <summary>
        /// 是否可以表排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Boolean? Sortable { get; set; }


        /// <summary>
        /// 头部重写模版
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public String HeadTemplate { get; set; }

        /// <summary>
        /// 重写模版
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public String Template { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsShow { get; set; }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public int Condition { get; set; }
    }
}
