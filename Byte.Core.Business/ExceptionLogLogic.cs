using Byte.Core.Entity;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business
{

    public class ExceptionLogLogic : BaseBusinessLogic<long, ExceptionLog, ExceptionLogRepository>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public ExceptionLogLogic(ExceptionLogRepository repository) : base(repository)
        {
        }

    }
}
