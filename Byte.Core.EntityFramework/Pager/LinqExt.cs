using Byte.Core.Common.Extensions;
using Byte.Core.Common.Pager;
using Byte.Core.EntityFramework.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace Byte.Core.EntityFramework.Pager
{

    public static class LinqExt
    {

        /// <summary>
        /// 动态排序法
        /// </summary>
        /// <typeparam name="T">实体类型</typeparam>
        /// <param name="source">IQueryable数据源</param>
        /// <param name="sortColumn">排序的列</param>
        /// <param name="sortType">排序的方法</param>
        /// <returns></returns>
        public static IOrderedQueryable<T> ThenBy<T>(this IQueryable<T> source, string sortColumn, string sortType)
        {
            //return source.OrderBy(new KeyValuePair<string, string>(sortColumn, sortType));
            return source.OrderBy($"{sortColumn} {sortType}");
        }


        public static PagedResults<TOut> ToPagedResults<TIn, TOut>(this IQueryable<TIn> source, PageParam queryParam, Func<TIn, TOut> selectFunc = null)

        {

            if (queryParam.Props != null && queryParam.Props.Count > 0)
            {
                var type = typeof(TIn);
                foreach (KeyValuePair<string, string> sort in queryParam.Props)
                {
                    var key = sort.Key.ToFirstUpperStr();
                    Expression<Func<TIn, bool>> newWhere = x => true;

                    var prop = type.GetProperty(key);
                    if (prop.PropertyType == typeof(String))
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<TIn, bool>(
                         ParsingConfig.Default, false, $@"{key}.Contains(@0)", sort.Value);
                    }
                    else
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<TIn, bool>(
                            ParsingConfig.Default, false, $@"{key}==@0", sort.Value);
                    }
                    source = source.Where(newWhere);
                }
            }
            if (queryParam.StartIndex < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于0");
            }

            if (queryParam.PageSize <= 0)
            {
                throw new InvalidOperationException("每页记录数不能小于0");
            }

            var pagerInfo = new PagerInfo(queryParam)
            {
                TotalRowCount = source.Count()
            };
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }

            IOrderedQueryable<TIn> orderedQueryable = null;
            if (queryParam.SortList != null && queryParam.SortList.Count > 0)
            {
                int num = 0;
                foreach (KeyValuePair<string, string> sort in queryParam.SortList)
                {

                    if (!string.IsNullOrWhiteSpace(sort.Value))
                        orderedQueryable = ((num++ == 0) ? source.OrderBy($"{sort.Key} {sort.Value}"  ) : orderedQueryable.ThenBy($"{sort.Key} {sort.Value}" ));
                }
            }
            else
            {
                orderedQueryable = source.OrderBy($"{queryParam.SortName} {queryParam.SortOrderDesc}");
            }

          
            

            IQueryable<TIn> queryable = orderedQueryable;
            IQueryable<TIn> source2 = queryable ?? source;
            if (selectFunc == null)
            {
                selectFunc = ObjectFastClone<TIn, TOut>.GetFunc();
            }
            //.AsEnumerable() 
            List<TOut> data = source2.Skip(pagerInfo.StartIndex).Take(queryParam.PageSize)
                .Select(selectFunc)
                .ToList();
            return new PagedResults<TOut>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
        }


        public static async Task<PagedResults<TOut>> ToPagedResultsAsync<TIn, TOut>(this IQueryable<TIn> source, PageParam queryParam, Func<TIn, TOut> selectFunc = null)

        {


            if (queryParam.Props != null && queryParam.Props.Count > 0)
            {
                var type = typeof(TIn);
                foreach (KeyValuePair<string, string> sort in queryParam.Props)
                {
                    var key = sort.Key.ToFirstUpperStr();
                    Expression<Func<TIn, bool>> newWhere = x => true;

                    var prop = type.GetProperty(key);
                    if (prop.PropertyType == typeof(String))
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<TIn, bool>(
                         ParsingConfig.Default, false, $@"{key}.Contains(@0)", sort.Value);
                    }
                    else
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<TIn, bool>(
                            ParsingConfig.Default, false, $@"{key}==@0", sort.Value);
                    }
                    source = source.Where(newWhere);
                }
            }
            if (queryParam.StartIndex < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于0");
            }

            if (queryParam.PageSize <= 0)
            {
                throw new InvalidOperationException("每页记录数不能小于0");
            }

            var pagerInfo = new PagerInfo(queryParam)
            {
                TotalRowCount = await source.CountAsync()
            };
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }

            IOrderedQueryable<TIn> orderedQueryable = null;
            if (queryParam.SortList != null && queryParam.SortList.Count > 0)
            {
                int num = 0;
                foreach (KeyValuePair<string, string> sort in queryParam.SortList)
                {
                    if (!string.IsNullOrWhiteSpace(sort.Value))
                        orderedQueryable = ((num++ == 0) ? source.OrderBy($"{sort.Key} {sort.Value}") : orderedQueryable.ThenBy($"{sort.Key} {sort.Value}"));
                }
            }
            else
            {
                orderedQueryable = source.OrderBy($"{queryParam.SortName} {queryParam.SortOrderDesc}");
            }
          


            IQueryable<TIn> queryable = orderedQueryable;
            IQueryable<TIn> source2 = queryable ?? source;
            if (selectFunc == null)
            {
                selectFunc = ObjectFastClone<TIn, TOut>.GetFunc();
            }
            //.AsEnumerable() 
            List<TOut> data = await source2
               .Skip(pagerInfo.StartIndex)
               .Take(queryParam.PageSize).Select(selectFunc).ToDynamicListAsync<TOut>();
            return new PagedResults<TOut>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
        }

        public static async Task<PagedResults<T>> ToPagedResultsAsync<T>(this IQueryable<T> source, PageParam queryParam)
        {

            if (queryParam.Props != null && queryParam.Props.Count > 0)
            {
                var type = typeof(T);
                foreach (KeyValuePair<string, string> sort in queryParam.Props)
                {
                    var key = sort.Key.ToFirstUpperStr();
                    Expression<Func<T, bool>> newWhere = x => true;

                    var prop = type.GetProperty(key);
                    if (prop.PropertyType == typeof(String))
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                         ParsingConfig.Default, false, $@"{key}.Contains(@0)", sort.Value);
                    }
                    else
                    {
                        newWhere = DynamicExpressionParser.ParseLambda<T, bool>(
                            ParsingConfig.Default, false, $@"{key}==@0", sort.Value);
                    }
                    source = source.Where(newWhere);
                }
            }
            if (queryParam.StartIndex < 0)
            {
                throw new InvalidOperationException("起始记录数不能小于0");
            }

            if (queryParam.PageSize <= 0)
            {
                throw new InvalidOperationException("每页记录数不能小于0");
            }

            var pagerInfo = new PagerInfo(queryParam)
            {
                TotalRowCount = source.Count()
            };
            if (queryParam.StartIndex >= pagerInfo.TotalRowCount)
            {
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != 0) ? pagerInfo.TotalRowCount : 0);
                pagerInfo.StartIndex = ((pagerInfo.StartIndex != -1) ? pagerInfo.StartIndex : 0);
            }

            IOrderedQueryable<T> orderedQueryable = null;
            if (queryParam.SortList != null && queryParam.SortList.Count > 0)
            {
                int num = 0;
                foreach (KeyValuePair<string, string> sort in queryParam.SortList)
                {

                    if (!string.IsNullOrWhiteSpace(sort.Value))
                        orderedQueryable = ((num++ == 0) ? source.OrderBy($"{sort.Key} {sort.Value}") : orderedQueryable.ThenBy($"{sort.Key} {sort.Value}"));
                }
            }
            else
            {
                orderedQueryable = source.OrderBy($"{queryParam.SortName} {queryParam.SortOrderDesc}");
            }



            IQueryable<T> queryable = orderedQueryable;
            IQueryable<T> source2 = queryable ?? source;
          
            //.AsEnumerable() 
            List<T> data = source2.Skip(pagerInfo.StartIndex)
                .Take(queryParam.PageSize)
                .ToList();
            return new PagedResults<T>
            {
                PagerInfo = pagerInfo,
                Data = data
            };
        }

    }
}

