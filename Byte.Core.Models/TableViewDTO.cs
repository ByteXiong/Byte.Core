using Byte.Core.SqlSugar;
using Byte.Core.Entity;
using System.ComponentModel.DataAnnotations;
using SqlSugar;
using Byte.Core.Tools;
namespace Byte.Core.Models
{

    //public class TableViewParam : PageParam
    //{
    //    public string? KeyWord { get; set; }

    //}

    public class TableViewPageParam:PageParam
    {

        public string Tableof { get; set; }
        public string Router { get; set; }
        public ViewTypeEnum Type { get; set; }
    }
    public class TableViewParam
    {

        public string Tableof { get; set; }
        public string Router { get; set; }
        public ViewTypeEnum Type { get; set; }
    }



    public class UpdateTableViewParam : TableView { 
    
    
    }




}