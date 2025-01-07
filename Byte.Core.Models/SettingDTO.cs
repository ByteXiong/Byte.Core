using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{
    public class UpdateSettingDTO : AddSettingDTO
    {

    }
    public class AddSettingDTO
    {
        /// <summary>
        /// 键
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// 值
        /// </summary>
        [Required]
        public string Value { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

    }
}
