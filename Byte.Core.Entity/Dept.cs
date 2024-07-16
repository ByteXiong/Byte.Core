using Byte.Core.SqlSugar;
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
        /// 账号
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Icon { get; set; }

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
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Remark { get; set; }


        /// <summary>
        /// 排序
        /// </summary>

        public Int32 Sort { get; set; }

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.DeptId), nameof(User_Dept_Role.UserId))]
        public List<User> Users { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(Id),nameof(Role.DeptId))]
        public List<Role> Roles { get; set; }

        [SugarColumn(IsIgnore = true)]
        public List<Dept> Children { get; set; }

        #endregion
    }

    //[JsonIgnore]//隐藏
}
