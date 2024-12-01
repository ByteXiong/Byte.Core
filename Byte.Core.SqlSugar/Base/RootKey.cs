using SqlSugar;

namespace Byte.Core.SqlSugar
{
    /// <summary>
    /// 泛型主键
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class RootKey<T> where T : IEquatable<T>
    {
        /// <summary>
        /// 主键
        /// </summary>
        [SugarColumn(IsPrimaryKey = true)]
        public T Id { get; set; }
    }
}
