using Byte.Core.Common;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 部门
    /// </summary>
    [SugarTable("Dept")]
    public class Dept : BaseEntity<Guid>
    {

        

        /// <summary>
        /// 图标
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Image { get; set; }

        /// <summary>
        /// 单位名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string Name { get; set; }

        /// <summary>
        /// 简写名称
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public string EasyName { get; set; }
        /// <summary>
        /// 父级部门ID
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public Guid? ParentId { get; set; }
        /// <summary>
        /// 地址
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Address { get; set; }
        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

        /// <summary>
        ///默认联系电话
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string Phone { get; set; }
        /// <summary>
        ///  默认联系人
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = true)]
        public string Man { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
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

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.DeptId), nameof(User_Dept_Role.UserId))]
        public List<User> Users { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(Id), nameof(Role.DeptId))]
        public List<Role> Roles { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Dept> Children { get; set; }

        #endregion
    }

    //[JsonIgnore]//隐藏
}
