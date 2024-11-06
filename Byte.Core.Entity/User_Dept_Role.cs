using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 用户-部门-角色
    /// </summary>
    [SugarTable("User_Dept_Role")]
    public class User_Dept_Role
    {

        /// <summary>
        /// 用户Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int UserId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int DeptId { get; set; }

        /// <summary>
        /// 角色Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int RoleId { get; set; }
    }

    //[JsonIgnore]//隐藏
}
