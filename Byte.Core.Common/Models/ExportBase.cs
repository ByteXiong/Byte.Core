using System.ComponentModel.DataAnnotations;

namespace Byte.Core.Common.Models
{
    public class ExportBase
    {


        /// <summary>
        /// ID
        /// </summary>
        [Display(Name = "ID")]
        public long Id { get; set; }
    }
}
