using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 数据库 
    /// </summary>
    [SugarTable("Byte_Tenant")]
    public class Tenant : BaseEntity<int>
    {
        /// <summary>
        /// 配置Id
        /// </summary>
        [SugarColumn(Length = 50, IsNullable = true)]
        public String ConnId { get; set; }
        /// <summary>
        /// 角色类型
        /// </summary>
        public DbType DBType { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 命中率 
        /// </summary>
        public int HitRate { get; set; }
        /// <summary>
        /// 数据库链接
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String ConnectionString { get; set; }


        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true  )]
        public int OrderNo { get; set; } = 100;

        #region 导航

    [SugarColumn(IsIgnore = true)]
    [Navigate(typeof(Dept_Tenant), nameof(Dept_Tenant.TenantId), nameof(Dept_Tenant.DeptId))]
    public List<Dept> Depts { get; set; }
     #endregion
    }
    //[JsonIgnore]//隐藏
}
