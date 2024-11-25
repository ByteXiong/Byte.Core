using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 组织-数据库
    /// </summary>
    [SugarTable("Byte_Dept_DbConfig")]
    
    public class Dept_Tenant
    {
        /// <summary>
        /// 菜单Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int DeptId { get; set; }


        /// <summary>
        /// 数据库Id
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public int TenantId { get; set; }
    }

    //[JsonIgnore]//隐藏
}
