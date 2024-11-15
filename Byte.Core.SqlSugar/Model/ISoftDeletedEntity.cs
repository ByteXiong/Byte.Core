using SqlSugar;

namespace Byte.Core.SqlSugar.Model;

/// <summary>
/// 删除接口
/// </summary>
public interface ISoftDeletedEntity
{
    /// <summary>
    /// 是否删除
    /// </summary>

    bool IsDeleted { get; set; }
}
