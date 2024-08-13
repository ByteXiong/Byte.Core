using Byte.Core.Common.Attributes.RedisAttribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Models
{
    public class RedisDemoDTO
    {
        [FindKey]
        public Guid Id { get; set; }
        [FindKey]
        public string Code { get; set; }
        public string Srot { get; set; }
    }
}
