using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
namespace Byte.Core.Models
{

    public class TableModelParam : PageParam
    {
        public string? KeyWord { get; set; }

    }
    /// <summary>
    /// 分页
    /// </summary>
    public class TableModelDTO : TableModel
    {


    }

    public class TableModelInfoParam
    {
         public Guid? Id { get; set; }

        /// <summary>
        /// 路由
        /// </summary>
        public String Router { get; set; }

        /// 来源模型
        /// </summary>
        public String Table { get; set; }

    }

    public class TableModelInfo : TableModel
    {
    }

    /// <summary>
    /// 修改
    /// </summary>
    public class UpdateTableModelParam : AddTableModelParam
    {


    }

    /// <summary>
    ///  添加
    /// </summary>
    public class AddTableModelParam : TableModel
    {
    }

 




}
