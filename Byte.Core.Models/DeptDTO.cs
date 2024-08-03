using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
namespace Byte.Core.Models
{

    /// <summary>
    ///  部门分页查询
    /// </summary>
    public class DeptParam : PageParam
    {
        // public string KeyWord { get; set; }	
    }
    /// <summary>
    ///  部门 修改
    /// </summary>
    public class UpdateDeptParam : AddDeptParam
    {


    }

    /// <summary>
    ///  部门 添加
    /// </summary>
    public class AddDeptParam : Dept
    {

    }
    /// <summary>
    /// 部门
    /// </summary>
    public class DeptTreeDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 账号
        /// </summary>
        [MaxLength(50, ErrorMessage = "超出最大长度")]
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出最大长度")]
        public String Icon { get; set; }


        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime? CreateTime { get; set; }





        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [MaxLength(200, ErrorMessage = "超出最大长度")]
        public String Remark { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        public Int32 Sort { get; set; }



        public List<DeptTreeDTO> Children { get; set; }
    }


    /// <summary>
    /// 部门 详情
    /// </summary>
    public class DeptInfo : Dept
    {


    }

    /// <summary>
    /// 菜单
    /// </summary>
    public class DeptSelectDTO
    {
        /// <summary>
        /// 主键Id!
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 菜单
        /// </summary>
        public String Name { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public String Icon { get; set; }
        public List<DeptSelectDTO>? Children { get; set; }
    }
}
