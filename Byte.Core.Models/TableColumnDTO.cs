using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
namespace Byte.Core.Models
{

    public class TableColumnParam : PageParam
    {
        public string? KeyWord { get; set; }

    }
    /// <summary>
    /// 排序
    /// </summary>
    public class TableSortParam
    {
        public long Id { get; set; }
        public int Sort { get; set; }
    }

    public class DataColumn{

        /// <summary>
        ///  表名
        /// </summary>
        public string TableName { get; set; }


        /// <summary>
        ///  注释
        /// </summary>
        public string Common { get; set; }
        /// <summary>
        ///  字段
        /// </summary>
        public string ColumnKey { get; set; }
        /// <summary>
        ///  描述
        /// </summary>
        public string Type { get; set; }


        /// <summary>
        /// 是否主键
        /// </summary>
        public bool IsPrimaryKey { get; set; }

        /// <summary>
        /// 是否自增
        /// </summary>
        public bool IsIdentity { get; set; }

        /// <summary>
        /// 是否可空
        /// </summary>
        public bool IsNull { get; set; }

        /// <summary>
        /// 长度
        /// </summary>
        public int Length { get; set; }
        /// <summary>
        /// 精度
        /// </summary>
        public int Accuracy { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }

    public class TableDataPageParam : PageParam
    {
    }
    public class TableDataFormParam 
    {
        
        public string Id { get; set; }
    }

    public class UpdateTableDataParam: TableDataFormParam
    {

        public Dictionary<string,string> Data { get; set; }


    }


}