using Byte.Core.Repository;
using Byte.Core.Repository;
using Byte.Core.Entity;
using Byte.Core.SqlSugar;
using Byte.Core.Models;
using Microsoft.AspNetCore.Mvc;
using Byte.Core.Common.Extensions;
using Mapster;
using System.Linq.Expressions;
using Byte.Core.Tools;
using Byte.Core.Common.Helpers;
using System.Reflection;

namespace Byte.Core.Business
{
    public class TableColumnLogic : BaseBusinessLogic<Guid, TableColumn, TableColumnRepository>
    {
        public TableColumnLogic( TableColumnRepository repository) : base(repository)
        {
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<TableColumnDTO>> GetPageAsync([FromQuery]  TableColumnParam param)
        {
            Expression<Func< TableColumn, bool>> where = x => true;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Label.Contains(param.KeyWord));
            }
            var page = await GetIQueryable(where).Select< TableColumnDTO>().ToPagedResultsAsync(param);

            return page;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task< TableColumnInfo> GetInfoAsync(Guid id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select< TableColumnInfo>().FirstAsync();
            return entity;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(UpdateTableColumnParam param)
        {
         
              

                 TableColumn model = param.Adapt<TableColumn>();

                await AddAsync(model);
            
                return model.Id;
      


        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateAsync(UpdateTableColumnParam param)
        {

        

                await UpdateAsync(x => x.Id == param.Id, x => new  TableColumn
                {
                   

                });

                return param.Id;
        }


  




    }
}
