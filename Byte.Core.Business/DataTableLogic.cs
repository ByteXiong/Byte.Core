using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.SqlSugar;
using Byte.Core.Tools;
using Jil;
using Mapster;
using Newtonsoft.Json;
using NPOI.HSSF.Record;
using NPOI.SS.Formula.Functions;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using DbType = SqlSugar.DbType;

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

        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableHeaderDTO> GetTableHeaderAsync(TableHeaderParam param)
        {
            var columns = new List<DataTableColumnDTO>();
            if (param.Table.IsNullOrEmpty()) throw new BusException("表名不能为空");
            else if (param.Table.Substring(param.Table.Length - 3) == "DTO")
            {
               columns = GetXml(param.Table);
            }
            else {

                columns = await GetDataTableAsync(param.Table);
                }

            #region  获取字段
            var sysList = columns.Select(x => new TableColumn
            {
                Table = x.TableName,
                Key = x.ColumnKey,
                Title = x.Common,
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
            header.Columns = list;
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
            param.Key??=Guid.NewGuid().ToString();  
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
        /// <returns></returns>5
        public async Task<List<TableColumn>> GetHeaderAsync(TableHeaderParam param)
        {
            var entity = await _unitOfWork.GetDbClient().Queryable<TableColumn>().Where(x => x.Table == param.Table && x.IsShow).OrderBy(x => x.Sort).ToListAsync();
            entity.ForEach(x => x.Key.ToFirstLowerStr());
            return entity;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResults<dynamic>> PageAsync(TableDataParam param)
        {
            var sql = $"select * from {param.Table}".ToSqlFilter();
            var list = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).SearchWhere(param).ToPagedResultsAsync(param);
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

        #region 私有方法

        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<DataTableColumnDTO> GetXml(string tableName)
        {

            var typeName = "Byte.Core.Models";
            var xmlCommentHelper = new XmlCommentHelper();
            //var xmlFile = AppDomain.CurrentDomain.BaseDirectory + typeName + ".xml";
            //"E:\\MyCode\\LY_WMSCloud\\LY_WMSCloud.Business\\bin\\Debug\\net6.0\\LY_WMSCloud.Models.xml"
            //xmlCommentHelper.Load(new string[] { xmlFile });
            xmlCommentHelper.LoadAll();
            //type
            //var path = $"LY_WMSCloud.Models.{model}";
            //Type type= Type.GetType(path);

            Assembly assIBll = Assembly.LoadFrom(AppDomain.CurrentDomain.BaseDirectory + "/" + typeName + ".dll");
            //加载dll后,需要使用dll中某类.
            Type type = assIBll.GetType($"{typeName}.{tableName}");//获取类名，必须 命名空间+类名 


            var props = type.GetProperties().Where(p => p .GetCustomAttribute<JsonIgnoreAttribute>()  == null).ToArray();

            
            //entity.Comment = xmlCommentHelper.GetComment($"T:{type.FullName}", "summary");
            var list = new List<DataTableColumnDTO>();
            for (int i = 0; i < props.Length; i++)
            {
                MemberInfo prop = props[i];
                var common = xmlCommentHelper.GetFieldOrPropertyComment(prop);
                var model = new DataTableColumnDTO()
                {
                     TableName = tableName.Trim(),
                     Common = common.Trim(),
                     ColumnKey = prop.Name.ToFirstLowerStr(),//转小写,
                    //Sortable = sortable,
                };
                list.Add(model);
            }

            return list;
        }

        /// <summary>
        ///  获取表结构
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        private async Task<List<DataTableColumnDTO>>  GetDataTableAsync(string tableName){

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
        Where("table_schema = @schema And TableName =@tableName ", new
        {
            schema = _unitOfWork.GetDbClient().CurrentConnectionConfig.ConfigId,
            tableName = tableName
        })
    .ToListAsync();
            return dataTable;
    #endregion
}

    }
}
