using Byte.Core.SqlSugar;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    [SugarTable("User")]
    public class User : BaseEntity<Guid>
    {

        /// <summary>
        /// 名称
        /// </summary>
        [Column("Name")]
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Name { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [Column("Avatar")]
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Avatar { get; set; }


        /// <summary>
        /// 密码
        /// </summary>
        [Column("Password")]
        [SugarColumn(Length = 100, IsNullable = true)]
        public String Password { get; set; }




        /// <summary>
        /// 状态
        /// </summary>

        public bool State { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String Account { get; set; }

        #region 导航
        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.UserId), nameof(User_Dept_Role.DeptId))]
        public List<Dept> Depts { get; set; }


        [SugarColumn(IsIgnore = true)]
        [Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.UserId), nameof(User_Dept_Role.RoleId))]
        public List<Role> Roles { get; set; }

        #endregion
    }

    //[JsonIgnore]//隐藏
}
