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
using SqlSugar;

namespace Byte.Core.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class TableColumnLogic : BaseBusinessLogic<int, TableColumn, TableColumnRepository>
    {

        readonly IUnitOfWork _unitOfWork;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public TableColumnLogic(TableColumnRepository repository, IUnitOfWork unitOfWork) : base(repository)
        {
            _unitOfWork = unitOfWork;
        }


        public  async Task<dynamic> GetFormAsync(int id, string tableof)
        {
           var columns = GetIQueryable(x=>x.TableView.Tableof== tableof && x.TableView.Type== ViewTypeEnum.编辑).ToList();
              
            var sql = $"select * from {tableof} ".ToSqlFilter();
            var info = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql)
                .Where("id = @id ", new
            {
                id = id
                }).FirstAsync();
            return info;
        }

        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(Dictionary<string, object> param, string tableof)
        {

            return _unitOfWork.GetDbClient().Insertable(param).AS(tableof).ExecuteCommand();
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(Dictionary<string, object> param,string tableof)
        {

            return _unitOfWork.GetDbClient().Updateable(param).AS(tableof).WhereColumns("id").ExecuteCommand();
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(int[] ids, string tableof)
        {

            return _unitOfWork.GetDbClient().Deleteable<object>().AS(tableof).Where("id in (@id) ", new { id = ids }).ExecuteCommand();//批量
        }



    }
}
