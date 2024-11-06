using Byte.Core.Common.Attributes.RedisAttribute;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 测试表
    /// </summary>
    [SugarTable("RedisDemo")]
    public class RedisDemo : BaseEntity<int>
    {
        /// <summary>
        /// 账号
        /// </summary>
        [FindKey]
        public String Name { get; set; }
        /// <summary>
        /// 状态
        /// </summary>
        public bool State { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 200, IsNullable = true)]
        public String Remark { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [SugarColumn(Length = 20, IsNullable = false)]
        public string Code { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public int Sort { get; set; }

    }

    //[JsonIgnore]//隐藏
}
