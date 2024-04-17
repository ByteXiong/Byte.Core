using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
