using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.EntityFramework.Options
{
    public class CustomerSettings
    {
        public IList<DbContextOption> DatabaseSettings { get; set; }
    }
}
