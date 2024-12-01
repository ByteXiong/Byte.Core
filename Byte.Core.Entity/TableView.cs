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
using StackExchange.Redis;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 表格重写
    /// </summary>
    [SugarTable("Byte_TableView")]
    
    public class TableView : BaseEntity<long>
    {
        /// <summary>
        /// 表名
        /// </summary>
        public string Tableof { get; set; }
        /// <summary>
        /// 路由
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Router { get; set; }
        public ViewTypeEnum Type { get; set; }

        /// <summary>
        /// 默认排序字段
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public  string SortKey { get; set; }

        /// <summary>
        /// 排序排序方式
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public OrderTypeEnum SortOrder { get; set; }

        /// <summary>
        /// 多余参数 
        /// </summary>
        [SugarColumn(ColumnDataType = "text", IsNullable = true)]
        public string Props { get; set; }

        #region 导航

        /// <summary>
        /// 字段
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(TableColumn.ViewId), nameof(Id))]
        public List<TableColumn> TableColumns { get; set; }

        #endregion

    }
}
