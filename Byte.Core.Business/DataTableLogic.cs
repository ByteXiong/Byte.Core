using Byte.Core.Common.Extensions;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Jil;
using Mapster;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Byte.Core.Business
{
    /// <summary>
    ///  数据表
    /// </summary>
    public class DataTableLogic
    {
        readonly IUnitOfWork _unitOfWork;
        public DataTableLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// 获取表信息
        /// </summary>
        /// <returns></returns>
        public async Task<List<DataTableDTO>> GetDataAsync()
        {
            var sql = "";
            //var sql =
            //        "SELECT TABLE_NAME as TableName," +
            //        " Table_Comment as TableComment" +
            //        " FROM INFORMATION_SCHEMA.TABLES" +
            //        $" where TABLE_SCHEMA = 'Byte.Core_DB2'";
            if (_unitOfWork.GetDbClient().CurrentConnectionConfig.DbType == DbType.MySql)
            {
                sql = @"select * from (SELECT (case when a.colorder=1 then d.name else '' end) as TableName,
                      (case when a.colorder=1 then isnull(f.value,'') else '' end) as TableComment
                       FROM syscolumns a
                       inner join sysobjects d on a.id=d.id  and d.xtype='U' and  d.name<>'dtproperties'
                       left join sys.extended_properties f on d.id=f.major_id and f.minor_id=0) t
                       where t.TableName!=''";
            }
            var list = await _unitOfWork.GetDbClient().SqlQueryable<DataTableDTO>(sql).ToListAsync();
            return list;
        }


        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public async Task<List<DataTableColumnDTO>> GetTableColumnsAsync(string tableName)
        {

            var sql = "";
            if (_unitOfWork.GetDbClient().CurrentConnectionConfig.DbType == DbType.MySql)
            {
                sql =
                 "select table_name as TableName,column_name as ColName, " +
                 " column_default as DefaultValue," +
                 " IF(extra = 'auto_increment','TRUE','FALSE') as IsIdentity," +
                 " IF(is_nullable = 'YES','TRUE','FALSE') as IsNullable," +
                 " DATA_TYPE as ColumnType," +
                 " CHARACTER_MAXIMUM_LENGTH as ColumnLength," +
                 " IF(COLUMN_KEY = 'PRI','TRUE','FALSE') as IsPrimaryKey," +
                 " COLUMN_COMMENT as Comments " +
                 $" from information_schema.columns where table_schema = '{_unitOfWork.GetDbClient().CurrentConnectionConfig.ConfigId}' and table_name = '{tableName}'";
            }

            var list = await _unitOfWork.GetDbClient().SqlQueryable<DataTableColumnDTO>(sql).ToListAsync();
            return list;
        }



        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableHeaderDTO> GetTableHeaderAsync(TableHeaderParam param)
        {
            #region  获取数据库字段
            var sql = "";
            if (_unitOfWork.GetDbClient().CurrentConnectionConfig.DbType == DbType.MySql)
            {
                sql =
                 @"SELECT
                    table_name AS   TableName,
	                column_name AS  ColumnKey,
	                column_default AS DefaultValue,
	            COLUMN_COMMENT AS Common,
		        table_schema
                FROM
                information_schema.COLUMNS";
            }
            var dataTable = await _unitOfWork.GetDbClient().SqlQueryable<DataTableColumnDTO>(sql).
                Where("table_schema = @schema And TableName =@tableName ", new {
                    schema=_unitOfWork.GetDbClient().CurrentConnectionConfig.ConfigId,
                    tableName = param.Table
                })
                .ToListAsync();

            var  sysList = dataTable.Select(x => new TableColumn
            {  
              
              Table = x.TableName,
              Key=x.ColumnKey,
              Title=x.Common,
            }).ToList();
            #endregion


            //获取自定义字段
            var entity = await _unitOfWork.GetDbClient().Queryable<TableColumn>().Where(x => x.Table == param.Table).ToListAsync();
            var keys1 = entity.Select(x => x.Key).ToList();
            var keys2 = sysList.Select(x => x.Key).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableColumn>();
            foreach (var item in keys)
            {
                var model = entity.FirstOrDefault(x => x.Key == item);
                model ??= sysList.FirstOrDefault(x => x.Key == item);
                list.Add(model);
            }

            var header = new TableHeaderDTO();
            header.Table = param.Table;
            header.Columns= list;
            return header;
            // var aa = await _unitOfWork.GetDbClient().SqlQueryable<DataTableColumnDTO>(sql).ToListAsync();
            //return aa;
        }

        /// <summary>
        /// 设置表头
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableColumn> SetTableHeaderAsync(TableColumn param) {
            await _unitOfWork.GetDbClient().Storageable(param).ExecuteReturnEntityAsync();

            return param;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public async Task DeleteTableHeaderAsync(int[] ids)
        {
            await _unitOfWork.GetDbClient().Deleteable<TableColumn>().In(ids).ExecuteCommandAsync();

        }




        #endregion


        #region 信息获取
        /// <summary>
        /// 表头信息获取
        /// </summary>
        /// <returns></returns>
        public async Task<List<TableColumn>> GetHeaderAsync(TableHeaderParam param)
        {
         var entity = await _unitOfWork.GetDbClient().Queryable<TableColumn>().Where(x => x.Table == param.Table&&x.IsShow).ToListAsync();
            entity.ForEach(x => x.Key= x.Key.ToFirstLowerStr());
         return entity;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResults<dynamic>> PageAsync(TableDataParam param)
        {

            var conModels = new List<IConditionalModel>();

            param.Search?.ForEach(x =>
            {
                var model= new ConditionalModel();
                x.Value.ForEach(y =>
                {
                    switch (y.Key)
                    {
                        case "key":
                            model.FieldName = y.Value;
                            
                            break;
                        case "searchType":
                            switch ((SearchTypeEnum)y.Value.ToInt()) {
                                case SearchTypeEnum.模糊:
                                    model.ConditionalType = ConditionalType.Like;
                                    break;
                                case SearchTypeEnum.大于:
                                    model.ConditionalType = ConditionalType.GreaterThan;
                                    break;
                                case SearchTypeEnum.大于或等于:
                                    model.ConditionalType = ConditionalType.GreaterThanOrEqual;
                                    break;
                                case SearchTypeEnum.小于:
                                    model.ConditionalType = ConditionalType.LessThan;
                                    break;
                                case SearchTypeEnum.小于或等于:
                                    model.ConditionalType = ConditionalType.GreaterThanOrEqual;
                                    break;
                                default:
                                    model.ConditionalType = ConditionalType.Equal;
                                    break;
                            }

                            break;
                        case "value":
                            model.FieldValue = y.Value;
                            break;
                        default:
                            break;
                    }

                });
                conModels.Add(model);
            });
            
            var sql = $"select * from {param.Table}".ToSqlFilter();

            var list = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).Where(conModels).ToPagedResultsAsync(param);
            return list;
        }



        #endregion



        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task<TableColumn> InfoAsync()
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).FirstAsync();
            return aa.FirstOrDefault();
        }

        /// <summary>
        /// 更新
        /// </summary>
        /// <returns></returns>
        public async Task UpdateAsync()
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).ToListAsync();
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <returns></returns>
        public async Task DeleteAsync(int[] ids)
        {
            var sql = "select * from user";
            var aa = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).ToListAsync();
        }

    }
}
