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
    /// <summary>
    /// 
    /// </summary>
    public class TableColumnLogic : BaseBusinessLogic<Guid, TableColumn, TableColumnRepository>
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="repository"></param>
        public TableColumnLogic(TableColumnRepository repository) : base(repository)
        {
        } 


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<TableColumnDTO>> GetPageAsync([FromQuery] TableColumnParam param)
        {
            Expression<Func<TableColumn, bool>> where = x => true;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Table.Contains(param.KeyWord) || x.Title.Contains(param.KeyWord));
            }
            var page = await GetIQueryable(where).Select<TableColumnDTO>().ToPagedResultsAsync(param);

            return page;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<TableColumnInfo> GetInfoAsync(Guid id)
        {
            var entity = await GetIQueryable(x => x.Id == id).Select<TableColumnInfo>().FirstAsync();
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



            await UpdateAsync(x => x.Id == param.Id, x => new TableColumn
            {


            });

            return param.Id;
        }


        /// <summary>
        /// 获取头部列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableModel> GetColumnsAsync(TableGetColumnParam param)
        {

            var sysList = GetXml(param.Table);
            sysList.Router = param.Router;
            var entity = await GetIQueryable(x => x.Table == param.Table && x.Router == param.Router).ToListAsync();

            //sysList.Columns = sysList.Columns.Select(x => {

            //    var model = entity.FirstOrDefault(y => y.Title == x.Title);

            //    x.Router = param.Router;
            //    return x;
            //}).OrderBy(x => x.Sort).ToList();

            var keys1 = entity.Select(x => x.Key).ToList();
            var keys2 = sysList.Columns.Select(x => x.Key).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableColumn>();
            foreach (var item in keys)
            {
                var model = entity.FirstOrDefault(x => x.Key == item);
                model ??= sysList.Columns.FirstOrDefault(x => x.Key == item);
                list.Add(model);
            }

            var table = new TableModel()
            {   Table = param.Table,
                Router = param.Router,
                Columns = list
            };
            return table;
        }



        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private TableModel GetXml(string table)
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
            Type type = assIBll.GetType($"{typeName}.{table}");//获取类名，必须 命名空间+类名 


            var props = type.GetProperties();

            var entity = new TableModel();
            entity.Table = table;
            //entity.Comment = xmlCommentHelper.GetComment($"T:{type.FullName}", "summary");
            var list = new List<TableColumn>();
            for (int i = 0; i < props.Length; i++)
            {
                MemberInfo prop = props[i];
                var title = xmlCommentHelper.GetFieldOrPropertyComment(prop);
                var sortable = true;

                var model = new TableColumn()
                {
                    Id = Guid.NewGuid(),
                    Title = title.Trim(),
                    Key = prop.Name.ToFirstLowerStr(),//转小写,
                    Table = table,
                     
                    Sort = i + 99,
                    //Sortable = sortable,
                };
                list.Add(model);
            }
            entity.Columns = list;

            return entity;
        }




        /// <summary>
        /// 设置列表
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task SetColumnsAsync(TableModel param)
        {
            var num = await DeleteAsync(x => x.Table == param.Table && x.Router == param.Router);
            param.Columns?.ForEach(x =>
            {
                x.Router = param.Router;
                x.Table = param.Table;
            });
            num += await AddRangeAsync(param.Columns);
        }

    }
}
