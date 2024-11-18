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
    [SugarTable("Byte_TableData")]
    public class TableView : BaseEntity<int>
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
