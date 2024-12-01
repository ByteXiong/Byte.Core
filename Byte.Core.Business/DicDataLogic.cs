using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business
{
    /// <summary>
    /// 字典
    /// </summary>
    public  class DicDataLogic : BaseBusinessLogic<long, DicData, DicDataRepository>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public DicDataLogic(DicDataRepository repository) : base(repository)
        {
        }


        /// <summary>
        /// 获取所有分组
        /// </summary>
        /// <returns></returns>
        public async Task<List<string>> GetAllGroupByAsync() { 
                return await GetIQueryable().Select(x => x.GroupBy).Distinct().ToListAsync();
        
        }


        /// <summary>
        /// 获取下拉
        /// </summary>
        /// <returns></returns>
        public async Task<List<DicDataSelectDTO>> GetSelectAsync(string  groupBy)
        {
            if (string.IsNullOrEmpty(groupBy)) return null;
            return await GetIQueryable(x=>x.GroupBy==groupBy).OrderBy(x => x.Sort).Select<DicDataSelectDTO>().ToListAsync();
        }




    }
    }
