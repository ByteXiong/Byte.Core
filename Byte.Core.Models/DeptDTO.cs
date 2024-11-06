using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using Byte.Core.Tools;
using SqlSugar;
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

        /// <summary>
        /// 验证码
        /// </summary>
        public int MsgCode { get; set; }
    }
    /// <summary>
    /// 部门
    /// </summary>
    public class DeptTreeDTO
    {
        public int Id { get; set; }
        /// <summary>
        /// 图标
        /// </summary>
        public string Image { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 简写名称
        /// </summary>
        public string EasyName { get; set; }
        /// <summary>
        /// 父级部门ID
        /// </summary>
        public int? ParentId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        public string Address { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///默认联系电话
        /// </summary>
        public string Phone { get; set; }
        /// <summary>
        ///  默认联系人
        /// </summary>
        public string Man { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }


        /// <summary>
        /// 类型 公司.部门
        /// </summary>
        public DeptTypeEnum Type { get; set; }


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
        public int Id { get; set; }

        /// <summary>
        /// 父级
        /// </summary>
        public int? ParentId { get; set; }
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
