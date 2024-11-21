using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Newtonsoft.Json;
using SqlSugar;
using System.ComponentModel.DataAnnotations.Schema;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 用户
    /// </summary>
    [SugarTable("Byte_User")]
    
    public class User : BaseEntity<int>
    {

        /// <summary>
        /// 昵称
        /// </summary>
        [Column("Name")]
        [SugarColumn(Length = 50, IsNullable = true)]
        public String NickName { get; set; }


        /// <summary>
        /// 头像
        /// </summary>
        [Column("Avatar")]
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Avatar { get; set; }



        /// <summary>
        /// 手机号
        /// </summary>
        [Column("Phone")]
        [SugarColumn(Length = 20, IsNullable = true)]
        public String Phone { get; set; }



        /// <summary>
        /// 邮箱
        /// </summary>
        [Column("Email")]
        [SugarColumn(Length = 30, IsNullable = true)]
        public String Email { get; set; }
        
        /// <summary>
        /// 密码
        /// </summary>
        [Column("Password")]
        [JsonIgnore]
        [SugarColumn(Length = 100, IsNullable = true)]
        public String Password { get; set; }




        /// <summary>
        /// 状态
        /// </summary>

        public bool Status { get; set; }


        /// <summary>
        /// 账号
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String UserName { get; set; }

        #region 导航

        /// <summary>
        ///用户部门角色关系
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        [Navigate(NavigateType.OneToMany, nameof(User_Dept_Role.UserId), nameof(Id))]
        public List<User_Dept_Role> User_Dept_Roles { get; set; }
        //[SugarColumn(IsIgnore = true)]
        //[Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.UserId), nameof(User_Dept_Role.DeptId))]
        //public List<Dept> Depts { get; set; }


        //[SugarColumn(IsIgnore = true)]
        //[Navigate(typeof(User_Dept_Role), nameof(User_Dept_Role.UserId), nameof(User_Dept_Role.RoleId))]
        //public List<Role> Roles { get; set; }

        #endregion
    }

    //[JsonIgnore]//隐藏
}
