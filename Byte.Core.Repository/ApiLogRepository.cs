﻿using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 接口日志
    /// </summary>
    public class ApiLogRepository : BaseRepository<int, ApiLog>
    {
        public ApiLogRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}