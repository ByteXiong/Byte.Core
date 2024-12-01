using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Repository
{
 
    public class TableColumnRepository : BaseRepository<long, TableColumn>
    {
        public TableColumnRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
