using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Repository
{
    public class TableModelRepository : BaseRepository<Guid, TableModel>
    {
        public TableModelRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}
