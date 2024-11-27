using Byte.Core.SqlSugar;
using Byte.Core.SqlSugar.Model;
using SqlSugar;

namespace Byte.Core.Entity
{
    /// <summary>
    /// 字典
    /// </summary>
    [SugarTable("Byte_DicData")]
    
    public class DicData : BaseEntity<int> , ISoftDeletedEntity
    {


        /// <summary>
        /// 分组
        /// </summary>
        [SugarColumn(Length = 50)]
        public string GroupBy { get; set; }
   
        /// <summary>
        /// 值
        /// </summary>
        [SugarColumn(Length = 50)]
        public string Label { get; set; }
        /// <summary>
        /// 键
        /// </summary>
        [SugarColumn(Length =200)]
        public string  Value { get; set; }


        /// <summary>
        /// 备注
        /// </summary>
        [SugarColumn(Length = 500, IsNullable = true)]
        public string Remark { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public bool Status { get; set; }
        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }

    }
    //[JsonIgnore]//隐藏
}
