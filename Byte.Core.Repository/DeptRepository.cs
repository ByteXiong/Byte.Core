﻿using Byte.Core.SqlSugar;
using Byte.Core.Entity;

namespace Byte.Core.Repository
{
    /// <summary>
    /// 部门
    /// </summary>
    public class DeptRepository : BaseRepository<long, Dept>
    {
        public DeptRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
        {
        }
    }
}