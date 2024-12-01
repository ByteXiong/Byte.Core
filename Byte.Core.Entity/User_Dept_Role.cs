using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 用户-部门-角色
    /// </summary>
    [SugarTable("Byte_User_Dept_Role")]
    
    public class User_Dept_Role
    {

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 部门Id
        /// </summary>
        public long DeptId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        public long RoleId { get; set; }
        #region 导航
        /// <summary>
        ///部门
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(DeptId))]
        public Dept Dept { get; set; }

        /// <summary>
        ///角色
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(RoleId))]
        public Role Role { get; set; }

        /// <summary>
        ///用户
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.ManyToOne, nameof(UserId))]
        public User User { get; set; }
        #endregion
    }

    //[JsonIgnore]//隐藏
}
