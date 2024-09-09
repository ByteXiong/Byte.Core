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
    public class TableColumnDTO
    {
       
        public Guid? Id { get; set; }
        /// <summary>
        /// 名称
        /// </summary>
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        public String Avatar { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        public String Password { get; set; }




        /// <summary>
        /// 状态
        /// </summary>

        public bool State { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        public String Account { get; set; }



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


}
