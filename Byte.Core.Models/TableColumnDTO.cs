using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
namespace Byte.Core.Models
{

    public class TableColumnParam : PageParam
    {
        public string? KeyWord { get; set; }

    }
    /// <summary>
    /// 修改
    /// </summary>
    public class UpdateTableColumnParam : AddTableColumnParam
    {


    }

    /// <summary>
    ///  添加
    /// </summary>
    public class AddTableColumnParam : TableColumn
    {
        public Guid[]? RoleIds { get; set; }
    }

    /// <summary>
    /// 分页
    /// </summary>
    public class TableColumnDTO : TableColumn
    {



    }


    /// <summary>
    /// 用户 详情
    /// </summary>
    public class TableColumnInfo : TableColumn
    {

    }


    public class TableGetColumnParam
    {
        public string Table { get; set; }
        public string Router { get; set; }
    }

    /// <summary>
    /// 头部醒醒
    /// </summary>
    public class TableModel
    {
        /// <summary>
        /// 
        /// </summary>
        public String Table { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TableColumn> Columns { get; set; }
    }
}