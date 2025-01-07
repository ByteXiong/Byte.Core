using Byte.Core.SqlSugar.Model;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.SqlSugar.Base
{
    /// <summary>
    /// 实体基类 无数据权限检查
    /// </summary>

    [SugarIndex("index_{table}_IsDeleted", nameof(IsDeleted), OrderByType.Asc)]
    public class BaseEntityNoDataScope<T> : BaseEntity<T> where T : IEquatable<T>
    {

        /// <summary>
        /// 是否已删除
        /// </summary>
        [JsonIgnore]
        public bool IsDeleted { get; set; }
    }

}
