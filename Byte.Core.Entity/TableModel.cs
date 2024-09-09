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
    [SugarTable("TableModel")]
    public class TableModel : BaseEntity<Guid>
    {

        /// <summary>
        /// 标题
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Title { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public String Router { get; set; }

        /// 来源模型
        /// </summary>
        public String Table { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Comment { get; set; }
        /// <summary>

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(TableColumn.TableModelId),nameof(Id) )]
        public List<TableColumn> TableColumns { get; set; }

        #endregion
    }
}
