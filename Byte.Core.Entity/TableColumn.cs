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

namespace Byte.Core.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("TableColumn")]
    public class TableColumn : BaseEntity<Guid>
    {

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
        /// 来源模型
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public String Table { get; set; }



        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Int32? Sort { get; set; }

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
        /// 路由
        /// </summary>
        [SugarColumn(Length = 200)]
        public String Router { get; set; }



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
        public bool IsHidden { get; set; }
        /// <summary>
        /// 搜索条件
        /// </summary>
        public int Condition { get; set; }
    }
}
