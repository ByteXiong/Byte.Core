using Newtonsoft.Json;
using SqlSugar;

namespace Byte.Core.SqlSugar
{
    /// <summary>
    /// 实体基类
    /// </summary>
    [SugarIndex("index_{table}_CreateBy", nameof(CreateBy), OrderByType.Asc)]
    public class BaseEntity<T> : RootKey<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 创建者名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string CreateBy { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [SugarColumn( IsNullable = true)]
        public long CreateTime { get; set; }
        /// <summary>
        /// 更新者名称
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public string UpdateBy { get; set; }

        /// <summary>
        /// 最后更新时间
        /// </summary>
        [SugarColumn(IsNullable = true)]
        public long? UpdateTime { get; set; }
    }
}
