using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{

    public class DataTableParam
    {
      public string TableName { get; set; }

    }
    public class DataTableDTO
    {
    
    /// <summary>
    ///  表名
    /// </summary>
    public string TableName { get; set; }
    /// <summary>
    ///  描述
    /// </summary>
    public string Description { get; set; }
    }
    public class DataTableCommonDTO
    {

        /// <summary>
        ///  表名
        /// </summary>
        public string TableName { get; set; }
        /// <summary>
        ///  描述
        /// </summary>
        public string Description { get; set; }
    }

    public class DataTableColumnDTO
    {
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
        public string  ColumnKey { get; set; }
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

    #region 获取头部

    public class TableHeaderParam
    {

        public string Table { get; set; }
        public string Router { get; set; }
    }

    public class TableHeaderDTO
    {
        /// <summary>
        /// 
        /// </summary>
        public String Table { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string Router { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public List<TableColumn> Columns { get; set; }
    }




    #endregion

    public class TableDataParam : PageParam
    {

        public string Table { get; set; }
        public string Router { get; set; }

        public Dictionary<string, Dictionary< string,string>> Search { get; set; }
}
}
