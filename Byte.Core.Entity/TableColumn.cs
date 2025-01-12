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
    [SugarTable("byte_tablecolumn")]
    //[Tenant(AppConfig.TenantTable)]
    public class TableColumn : BaseEntity<long>
    {

        /// <summary>
        ///  视图Id
        /// </summary>
        public long ViewId { get; set; }

        /// <summary>
        /// 字段名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Title { get; set; }
        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn( Length = 50,IsNullable = true)]
        public string Key { get; set; }
        /// <summary>
        /// 搜索类型
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public ConditionalType? SearchType { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public ColumnTypeEnum? ColumnType { get; set; }


        /// <summary>
        /// 字段描述
        /// </summary>
        [SugarColumn(IsNullable = true , Length = 50)]
        public string ColumnTypeDetail { get; set; }


        /// <summary>
        /// 字段验证
        /// </summary>
        [SugarColumn(IsNullable = true, Length = 50)]
        public string ColumnTypeRules { get; set; }

        /// <summary>
        /// 是否自定义
        /// </summary>
        public bool IsCustom { get; set; }
        ///// <summary>
        ///// 这一列是否可以导出
        ///// </summary>
        //public bool AllowExport { get; set; }
        ///// <summary>
        ///// 表头列对齐方式，若不设置该项，则使用列内的文本排列
        ///// </summary>
        //public TableAlignEnum TitleAlign { get; set; }
        ///// <summary>
        ///// 该列单元格的 HTML 属性
        ///// </summary>
        //public string CellProps { get; set; }


        // public List<TableColumn> Children { get; set; }


        // public string ClassName { get; set; }

        // public string ColSpan { get; set; }

        // public string DefaultFilterOptionValue { get; set; }

        // public  string DefaultFilterOptionValues { get; set; }

        // public string defaultSortOrder { get; set; }

        // public string disabled { get; set; }
        // ublic string ellipsis { get; set; }
        // ublic string EllipsisComponent { get; set; }
        // ublic string expandable { get; set; }
        // ublic string filter { get; set; }
        // ublic string filterMode { get; set; }
        // ublic string disabled { get; set; }
        // ublic string disabled { get; set; }
        /// <summary>
        /// 字段类型
        /// </summary>
        //[SugarColumn(Length = 50, IsNullable = true)]
        //public string Type { get; set; }
        ///// <summary>
        ///// 名称
        ///// </summary>
        //[SugarColumn(Length = 50, IsNullable = true)]
        //public String Label { get; set; }

        ///// <summary>
        ///// 字段
        ///// </summary>
        //[SugarColumn(Length = 50, IsNullable = true)]
        //public String Prop { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Int32? Sort { get; set; }
        
        
        ///// <summary>
        ///// 列是否固定在左侧或者右侧。 
        ///// </summary>
        //[SugarColumn(IsNullable = true)]
        //public TableFixedEnum Fixed { get; set; }
        ///// <summary>
        ///// 宽度
        ///// </summary>
        //[SugarColumn( IsNullable = true)]
        //public Int32? Width { get; set; }

        ///// <summary>
        ///// 是否可以表排序
        ///// </summary>
        //[SugarColumn(IsNullable = true)]
        //public Boolean? Sortable { get; set; }


        ///// <summary>
        ///// 头部重写模版
        ///// </summary>
        //[SugarColumn(IsNullable = true)]
        //public String HeadTemplate { get; set; }

        ///// <summary>
        ///// 重写模版
        ///// </summary>
        //[SugarColumn(IsNullable = true)]
        //public String Template { get; set; }
        /// <summary>
        /// 是否隐藏
        /// </summary>
        public bool IsShow { get; set; }
        ///// <summary>
        ///// 搜索条件
        ///// </summary>
        //public int Condition { get; set; }
        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn( ColumnDataType = "text", IsNullable = true)]
        public string Props { get; set; }



        #region 导航

        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(ViewId), nameof(TableView.Id))]
        public TableView TableView { get; set; }

        #endregion
    }
}
