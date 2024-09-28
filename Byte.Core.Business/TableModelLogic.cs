using Byte.Core.Common.Extensions;
using Byte.Core.Common.Helpers;
using Byte.Core.Entity;
using Byte.Core.Models;
using Byte.Core.Repository;
using Byte.Core.SqlSugar;
using Mapster;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;
using System.Reflection;

namespace Byte.Core.Business
{
    /// <summary>
    /// 
    /// </summary>
    public class TableModelLogic : BaseBusinessLogic<Guid, TableModel, TableModelRepository>
    {
        readonly IUnitOfWork _unitOfWork;

        /// <summary></summary>
        /// <param name="repository"></param>
        /// <param name="unitOfWork"></param>
        public TableModelLogic(TableModelRepository repository, IUnitOfWork unitOfWork) : base(repository)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// 分页
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<PagedResults<TableModelDTO>> GetPageAsync([FromQuery] TableModelParam param)
        {
            Expression<Func<TableModel, bool>> where = x => true;
            if (!string.IsNullOrWhiteSpace(param.KeyWord))
            {
                param.KeyWord = param.KeyWord.Trim();
                where = where.And(x => x.Table.Contains(param.KeyWord));
            }

            var page = await GetIQueryable(where).Select<TableModelDTO>().ToPagedResultsAsync(param);

            return page;
        }



        /// <summary>
        /// 查询详情
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<TableModelInfo> GetInfoAsync(TableModelInfoParam param)
        {
            Expression<Func<TableModel, bool>> where = x => true;
            //if (param.Id!=null) {
            //    where = where.And(x => x.Id==param.Id);
            //}
            //else{
            //    where = where.And(x => x.Table == param.Table && x.Router == param.Router);
            //}
     
            var entity = await Repository.GetIQueryable(where)
                .Includes(x => x.TableColumns.OrderBy(column => column.Sort).ToList())
                .Select(x => new TableModelInfo()
                {
                    TableColumns = x.TableColumns,
                }, true).FirstAsync();

            var sysList = GetXml(param.Table);
            if (entity == null)
            {
                return sysList;
            }

            var keys1 = entity.TableColumns.Select(x => x.Key).ToList();
            var keys2 = sysList.TableColumns.Select(x => x.Key).ToList();
            var keys = keys1.Union(keys2).Distinct().ToList();
            var list = new List<TableColumn>();
            foreach (var item in keys)
            {
                var model = entity.TableColumns.FirstOrDefault(x => x.Key == item);
                model ??= sysList.TableColumns.FirstOrDefault(x => x.Key == item);
                list.Add(model);
            }

            entity.TableColumns = list;
            return entity;
        }


        /// <summary>
        /// 添加
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> AddAsync(AddTableModelParam param)
        {
            TableModel model = param.Adapt<TableModel>();
            //var aa = model.TableColumns.FirstOrDefault().Id;
            //await AddAsync(model);
            await _unitOfWork.GetDbClient().InsertNav(model).Include(x => x.TableColumns).ExecuteCommandAsync();
            return model.Id;
        }

        /// <summary>
        /// 编辑
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public async Task<Guid> UpdateAsync(UpdateTableModelParam param)
        {
            TableModel model = param.Adapt<TableModel>();

            await _unitOfWork.GetDbClient().UpdateNav(model).Include(x => x.TableColumns).ExecuteCommandAsync();

            return param.Id;
        }




        /// <summary>
        /// 反射中找到XML
        /// </summary>
        /// <param name="table"></param>
        /// <returns></returns>
        private TableModelInfo GetXml(string table)
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
            Type type = assIBll.GetType($"{typeName}.{table}"); //获取类名，必须 命名空间+类名 


            if (type != null)
            {
                var keys = type.GetProperties();

                var entity = new TableModelInfo();
                entity.Table = table;
                entity.Comment = xmlCommentHelper.GetComment($"T:{type.FullName}", "summary");
                var list = new List<TableColumn>();
                for (int i = 0; i < keys.Length; i++)
                {
                    MemberInfo key = keys[i];
                    var title = xmlCommentHelper.GetFieldOrPropertyComment(key);
                    var model = new TableColumn()
                    {
                        Title = title.Trim(),
                        Key = key.Name.ToFirstLowerStr(), //转小写,
                        Sort = i + 99,
                    };
                    list.Add(model);
                }

                entity.TableColumns = list;

                return entity;
            }

            return new();
        }
    }




}
