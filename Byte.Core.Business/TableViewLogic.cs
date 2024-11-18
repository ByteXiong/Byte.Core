using Byte.Core.Common.Extensions;
using Byte.Core.Common.Filters;
using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
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
using DataColumn = Byte.Core.Models.DataColumn;
using DbType = SqlSugar.DbType;

namespace Byte.Core.Business
{
    /// <summary>
    ///  数据表
    /// </summary>
    public class TableViewLogic : BaseBusinessLogic<int, TableView, TableViewRepository>
    {
        public readonly IUnitOfWork _unitOfWork;
        readonly TableColumnRepository _tableColumnRepository;
        public TableViewLogic(  TableViewRepository repository, IUnitOfWork unitOfWork, TableColumnRepository tableColumnRepository) : base(repository)
        {
            _unitOfWork = unitOfWork;
            _tableColumnRepository = tableColumnRepository;
        }


        #region 表头信息设置
        /// <summary>
        /// 获取表字段
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableView> GetTableHeaderAsync(TableViewParam param)
        {
            var sysList = new List<TableColumn>();
            if (param.Tableof.IsNullOrEmpty()) throw new BusException("表名不能为空");
            else if (param.Tableof.Substring(param.Tableof.Length - 3) == "DTO")
            {
                sysList = GetXml(param.Tableof);
            }
            else {

                sysList = await GetTableColumnAsync(param.Tableof);
            }





            //获取自定义字段
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof && x.Type == param.Type).Includes(x=>x.TableColumns).FirstAsync();

            if (entity == null) throw new BusException("暂未创建模型,请创建模型", ParamConfig.OK);

            var keys1 = entity?.TableColumns?.Select(x => x.Key).ToList()??  new List<string>();
            var keys2 = sysList.Select(x => x.Key).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableColumn>();
            foreach (var item in keys)
            {
                var model = entity?.TableColumns?.FirstOrDefault(x => x.Key == item);
                model ??= sysList.FirstOrDefault(x => x.Key == item);
                list.Add(model);
            }
            list.ForEach(x=>x.ViewId=entity.Id);
             entity.TableColumns = list;
            return entity;
            // var aa = await _unitOfWork.GetDbClient().SqlQueryable<TableViewColumnDTO>(sql).ToListAsync();
            //return aa;
        }
        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> AddAsync(UpdateTableViewParam param)
        {
            TableView model = param.Adapt<TableView>();
            await AddAsync(model);
            return model.Id;
        }

        /// <summary>
        /// 添加模型
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(UpdateTableViewParam param)
        {
            var entity = await GetIQueryable(x => x.Id == param.Id).FirstOrDefaultAsync();
            entity.Tableof = param.Tableof;
            entity.Type = param.Type;
            entity.Router = param.Router;
            await UpdateAsync(entity);
            return param.Id;
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
        /// 排序
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SetTableSortAsync(List<TableSortParam> param)
        {
            var tableColumns = param.Select(x => new TableColumn { Id = x.Id, Sort = x.Sort }).ToList();
            await _unitOfWork.GetDbClient().Updateable(tableColumns ).UpdateColumns(s => new { s.Sort}).ExecuteCommandAsync();
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
        public async Task<TableView> GetViewAsync(TableViewParam param)
        {
            var entity = await GetIQueryable(x => x.Tableof == param.Tableof &x.Type  == param.Type )
                .Includes(x=>x.TableColumns.Where(y=>y.IsShow).OrderBy(x => !x.IsShow && string.IsNullOrEmpty(x.Props)).ThenBy(x => x.Sort).ToList()).FirstAsync();
            entity?.TableColumns?.ForEach(x => x.Key= x.Key.ToFirstLowerStr());

            return entity;
        }
        /// <summary>
        /// 分页
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResults<dynamic>> PageAsync(TableViewPageParam param)
        {
            var sql = $"select * from {param.Tableof}".ToSqlFilter();
            var page = await _unitOfWork.GetDbClient().SqlQueryable<dynamic>(sql).SearchWhere(param).ToPagedResultsAsync(param);
            return page;
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


        #endregion
        #region 私有方法

        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private List<TableColumn> GetXml(string tableName)
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
            var list = new List<TableColumn>();
            for (int i = 0; i < props.Length; i++)
            {
                MemberInfo prop = props[i];
                var common = xmlCommentHelper.GetFieldOrPropertyComment(prop);
                var model = new TableColumn()
                {
                    Table = tableName.Trim(),
                    Title =  common.Trim(),
                    Key = prop.Name.ToFirstLowerStr(),//转小写,
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
        private async Task<List<TableColumn>>  GetTableColumnAsync(string tableName){

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
    var columnView = await _unitOfWork.GetDbClient().SqlQueryable<DataColumn>(sql).
        Where("table_schema = @schema And TableName =@tableName ", new
        {
            schema = _unitOfWork.GetDbClient().CurrentConnectionConfig.ConfigId,
            tableName = tableName
        })
    .ToListAsync();
     var columns = columnView.Select(x=>new  TableColumn{ 
          Table = x.TableName,
           Key= x.ColumnKey,
         Title =x.Common,

     }).ToList();
   return columns;
    #endregion
}

    }
}
